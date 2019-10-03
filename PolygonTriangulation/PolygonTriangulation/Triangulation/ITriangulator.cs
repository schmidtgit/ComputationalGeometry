using PolygonTriangulation.Builder;
using PolygonTriangulation.ImplicitObjects;
using PolygonTriangulation.Polygon;

namespace PolygonTriangulation.Triangulation {
	/// <summary>
	/// Triangulates a SDF.
	/// </summary>
	public interface ITriangulator {
		/// <summary>
		/// Triangulates the given SDF by using the given builder.
		/// </summary>
		IPolygon Run(SDF obj, IPolygonBuilder builder);
	}
}
