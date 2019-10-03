using System;
using PolygonTriangulation.Model;

namespace PolygonTriangulation.ImplicitObjects {
	public class Displacement : SDF {
		private SDF _original;
		private Func<Vec3, float> _displacement;

		/// <summary>
		/// Applies a displacement function to an object.
		/// </summary>
		/// <remarks>
		/// The displacement function should be determinitic to ensure a correct object.
		/// </remarks>
		/// <param name="original">The original object.</param>
		/// <param name="displacement">The displacement function.</param>
		public Displacement(SDF original, Func<Vec3, float> displacement) {
			_original = original;
			_displacement = displacement;
		}

		public override float Distance(Vec3 p) {
			return _original.Distance(p) + _displacement(p);
		}

		public override bool Precise() {
			return false;
		}
	}
}
