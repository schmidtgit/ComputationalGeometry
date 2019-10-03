using System;
using PolygonTriangulation.Model;

namespace PolygonTriangulation.ImplicitObjects {
	public class CustomSDF : SDF {
		private Func<Vec3, float> _sdf;

		/// <summary>
		/// Creates a custom object from a delegate.
		/// </summary>
		/// <remarks>
		/// The delegate should be deterministic to ensure a correct mesh.
		/// </remarks>
		/// <param name="sdf">A delegate desribing a signed distance function.</param>
		public CustomSDF(Func<Vec3, float> sdf) {
			_sdf = sdf;
		}

		public override float Distance(Vec3 p) {
			return _sdf(p);
		}

		public override bool Precise() {
			return false;
		}
	}
}
