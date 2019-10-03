using PolygonTriangulation.Model;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PolygonTriangulation.Polygon {
	/// <remark>
	/// Extensions are used since interfaces can not implement operators.
	/// </remark>
	public static class PolygonExtension {
		/// <summary>
		/// Add all vertices and triangles from both Polygons together.
		/// </summary>
		/// <param name="a">The first polygon</param>
		/// <param name="b">The second polygon</param>
		/// <returns>Polygon A + Polygon B</returns>
		public static IPolygon Add(this IPolygon a, IPolygon b) {
			return new ExportPolygon(a.Vertices.Concat(b.Vertices).ToArray(), a.Triangles.Concat(b.Triangles).ToArray());
		}

		/// <summary>
		/// Appends a vector to each vertices in the IPolygon,
		/// effectively moving it.
		/// </summary>
		/// <param name="a">Polygon to move</param>
		/// <param name="v">Distanve to move</param>
		/// <returns>Polygon A moved by V</returns>
		public static IPolygon Move(this IPolygon a, Vec3 v) {
			var vert = a.Vertices.ToArray();
			Parallel.For(0, vert.Length, i => vert[i] = vert[i] + v);
			return new ExportPolygon(vert, a.Triangles.ToArray());
		}

		/// <summary>
		/// Calculates a normal vector for each face in the polygon
		/// </summary>
		/// <param name="mesh">The Polygon to calculate normals on</param>
		/// <returns>Normals expressed as vectors</returns>
		public static IEnumerable<Vec3> GetNormals(this IPolygon mesh) {
			foreach (Triangle t in mesh.GetTriangles()) {
				yield return Vec3.NormalizedCrossProduct(t.Second - t.First, t.Third - t.Second);
			}
		}
	}
}
