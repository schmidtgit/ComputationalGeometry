using System;
using PolygonTriangulation.Model;

namespace PolygonTriangulation.Triangulation {
    /// <summary>
    /// Triangulates an object using the Naive Surface Nets algorithm.
    /// </summary>
    /// <remarks> 
    /// Based on surfacenets.js by Mikola Lysenko.
    /// https://github.com/mikolalysenko/isosurface/blob/master/lib/surfacenets.js 
    /// Calculates the vertex position based on distances the surface
    /// </remarks>
    public class WeightedNaiveSurfaceNets : NaiveSurfaceNets {
        int[] cube_edges;

        protected override Vec3 CalculateVertex(Vec3 origin, float[] values, int edgeMask) {
            float[] v = new float[3];
            int e_count = 0;

            // Foreach edge in the cube
            for(int i = 0; i < 12; i++) {

                // Use edge mask to check if it is crossed
                if((edgeMask & (1 << i)) == 0) 
                    continue;

                e_count++;

                //Now find the point of intersection
                var e0 = cube_edges[i << 1];        //Unpack vertices
                var e1 = cube_edges[(i << 1) + 1];
                var g0 = values[e0];                //Unpack grid values
                var g1 = values[e1];
                var t = g0 - g1;                    //Compute point of intersection
                if(Math.Abs(t) > 1e-6) {
                    t = g0 / t;
                } else {
                    continue;
                }

                // Interpolate vertices!
                for(int j = 0, k = 1; j < 3; ++j, k <<= 1) {
                    var a = e0 & k;
                    var b = e1 & k;
                    if(a != b) {
                        v[j] += a != 0 ? 1.0f - t : t;
                    } else {
                        v[j] += a != 0 ? 1.0f : 0;
                    }
                }
            }

            // Find the average
            return new Vec3(v[2] / e_count, v[1] / e_count, v[0] / e_count) + origin;
        }

        protected override void Initialize() {
            // Initialize the cube_edges table, by Mikola Lysenko
            cube_edges = new int[24];
            var k = 0;
            for(var i = 0; i < 8; ++i) {
                for(var j = 1; j <= 4; j <<= 1) {
                    var p = i ^ j;
                    if(i <= p) {
                        cube_edges[k++] = i;
                        cube_edges[k++] = p;
                    }
                }
            }
            base.Initialize();
        }

		public override string ToString() {
			return "Naive Surface Nets - Weighted";
		}
	}
}
