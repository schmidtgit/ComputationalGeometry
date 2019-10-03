using System;
using System.Collections.Generic;
using System.Linq;
using PolygonTriangulation.Model;

namespace PolygonTriangulation.Polygon {
	public class ListPolygon : IPolygon{
		private List<Triangle> mesh;

		public ListPolygon(List<Triangle> triangles) { mesh = triangles; }

		public int TriangleCount {
			get {
				return mesh.Count;
			}
		}

		public IEnumerable<int> Triangles {
			get {
				return Enumerable.Range(0, VertexCount);
			}
		}

		public int VertexCount {
			get {
				return mesh.Count * 3;
			}
		}

		public IEnumerable<Vec3> Vertices {
			get {
				foreach (Triangle t in mesh) {
					yield return t.First;
					yield return t.Second;
					yield return t.Third;
				}
			}
		}

		public List<Triangle> GetTriangles() {
			return mesh;
		}
	}
}
