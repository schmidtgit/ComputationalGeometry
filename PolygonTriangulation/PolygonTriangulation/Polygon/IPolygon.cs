using PolygonTriangulation.Model;
using System.Collections.Generic;

namespace PolygonTriangulation.Polygon {
	/// <summary>
	/// Interface that allows for multiple ways to hold tris and vert data.
	/// </summary>
	public interface IPolygon {
		List<Triangle> GetTriangles();
		int TriangleCount { get; }
		int VertexCount { get; }
		IEnumerable<int> Triangles { get; }
		IEnumerable<Vec3> Vertices { get; }
	}
}
