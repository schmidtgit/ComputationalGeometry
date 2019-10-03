using System;
using PolygonTriangulation.Model;

namespace PolygonTriangulation.ImplicitObjects {
	public class Rotation : SDF {
		private SDF _original;
		private float x, y, z;
        private float cxcy, cxczsypsxsz, cxczpsxsysz, cxsyszmczsx,
            cycz, cysx, cysz, czsxszmcxsz, msy;

		/// <summary>
		/// Rotate the object around x y and z in the order of z y and x.
		/// </summary>
		/// <remark>
		/// Values are given in radians.
		/// </remark>
		/// <param name="original">SDF to rotate.</param>
		/// <param name="xrotation">Rotation around x-axis given in radians.</param>
		/// <param name="yrotation">Rotation around y-axis given in radians.</param>
		/// <param name="zrotation">Rotation around z-axis given in radians.</param>
		public Rotation(SDF original, float xrotation, float yrotation, float zrotation) {
			_original = original;
			x = xrotation;
			y = yrotation;
			z = zrotation;
            Initialize();
		}

		/// <summary>
		/// Rotate the object around x y and z in the order of z y and x.
		/// </summary>
		/// <remark>
		/// Values are given in degrees.
		/// </remark>
		/// <param name="original">SDF to rotate.</param>
		/// <param name="xrotation">Rotation around x-axis given in degrees.</param>
		/// <param name="yrotation">Rotation around y-axis given in degrees.</param>
		/// <param name="zrotation">Rotation around z-axis given in degrees.</param>
		public Rotation(SDF original, int xdegrees, int ydegrees, int zdegrees) {
			_original = original;
			x = (float) (xdegrees % 360 * Math.PI) / 180;
			y = (float) (ydegrees % 360 * Math.PI) / 180;
			z = (float) (zdegrees % 360 * Math.PI) / 180;
            Initialize();
		}

        private void Initialize() {
            cycz = (float) (Math.Cos(y) * Math.Cos(z));
            czsxszmcxsz = (float) (Math.Cos(z) * Math.Sin(x) * Math.Sin(y) - Math.Cos(x) * Math.Sin(z));
            cxczsypsxsz = (float) (Math.Cos(x) * Math.Cos(z) * Math.Sin(y) + Math.Sin(x) * Math.Sin(z));
            cysz = (float) (Math.Cos(y) * Math.Sin(z));
            cxczpsxsysz = (float) (Math.Cos(x) * Math.Cos(z) + Math.Sin(x) * Math.Sin(y) * Math.Sin(z));
            cxsyszmczsx = (float) (Math.Cos(x) * Math.Sin(y) * Math.Sin(z) - Math.Cos(z) * Math.Sin(x));
            msy = (float) -Math.Sin(y);
            cysx = (float) (Math.Cos(y) * Math.Sin(x));
            cxcy = (float) (Math.Cos(x) * Math.Cos(y));
        }

		public override float Distance(Vec3 p) {
			float xx = p.X*cycz + p.Y*czsxszmcxsz + p.Z*cxczsypsxsz;
			float yy = p.X* cysz + p.Y* cxczpsxsysz + p.Z* cxsyszmczsx;
			float zz = p.X*msy + p.Y* cysx + p.Z* cxcy;

			return _original.Distance(new Vec3(xx, yy, zz));
		}

		public override string ToString() {
			var first = _original == null ? "_____" : _original.ToString();
			return $"{first} rotated {Math.Round(x * 180 / Math.PI)}°|{Math.Round(y * 180 / Math.PI)}°|{Math.Round(z * 180 / Math.PI)}°";
		}

		public override bool Precise() {
			return _original.Precise();
		}

        // Assumes the worst case instead of calculating the actual grid.
        // This is the distance between two opposite corners.
        public override int RequiredGridSize() {
            return (int) Math.Ceiling(Math.Sqrt((_original.RequiredGridSize() * _original.RequiredGridSize())*3));
        }
    }
}
