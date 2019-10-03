using PolygonTriangulation.Model;

namespace PolygonTriangulation.Triangulation {
    /// <summary>
    /// Triangulates an object using the Marching Cubes algorithm.
    /// </summary>
    /// <remarks> 
    /// Based on code made by Paul Bourke.
    /// http://paulbourke.net/geometry/polygonise/ 
    /// Calculates the vertex position based on distances to the surface.
    /// </remarks>
	public class WeightedCubes : MarchingCubes {
		public WeightedCubes() : this(128) { }
		public WeightedCubes(int resolution = 128) : this(resolution, resolution, resolution, 1) { }
		public WeightedCubes(int x, int y, int z, float step) : base(x, y, z, step){ }

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
			return "Marching Cubes - Weighted";
		}
	}
}
