using System;
using System.Collections.Generic;
using System.Numerics;

namespace CGLab7
{
    public class CardinalSpline
    {
        public List<Vector4> Points;
        public float c = 0.5f;

        public CardinalSpline(Vector4 a, Vector4 b, Vector4 c, Vector4 d, Vector4 e)
        {
            Points = new List<Vector4>{a, b, c, d, e};
        }

        public float Interpolate(int i, float t)
        {
            return h_00(t) * Points[i].Y + h_10(t) * m(i) +
                   h_01(t) * Points[i + 1].Y + h_11(t) * m(i + 1);
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

        private float m(int i)
        {
            return (1 - c) * (Points[i + 1].Y - Points[i - 1].Y) / (Points[i + 1].X - Points[i - 1].X);
        }
    }
}