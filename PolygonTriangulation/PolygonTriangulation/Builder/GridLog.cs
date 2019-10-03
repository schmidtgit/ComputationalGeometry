using PolygonTriangulation.Model;
using System.Collections.Generic;
using PolygonTriangulation.Polygon;

namespace PolygonTriangulation.Builder {
	public class GridLog : IPolygonBuilder{
		private IDictionary<Vec3, int> _vertices;
		private List<StepInfo> _steps;
		private StepInfo _currentStep;

		/// <summary>
		/// Initialize a new empty GridLog.
		/// </summary>
		public GridLog() {
			_vertices = new Dictionary<Vec3, int>();
			_steps = new List<StepInfo>();
			_currentStep = new StepInfo();
		}

		/// <summary>
		/// Creates a log for the position of the debug-cube.
		/// </summary>
		/// <param name="center">Center of the cube.</param>
		/// <param name="dist">Half length of the cube.</param>
		public void NewCube(Vec3 center, float dist) {
			if(_currentStep.Center != null) {
				_steps.Add(_currentStep);
				_currentStep = new StepInfo();
			}
			_currentStep.Center = center;
			_currentStep.HalfDist = dist;
			_currentStep.triangles = new List<int[]>(2);
		}

		/// <summary>
		/// Adds a triangle, represented by the three vertices, to the current debug-cube.
		/// Expected counter-clockwise direction.
		/// </summary>
		/// <param name="a">First point in triangle.</param>
		/// <param name="b">Second point in triangle.</param>
		/// <param name="c">Third point in triangle.</param>
		public void Append(Vec3 a, Vec3 b, Vec3 c) {
			int ai = VectorIndex(a);
			int bi = VectorIndex(b);
			int ci = VectorIndex(c);
			_currentStep.triangles.Add(new[] { ai, bi, ci });
		}

		/// <summary>
		/// Get the internal index of the given vertex.
		/// </summary>
		private int VectorIndex(Vec3 vert) {
			if(!_vertices.ContainsKey(vert)) {
				_vertices.Add(vert, _vertices.Count);
			}
			return _vertices[vert];
		}

		/// <summary>
		/// Created for easier appending when doing gridbased algorithms.
		/// Applies offset to each vertex to each vector before adding them to the current debug-cube.
		/// </summary>
		/// <param name="a">First point in triangle.</param>
		/// <param name="b">Second point in triangle.</param>
		/// <param name="c">Third point in triangle.</param>
		/// <param name="offset">Constant to add to each vector.</param>
		public void Append(Vec3 a, Vec3 b, Vec3 c, Vec3 offset) {
			Append(a + offset, b + offset, c + offset);
		}


		/// <summary>
		/// Builds an LogPolygon with no duplicated vertices.
		/// </summary>
		public IPolygon Build() {
			if(_currentStep.Center != null || _currentStep.triangles.Count != 0) {
				_steps.Add(_currentStep);
				_currentStep = new StepInfo();
			}
			return new LogPolygon(new Log { steps = _steps.ToArray(), vert = GetVertices() });
		}

		/// <summary>
		/// Returns an array of all vertices.
		/// </summary>
		private Vec3[] GetVertices() {
			var array = new Vec3[_vertices.Count];
			foreach(KeyValuePair<Vec3, int> pair in _vertices) {
				array[pair.Value] = pair.Key;
			}
			return array;
		}
		
		/// <summary>
		/// Prepares the builder for reuse.
		/// </summary>
		public void Clear() {
			_vertices.Clear();
			_steps.Clear();
			_currentStep = null;
		}
	}
}
