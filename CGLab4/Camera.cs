using System;
using System.Numerics;


namespace CG
{
    public class Camera
    {
        public Vector3 Position;
        public Vector3 Target;
        public Vector3 Up;
        public float AspectRatio;
        public float FOV;
        public float ClipStart;
        public float ClipEnd;
        
        public Camera(Vector3 position, Vector3 target, Vector3 up, float aspectRatio, float fov, float clipStart, float clipEnd)
        {
            Position = position;
            Target = target;
            Up = up;
            AspectRatio = aspectRatio;
            FOV = fov;
            ClipStart = clipStart;
            ClipEnd = clipEnd;
        }
        
        // объектно-видовая матрица камеры
        public Matrix4x4 GetViewMatrix()
        {
            var cameraDirection = Vector3.Normalize(Position - Target);
            var cameraRight = Vector3.Cross(Up, cameraDirection);
            var matrix1 = new Matrix4x4(
                cameraRight.X, cameraRight.Y, cameraRight.Z, 0,
                Up.X, Up.Y, Up.Z, 0,
                cameraDirection.X, cameraDirection.Y, cameraDirection.Z, 0,
                0, 0, 0, 1
            );
            var matrix2 = new Matrix4x4(
                1, 0, 0, -Position.X,
                0, 1, 0, -Position.Y,
                0, 0, 1, -Position.Z,
                0, 0, 0, 1
            );
            return matrix1 * matrix2;
        }

        // матрица проекции (применить после перехода в базис камеры)
        public Matrix4x4 GetProjectionMatrix()
        {
            var sin = (float)Math.Sin(FOV);
            var cotan = (float)Math.Cos(FOV) / sin;
            var clip = ClipEnd - ClipStart;
            return new Matrix4x4(
                cotan/AspectRatio,     0,     0,      0,
                0,                 cotan,     0,      0,
                0, 0, -(ClipStart+ClipEnd)/clip, -(2f*ClipStart*ClipEnd)/clip,
                0,                     0,     -1,     1
            );
        }

        public Vector3 GetRightVector()
        {
            return Vector3.Normalize(Vector3.Cross(Up, Position - Target));
        }
    }
}