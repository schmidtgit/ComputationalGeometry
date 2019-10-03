using PolygonTriangulation.Model;

namespace PolygonTriangulation.ImplicitObjects {
	public class InfinitePlane: SDF {
		float _v;

		/// <summary>
		/// Creates an infinite plane.
		/// </summary>
		/// <remarks>
		/// This everything below the plane is interpreted as being inside the object.
		/// </remarks>
		/// <param name="v"></param>
		public InfinitePlane(float v) { _v = v; }

		public override float Distance(Vec3 p) {
			return p.Y + _v;
		}

		public override bool Precise() {
			return true;
		}

		public override string ToString() {
			return $"Plane {_v}";
		}
	}
}
