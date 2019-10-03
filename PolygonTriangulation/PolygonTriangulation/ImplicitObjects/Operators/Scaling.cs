using System;
using PolygonTriangulation.Model;

namespace PolygonTriangulation.ImplicitObjects {
	public class Scaling : SDF {
		private SDF _original;
		private float scale;

		/// <summary>
		/// Scales a SDF with a given value.
		/// </summary>
		/// <remarks>
		/// A value larger than 1 enlarges the object.
		/// A value between 0 and 1 shrinks the object.
		///	Negative values have unspecified behavior.
		/// </remarks>
		/// <param name="original">The original SDF.</param>
		/// <param name="scaleValue">The scale value.</param>
		public Scaling(SDF original, float scaleValue) {
			_original = original;
			scale = scaleValue;
		}

		public override float Distance(Vec3 p) {
			return _original.Distance(p / scale) * scale;
		}

		public override string ToString() {
			var first = _original == null ? "_____" : _original.ToString();
			return $"{first} x {scale}";
		}

		public override int RequiredGridSize() {
			return (int) Math.Ceiling(_original.RequiredGridSize() * scale);
		}

		public override bool Precise() {
			return _original.Precise();
		}
	}
}
