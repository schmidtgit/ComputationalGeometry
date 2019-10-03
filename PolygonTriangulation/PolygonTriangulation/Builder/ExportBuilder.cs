using System.Collections.Generic;
using PolygonTriangulation.Model;
using PolygonTriangulation.Polygon;

namespace PolygonTriangulation.Builder {
	/// <summary>
	/// ExportBuilder ensures correct vertex re-use.
	/// </summary>
	public class ExportBuilder : IPolygonBuilder {
		Dictionary<Vec3, int> _vDictionary;
        List<int> _tris;
		int _vertIndex;

		public ExportBuilder() {
			_vDictionary = new Dictionary<Vec3, int>();
            _tris = new List<int>();
		}

		/// <summary>
		/// Adds a triangle, represented by the three vertices, to the polygon.
		/// Expected counter-clockwise direction. 
		/// </summary>
		/// <param name="a">First point in triangle.</param>
		/// <param name="b">Second point in triangle.</param>
		/// <param name="c">Third point in triangle.</param>
		public void Append(Vec3 a, Vec3 b, Vec3 c) {
			// Ignore invalid tris
			if (a.Equals(b) || a.Equals(c) || b.Equals(c))
				return;

			AddVector(a);
			AddVector(b);
			AddVector(c);
		}

		/// <summary>
		/// Add Vector to the dictionary.
		/// </summary>
		private void AddVector(Vec3 vert) {
			int e;
			if (_vDictionary.TryGetValue(vert, out e)) {
				_tris.Add(e);
			} else {
				_vDictionary.Add(vert, _vertIndex);
                _tris.Add(_vertIndex);
                _vertIndex++;
			}
		}

		/// <summary>
		/// Created for easier appending when doing gridbased algorithms.
		/// Applies offset to each vertex to each vector before adding them.
		/// </summary>
		/// <param name="a">First point in triangle.</param>
		/// <param name="b">Second point in triangle.</param>
		/// <param name="c">Third point in triangle.</param>
		/// <param name="offset">Constant to add to each vector.</param>
		public void Append(Vec3 a, Vec3 b, Vec3 c, Vec3 offset) {
			Append(a + offset, b + offset, c + offset);
		}

		/// <summary>
		/// Builds an ExportPolygon with no duplicated vertices.
		/// </summary>
		public IPolygon Build() {
			var vArr = new Vec3[_vDictionary.Count];
			var tris = _tris.ToArray();
			foreach (var pair in _vDictionary) {
				vArr[pair.Value] = pair.Key;
			}
			return new ExportPolygon(vArr, tris);
		}

		/// <summary>
		/// Prepares the builder for reuse.
		/// </summary>
		public void Clear() {
			_vDictionary.Clear();
            _tris.Clear();
            _vertIndex = 0;
		}
	}
}
