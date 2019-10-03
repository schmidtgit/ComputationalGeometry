using System;
using PolygonTriangulation.Model;

namespace PolygonTriangulation.ImplicitObjects {
	public class Sphere: SDF {
		private float _r; 

		/// <summary>
		/// Creates a sphere with the given radius.
		/// </summary>
		/// <remarks>
		/// The sphere is centered around (0,0,0).
		/// </remarks>
		/// <param name="radius">The desired radius of the sphere.</param>
		public Sphere(float radius) {
			_r = radius;
		}

		public override float Distance(Vec3 p) {
			return (float)Math.Sqrt(p.X * p.X + p.Y * p.Y + p.Z * p.Z) - _r;
		}

		public override string ToString() {
			return $"Sphere {_r}";
		}

		public override int RequiredGridSize() {
			return (int) Math.Ceiling((_r * 2) + epsilon);
		}

		public override bool Precise() {
			return true;
		}
	}
}
