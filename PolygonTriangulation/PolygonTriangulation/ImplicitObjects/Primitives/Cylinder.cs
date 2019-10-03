using System;
using PolygonTriangulation.Model;

namespace PolygonTriangulation.ImplicitObjects {
	public class Cylinder : SDF {
		private float _radius;
		private float halfHeight;

		/// <summary>
		/// Creates a cylinder with a given radius and height.
		/// </summary>
		/// <remarks>
		/// The cylinder is centered around (0,0,0).
		/// </remarks>
		/// <param name="radius">The desired radius of the cylinder.</param>
		/// <param name="height">The desired height of the cylinder.</param>
		public Cylinder(float radius, float height) {
			_radius = radius;
			halfHeight = height/2;
		}

		public override float Distance(Vec3 p) {
			float d = (float) Math.Sqrt(p.X * p.X + p.Z * p.Z) - _radius;
			return Math.Max(d, Math.Abs(p.Y) - halfHeight);
		}

		public override bool Precise() {
			return true;
		}

        public override int RequiredGridSize() {
            return (int) Math.Ceiling((Math.Max(_radius, halfHeight) * 2) + epsilon);
        }

		public override string ToString() {
			return $"Cylinder r{_radius}/h{halfHeight*2},";
		}
	}
}
