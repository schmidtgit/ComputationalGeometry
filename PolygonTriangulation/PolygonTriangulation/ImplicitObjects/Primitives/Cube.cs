using System;
using PolygonTriangulation.Model;

namespace PolygonTriangulation.ImplicitObjects {
	public class Cube: SDF {
		private float _l; // Notice, half of length

		/// <summary>
		/// A cube with the given side length.
		/// </summary>
		/// <remarks>
		/// The cube is centered around (0,0,0).
		/// </remarks>
		/// <param name="length">The length of a side.</param>
		public Cube(float length) {
			_l = length / 2;
		}

		public override float Distance(Vec3 p) {
			return Math.Max(Math.Max(Math.Abs(p.X), Math.Abs(p.Y)), Math.Abs(p.Z)) - _l;
		}

		public override string ToString() {
			return $"Cube {_l}";
		}

		public override int RequiredGridSize() {
			return (int) Math.Ceiling((_l * 2) + epsilon);
		}

		public override bool Precise() {
			return false;
		}
	}
}
