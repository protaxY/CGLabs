using System.Numerics;
using System.Collections.Generic;

namespace CG
{
    public class PointLight
    {
        public Vector4 Position;
        public Vector3 Intensity;

        PointLight(Vector4 position, Vector3 intensity)
        {
            Position = position;
            Intensity = intensity;
        }
    }
}