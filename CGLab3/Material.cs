using System.Numerics;

namespace CG
{
    public class Material
    {
        public Vector3 K_a;
        public Vector3 K_d;
        public Vector3 K_s;
        public float P;

        Material(Vector3 k_a, Vector3 k_d, Vector3 k_s)
        {
            K_a = k_a;
            K_d = k_d;
            K_s = k_s;
        }
        
        Material(float k_aR, float k_aG, float k_aB, float k_dR, float k_dG, float k_dB, float k_sR, float k_sG, float k_sB, float p)
        {
            K_a.X = k_aR;
            K_a.Y = k_aG;
            K_a.Z = k_aB;
            
            K_d.X = k_dR;
            K_d.Y = k_dG;
            K_d.Z = k_dB;
            
            K_s.X = k_sR;
            K_s.Y = k_sG;
            K_s.Z = k_sB;

            P = p;
        }
    }
}