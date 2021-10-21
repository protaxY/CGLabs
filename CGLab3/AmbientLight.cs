using System.Numerics;

namespace CG
{
    public class AmbientLight
    {
        public Vector3 Color;

        AmbientLight(Vector3 color)
        {
            Color = color;
        }
    }
}