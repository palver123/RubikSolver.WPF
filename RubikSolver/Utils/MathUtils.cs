using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media.Media3D;

namespace RubikSolver.Utils
{
    internal static class MathUtils
    {
        ///<summary>
        /// Rotations are performed in this order: pitch, yaw.
        /// </summary>
        /// <param name="yaw">CCW rotation around Z axis.</param>
        /// <param name="pitch">CCW rotation around X axis.</param>
        internal static Matrix3D CreatePitchYawRotation(this ProjectionCamera camera, double pitch, double yaw)
        {
            var forward = camera.LookDirection; forward.Normalize();
            var up = camera.UpDirection; up.Normalize();
            var right = Vector3D.CrossProduct(forward, up);
            var yawRotation = RotationFromAxisAngle(ref up, yaw);
            var pitchRotation = RotationFromAxisAngle(ref right, pitch);
            return pitchRotation * yawRotation;
        }

        internal static Matrix3D RotationFromAxisAngle(ref Vector3D axis, double angleY)
        {
            var sinA = Math.Sin(angleY);
            var cosA = Math.Cos(angleY);
            var NCOS = 1 - cosA;
            var axy = axis.X * axis.Y;
            var axz = axis.X * axis.Z;
            var ayz = axis.Y * axis.Z;

            return
                new Matrix3D(
                    cosA + axis.X * axis.X * NCOS,
                    axy * NCOS - axis.Z * sinA,
                    axz * NCOS + axis.Y * sinA,
                    0,

                    axy * NCOS + axis.Z * sinA,
                    cosA + axis.Y * axis.Y * NCOS,
                    ayz * NCOS - axis.X * sinA,
                    0,

                    axz * NCOS - axis.Y * sinA,
                    ayz * NCOS + axis.X * sinA,
                    cosA + axis.Z * axis.Z * NCOS,
                    0,

                    0, 0, 0, 1);
        }

    }
}
