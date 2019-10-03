using System.Collections.Generic;
using System.Linq;
using PolygonTriangulation.Model;

namespace PolygonTriangulation.Polygon {
	public class LogPolygon : IPolygon {
		private Log _log;

		public LogPolygon(Log log) {
			_log = log;
		}

		public Log GetLog() {
			return _log;
		}

		public int TriangleCount {
			get {
				return Triangles.Count();
			}
		}

		public IEnumerable<int> Triangles {
			get {
				foreach(StepInfo st in _log.steps) {
					foreach(int[] ia in st.triangles) {
						foreach(int i in ia) {
							yield return i;
						}
					}
				}
			}
		}

		public int VertexCount {
			get {
				return _log.vert.Length;
			}
		}

		public IEnumerable<Vec3> Vertices {
			get {
				return _log.vert;
			}
		}

		public List<Triangle> GetTriangles() {
			var tris = new List<Triangle>();
			foreach(StepInfo st in _log.steps) {
				foreach(int[] ia in st.triangles) {
					tris.Add(new Triangle(_log.vert[ia[0]], _log.vert[ia[0]], _log.vert[ia[0]]));
				}
			}
			return tris;
		}
	}
}
