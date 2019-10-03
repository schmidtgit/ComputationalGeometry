using System;
using PolygonTriangulation.Model;

namespace PolygonTriangulation.ImplicitObjects {
	public class Torus : SDF {
		private float _inner;
		private float _radius;

		/// <summary>
		/// Creates a torus with the desired size.
		/// </summary>
		/// <remarks>
		/// The torus is centered around (0,0,0).
		/// </remarks>
		/// <param name="distFromCenter">The distance from the center point to the center of the torus.</param>
		/// <param name="radius">The radius of the torus itself.</param>
		public Torus(float distFromCenter, float radius) {
			_inner = distFromCenter;
			_radius = radius;
		}
		
		public override float Distance(Vec3 p) {
			var temp = Math.Sqrt(p.X * p.X + p.Z * p.Z) - _inner;
			return (float) Math.Sqrt(temp * temp + p.Y * p.Y) - _radius;
		}

		public override string ToString() {
			return $"Torus h{_inner}/r{_radius}";
		}

		public override int RequiredGridSize() {
			return (int) Math.Ceiling(((_inner + _radius) * 2) + epsilon);
		}

		public override bool Precise() { return true; }
	}
}
