using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PolygonTriangulation.Model;

namespace PolygonTriangulation.ImplicitObjects {
	public class Box : SDF {
		private Vec3 _v;

		/// <summary>
		/// Creates a box with the given sidelengths.
		/// </summary>
		/// <remarks>
		/// The box is centered around (0,0,0).
		/// </remarks>
		/// <param name="x">The length in the x - axis.</param>
		/// <param name="y">The length in the y - axis.</param>
		/// <param name="z">The length in the z - axis.</param>
		public Box(float x, float y, float z) {
			_v = new Vec3(x, y, z);
		}

		public override float Distance(Vec3 p) {
			float x = Math.Max(p.X - new Vec3(_v.X * 0.5f, 0, 0).Magnitude(), - p.X - new Vec3(_v.X * 0.5f, 0, 0).Magnitude());
			float y = Math.Max (p.Y - new Vec3(_v.Y * 0.5f, 0, 0).Magnitude(), - p.Y - new Vec3(_v.Y * 0.5f, 0, 0).Magnitude());
			float z = Math.Max(p.Z - new Vec3(_v.Z * 0.5f, 0, 0).Magnitude(), - p.Z - new Vec3(_v.Z * 0.5f, 0, 0).Magnitude());
			float d = Math.Max(x, y);
			d = Math.Max(d, z);
			return d;
		}

		public override string ToString() {
			return $"Box {_v.Y}x{_v.X}x{_v.Z}";
		}

		public override int RequiredGridSize() {
			return (int) Math.Ceiling(Math.Max(_v.X, Math.Max(_v.Y, _v.Z)) + epsilon);
		}

		public override bool Precise() {
			return true;
		}
	}
}
