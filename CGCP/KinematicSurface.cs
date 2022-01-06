using System;
using System.Collections.Generic;
using System.Numerics;

namespace CGCP
{
    public class KinematicSurface
    {
        public CardinalSpline GuideCurve;
        public Bicorn GeneratingCurve;

        public KinematicSurface(CardinalSpline guideCurve, Bicorn generatingCurve)
        {
            GuideCurve = guideCurve;
            GeneratingCurve = generatingCurve;
        }

        public List<Vector4> Interpolate(int guideCurveQuality, int generatingCurveQuality, out List<uint> indexes)
        {
            List<Vector4> result = new List<Vector4>();
            indexes = new List<uint>();

            List<Vector4> guideCurveInterpolation = GuideCurve.Interpolate(guideCurveQuality);
            List<Vector4> generatingCurveInterpolation = GeneratingCurve.Interpolate(generatingCurveQuality);

            for (int i = 1; i < guideCurveInterpolation.Count - 1; ++i)
            {
                Matrix4x4 transformation = Matrix4x4.CreateTranslation(guideCurveInterpolation[i].X,
                                                                        guideCurveInterpolation[i].Y,
                                                                        guideCurveInterpolation[i].Z);
                transformation *= Matrix4x4.CreateRotationY((float)Math.Acos(((guideCurveInterpolation[i + 1] - guideCurveInterpolation[i]) /
                                                                              (guideCurveInterpolation[i + 1] - guideCurveInterpolation[i]).Length()).Y));
                transformation *= Matrix4x4.CreateRotationZ((float)Math.Acos(((guideCurveInterpolation[i + 1] - guideCurveInterpolation[i]) /
                                                                              (guideCurveInterpolation[i + 1] - guideCurveInterpolation[i]).Length()).Z));
                result.AddRange(TransformedGuideCurveInterpolation(generatingCurveQuality, transformation));
            }

            for (int i = 0; i < result.Count - generatingCurveQuality; ++i)
            {
                indexes.Add((uint) i);
                indexes.Add((uint) (i + generatingCurveQuality));
            }

            return result;
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

        // public Vector4 CalculateNormal(uint i)
        // {
        //     if ()
        //     {
        //         
        //     }
        // }
    }
}