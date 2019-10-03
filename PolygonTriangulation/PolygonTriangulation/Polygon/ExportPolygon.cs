using System.Collections.Generic;
using PolygonTriangulation.Model;
using PolygonTriangulation.Builder;

namespace PolygonTriangulation.Polygon {
	/// <summary>
	/// A simple IPolygon implementation.
	/// </summary>
	public class ExportPolygon : IPolygon {
		Vec3[] _vertices;
		int[] _triangles;

		/// <summary>
		/// Stores the given arrays directly.
		/// No checks are performed.
		/// </summary>
		/// <param name="vert">Array of vertices.</param>
		/// <param name="tris">Array of vertice indexes, pair of three forms a triangle.</param>
		public ExportPolygon(Vec3[] vert, int[] tris) {
			_vertices = vert;
			_triangles = tris;
		}

		/// <summary>
		/// Removed all duplicate vertices by converting to ExportPolygon.
		/// </summary>
		/// <param name="p">IPolygon to convert.</param>
		/// <returns>A IPolgyon(ExportPolygon) with no duplicate vertices.</returns>
		public static ExportPolygon Convert(IPolygon p) {
			if (p is ExportPolygon)
				return p as ExportPolygon;
			
			var b = new ExportBuilder();
			foreach (Triangle t in p.GetTriangles()) {
				b.Append(t.First, t.Second, t.Third);
			}

			return b.Build() as ExportPolygon;
		}

		/// <summary>
		/// Return number of triangles.
		/// </summary>
		public int TriangleCount {
			get {
				return _triangles.Length / 3;
			}
		}

		/// <summary>
		/// Returns all vertice indexes that form the triangles.
		/// </summary>
		public IEnumerable<int> Triangles {
			get {
				return _triangles;
			}
		}

		/// <summary>
		/// Returns number of vertices.
		/// </summary>
		public int VertexCount {
			get {
				return _vertices.Length;
			}
		}

		/// <summary>
		/// Returns all vertices.
		/// </summary>
		public IEnumerable<Vec3> Vertices {
			get {
				return _vertices;
			}
		}

		/// <summary>
		/// Converts internal arrays to a list of Triangles.
		/// </summary>
		/// <returns>List of Triangles that represent the polygon.</returns>
		public List<Triangle> GetTriangles() {
			var r = new List<Triangle>(TriangleCount);

			int i = 0;
			while (i < _triangles.Length) {
				r.Add(new Triangle(_vertices[_triangles[i++]], _vertices[_triangles[i++]], _vertices[_triangles[i++]]));
			}

			return r;
		}
	}
}
