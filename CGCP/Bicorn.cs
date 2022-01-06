using System;
using System.Collections.Generic;
using System.Numerics;

namespace CGCP
{
    public class Bicorn
    {
        public float a = 1;

        public Bicorn(float a)
        {
            this.a = a;
        }
        
        public List<Vector4> Interpolate(int quality)
        {
            List<Vector4> result = new List<Vector4>();
            for (float theta = (float) -Math.PI; theta < (float) Math.PI; theta += (float) (2 * Math.PI / quality))
            {
                result.Add(new Vector4((float) (a * Math.Sin(theta)),
                    (float) (a * Math.Pow(Math.Cos(theta), 2) * (2 + Math.Cos(theta)) /
                             (3 + Math.Pow(Math.Sin(theta), 2))), 0, 1));
            }

            return result;
        }
    }
}