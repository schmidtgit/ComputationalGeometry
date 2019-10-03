using System;
using PolygonTriangulation.Model;

namespace PolygonTriangulation.ImplicitObjects {
	public class Sine : SDF {
		private float _fastScale;
		private float _scale; // Scale only stored for "tostring"

		/// <summary>
		/// Creates a three dimensional sine function.
		/// </summary>
		/// <param name="scale">The desired scaling of the sine function.</param>
		public Sine(float scale) {
			_scale = scale;
			_fastScale = 1 / _scale;
		}

		public override float Distance(Vec3 p) {
			return (float) (Math.Sin(p.X * _fastScale) + Math.Sin(p.Y * _fastScale) + Math.Sin(p.Z * _fastScale));
		}

		public override string ToString() {
			return $"Sine {_scale}";
		}

		public override bool Precise() {
			return false;
		}
	}
}
