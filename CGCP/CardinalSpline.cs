using System;
using System.Collections.Generic;
using System.Numerics;
using CG;

namespace CGCP
{
    public class CardinalSpline
    {
        public List<Vector4> Points;
        public float c = 0.5f;

        public CardinalSpline(Vector4 a, Vector4 b, Vector4 c, Vector4 d, Vector4 e)
        {
            Points = new List<Vector4>{a, b, c, d, e};
        }

        public List<Vector4> Interpolate(int quality)
        {
            List<Vector4> result = new List<Vector4>();
            
            for (int i = 1; i < Points.Count - 2; ++i)
            {
                for (int j = 0; j <= quality; ++j)
                {
                    result.Add(Interpolate(i, (float)j / (float)quality));
                }
            }

            return result;
        }

        private Vector4 Interpolate(int i, float t)
        {
            return new Vector4(h_00(t) * Points[i].X + h_10(t) * m(i, 'X') +
                               h_01(t) * Points[i + 1].X + h_11(t) * m(i + 1, 'X'),
                h_00(t) * Points[i].Y + h_10(t) * m(i, 'Y') +
                h_01(t) * Points[i + 1].Y + h_11(t) * m(i + 1, 'Y'),
                h_00(t) * Points[i].Z + h_10(t) * m(i, 'Z') +
                h_01(t) * Points[i + 1].Z + h_11(t) * m(i + 1, 'Z'),
                1);
        }

        private float h_00(float t)
        {
            return 2 * (float) Math.Pow(t, 3) - 3 * (float) Math.Pow(t, 2) + 1;
        }
        
        private float h_10(float t)
        {
            return (float) Math.Pow(t, 3) - 2 * (float) Math.Pow(t, 2) + t;
        }
        
        private float h_01(float t)
        {
            return -2 * (float) Math.Pow(t, 3) + 3 * (float) Math.Pow(t, 2);
        }
        
        private float h_11(float t)
        {
            return (float) Math.Pow(t, 3) - (float) Math.Pow(t, 2);
        }

        private float m(int i, char coord)
        {
            switch (coord)
            {
                case 'X':
                    return (1 - c) * (Points[i + 1].X - Points[i - 1].X) / (Points[i + 1].X - Points[i - 1].X);
                case 'Y':
                    return (1 - c) * (Points[i + 1].Y - Points[i - 1].Y) / (Points[i + 1].X - Points[i - 1].X);
                case 'Z':
                    return (1 - c) * (Points[i + 1].Z - Points[i - 1].Z) / (Points[i + 1].X - Points[i - 1].X);
                default:
                    return 0;
            }
        }
    }
}