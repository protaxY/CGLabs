using System;
using System.Collections.Generic;
using System.Numerics;
using CG;

namespace CGCP
{
    public class KinematicSurface: Mesh
    {
        public CardinalSpline GuideCurve;
        public Bicorn GeneratingCurve;
        public int GuideCurveQuality = 10;
        public int GeneratingCurveQuality = 10;

        public KinematicSurface(CardinalSpline guideCurve, Bicorn generatingCurve)
        {
            GuideCurve = guideCurve;
            GeneratingCurve = generatingCurve;
            Update();
        }

        public void Update()
        {
            List<List<Vector4>> interpolation = new List<List<Vector4>>();
            
            #region генерация точек

            List<Vector4> bicornInterpolation = GeneratingCurve.Interpolate(GeneratingCurveQuality);
            Matrix4x4 bicornTransformation = Matrix4x4.CreateRotationZ((float)(Math.PI / 2));
            bicornTransformation *= Matrix4x4.CreateRotationY((float)(Math.PI / 2));
            for (int i = 0; i < bicornInterpolation.Count; ++i)
            {
                bicornInterpolation[i] = Vector4.Transform(bicornInterpolation[i], bicornTransformation);
            }

            List<Vector4> splineInterpolation = GuideCurve.Interpolate(GuideCurveQuality);
            List<float> splineRotationInterpolation = GuideCurve.InterpolateRotations(GuideCurveQuality);

            for (int i = 0; i < splineInterpolation.Count; ++i)
            {
                Vector4 direction = new Vector4();
                if (i == splineInterpolation.Count - 1)
                {
                    direction = splineInterpolation[i] - splineInterpolation[i - 1];
                } else direction = splineInterpolation[i + 1] - splineInterpolation[i];
                direction /= direction.Length();

                float phi = (float) Math.Atan2(direction.Y, direction.X);
                float theta = (float) Math.Acos(direction.Z);
                
                Matrix4x4 transformation = Matrix4x4.CreateRotationX(splineRotationInterpolation[i]);
                transformation *= Matrix4x4.CreateRotationZ(phi);
                transformation *= Matrix4x4.CreateRotationY(theta - (float) (Math.PI / 2));
                transformation *= Matrix4x4.CreateTranslation(new Vector3(splineInterpolation[i].X,
                                                                            splineInterpolation[i].Y, 
                                                                            splineInterpolation[i].Z));

                List<Vector4> transformedBicornInterpolation = new List<Vector4>();
                for (int j = 0; j < bicornInterpolation.Count; ++j)
                {
                    transformedBicornInterpolation.Add(Vector4.Transform(bicornInterpolation[j], transformation));
                }
                
                interpolation.Add(transformedBicornInterpolation);
            }

            #endregion

            #region формирование полигонов

            Vertices.Clear();
            Polygons.Clear();
            
            uint cnt = 0;
            for (int i = 0; i < interpolation.Count - 1; ++i)
            {
                for (int j = 0; j < interpolation[i].Count; ++j)
                {
                    Vertex a = new Vertex(interpolation[i][j], cnt);
                    Vertex b = new Vertex(interpolation[i + 1][j], cnt + 1);
                    Vertex c;
                    if (j == interpolation[i].Count - 1)
                        c = new Vertex(interpolation[i + 1][0], cnt + 3);
                    else c = new Vertex(interpolation[i + 1][j + 1], cnt + 3);
                    Vertex d;
                    if (j == interpolation[i].Count - 1)
                        d = new Vertex(interpolation[i][0], cnt + 2);
                    else d = new Vertex(interpolation[i][j + 1], cnt + 2);
                    cnt += 4;
                    Vertices.Add(a);
                    Vertices.Add(b);
                    Vertices.Add(d);
                    Vertices.Add(c);
                    
                    Polygons.Add(new Polygon(new List<Vertex>() {c, b, a}));
                    Polygons.Add(new Polygon(new List<Vertex>() {a, d, c}));
                }
            }

            #endregion
        }

        private List<Vector4> TransformedGuideCurveInterpolation(int generatingCurveQuality, Matrix4x4 transformation)
        {
            List<Vector4> generatingCurveInterpolation = GeneratingCurve.Interpolate(generatingCurveQuality);
            for (int i = 0; i < generatingCurveInterpolation.Count; ++i)
            {
                generatingCurveInterpolation[i] = Vector4.Transform(generatingCurveInterpolation[i], transformation);
            }

            return generatingCurveInterpolation;
        }
    }
}