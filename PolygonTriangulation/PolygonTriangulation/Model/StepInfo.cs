using System;
using System.Collections.Generic;

namespace PolygonTriangulation.Model {
	/// <summary>
	/// Contains information regarding algorithm visualization steps.
	/// </summary>
	public class StepInfo {
		private float _HalfDist;
		public Vec3 Center { get; set; }
		public float HalfDist { get { return _HalfDist; } set { _HalfDist = Math.Abs(value); } }
		public List<int[]> triangles { get; set; } = new List<int[]>();
	}
}
