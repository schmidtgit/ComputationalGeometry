using PolygonTriangulation.Model;
using System;

namespace PolygonTriangulation.Triangulation {
    /// <summary>
    /// Triangulates an object using the Marching Tetrahedra algorithm.
    /// </summary>
    /// <remarks> 
    /// Based on code made by Paul Bourke.
    /// http://paulbourke.net/geometry/polygonise/ 
    /// Calculates the vertex position based on distances to the surface.
    /// </remarks>
	public class WeightedTetrahedra : MarchingTetrahedra {
		public WeightedTetrahedra() : this(128) { }
		public WeightedTetrahedra(int resolution = 128) : base(resolution, resolution, resolution, 1) { }
		public WeightedTetrahedra(int x, int y, int z, int step) : base(x, y, z, step) {}

		
		protected override Vec3 CalculateVertex(Vec3 a, Vec3 b, float af, float bf) {
			//From https://github.com/Calvin-L/MarchingTetrahedrons/blob/master/Decimate.cpp
			float interp = -af / (bf - af);
			float oneMinusInterp = 1 - interp;

			float x = a.X * oneMinusInterp + b.X * interp;
			float y = a.Y * oneMinusInterp + b.Y * interp;
			float z = a.Z * oneMinusInterp + b.Z * interp;
			return new Vec3(x, y, z);
		}

		public override string ToString() {
			return "Marching Tetrahedra - Weighted";
		}
	}
}
