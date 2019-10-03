using System.Collections.Generic;
using PolygonTriangulation.Model;
using PolygonTriangulation.ImplicitObjects;

namespace PolygonTriangulation.Triangulation {
    /// <summary>
    /// Triangulates an object using the Naive Surface Nets algorithm.
    /// </summary>
    /// <remarks> 
    /// Based on surfacenets.js by Mikola Lysenko.
    /// https://github.com/mikolalysenko/isosurface/blob/master/lib/surfacenets.js 
    /// </remarks>
    public class NaiveSurfaceNets : GridBased
	{
		int[] edge_table;
        Dictionary<int, int> buffer;
        Vec3[] cornersDiff;
		List<Vec3> vertices;
        int[] offsets;
        int minx, miny, minz;

		protected override void Optimize(SDF obj) {
			useRefinement = obj.Precise();
			useLazyCube = obj.Precise();
			if (obj.RequiredGridSize() != 0) {
				int result = 1;
				if (useRefinement)
					while (result < (obj.RequiredGridSize() + 1)) // Naive Surface Nets require a larger grid in order to be rendered sides correct.
						result *= 2;
				else
                    result = obj.RequiredGridSize() % 2 == 0 ? obj.RequiredGridSize() + 2 : obj.RequiredGridSize() + 1;
                _gridX = _gridY = _gridZ = result;
			}
		}

		protected override void PostStep(int x, int y, int z, Vec3 origin) {
			// Pointer to buffer
            var pointer = z + y * (_gridZ) + (_gridZ) * (_gridY) * x;

            // Calculate cube corners!
            Vec3[] corners = new Vec3[8];
			float[] values = new float[8];
			for (int i = 0; i < 8; i++) {
				var p = origin + cornersDiff[i];
				corners[i] = p;
				values[i] = _obj.Distance(p);
			}

			// Calculate 8bit mask for fast sign checks!
			byte mask = 0;
			if (values[0] <= 0) mask |= 1;
			if (values[1] <= 0) mask |= 2;
			if (values[2] <= 0) mask |= 4;
			if (values[3] <= 0) mask |= 8;
			if (values[4] <= 0) mask |= 16;
			if (values[5] <= 0) mask |= 32;
			if (values[6] <= 0) mask |= 64;
			if (values[7] <= 0) mask |= 128;

			// Check for early termination
			if (mask == 0 || mask == 255)
				return;

			// Calculate sum of edges!
			var edgeMask = edge_table[mask];

            //Calculate vertex
            var vertex = CalculateVertex(origin, values, edgeMask);

            // Store all vertices generated
            buffer[pointer] = vertices.Count;
            vertices.Add(vertex);
            
			// Add faces
			for (int i = 0; i < 3; ++i) {
				//The first three entries of the edge_mask count the crossings along the edge
				if ((edgeMask & (1 << i)) == 0)
					continue;

				var iu = (i + 1) % 3;
				var iv = (i + 2) % 3;

                if ((z == minz && (iu == 0 || iv == 0)) ||
                        (y == miny && (iu == 1 || iv == 1)) ||
                        (x == minx && (iu == 2 || iv == 2))) 
                    continue;

				var du = offsets[iu]; 
				var dv = offsets[iv]; 

                //Remember to flip orientation depending on the sign of the corner.
                if ((mask & 1) == 0) {
                    _builder.Append(vertices[buffer[pointer]], vertices[buffer[pointer - du]], vertices[buffer[pointer - du - dv]]);
					_builder.Append(vertices[buffer[pointer - du - dv]], vertices[buffer[pointer-dv]], vertices[buffer[pointer]]);
				} else {
                    _builder.Append(vertices[buffer[pointer]], vertices[buffer[pointer - dv]], vertices[buffer[pointer - du - dv]]);
                    _builder.Append(vertices[buffer[pointer - du - dv]], vertices[buffer[pointer - du]], vertices[buffer[pointer]]);
                }
			}
        }

        /// <summary>
        /// Calculates the position of the vertex.
        /// </summary>
        /// <returns></returns>
        protected virtual Vec3 CalculateVertex(Vec3 origin, float[] values, int edgeMask) {
            return origin;
        }

        protected override void Initialize()
		{
            buffer = new Dictionary<int, int>();
			vertices = new List<Vec3>();

            //Offsets used for the pointers to the vertices
            offsets = new int[] { 1, (_gridZ ), (_gridZ) * (_gridY) };

            minx = -_gridX / 2;
            miny = -_gridY / 2;
            minz = -_gridZ / 2;

            cornersDiff = GenerateCorners();

            //Mikolas edge_table hardcoded
            edge_table = GenerateEdgeTable();
		}

        /// <summary>
        /// Generates the edge table to determine, which edges are cut by the surfaces.
        /// </summary>
        protected virtual int[] GenerateEdgeTable() {
            return new[]{
                0, 7, 25, 30, 98, 101, 123, 124,
                168, 175, 177, 182, 202, 205, 211, 212,
                772, 771, 797, 794, 870, 865, 895, 888,
                940, 939, 949, 946, 974, 969, 983, 976,
                1296, 1303, 1289, 1294, 1394, 1397, 1387, 1388,
                1464, 1471, 1441, 1446, 1498, 1501, 1475, 1476,
                1556, 1555, 1549, 1546, 1654, 1649, 1647, 1640,
                1724, 1723, 1701, 1698, 1758, 1753, 1735, 1728,
                2624, 2631, 2649, 2654, 2594, 2597, 2619, 2620,
                2792, 2799, 2801, 2806, 2698, 2701, 2707, 2708,
                2372, 2371, 2397, 2394, 2342, 2337, 2367, 2360,
                2540, 2539, 2549, 2546, 2446, 2441, 2455, 2448,
                3920, 3927, 3913, 3918, 3890, 3893, 3883, 3884,
                4088, 4095, 4065, 4070, 3994, 3997, 3971, 3972,
                3156, 3155, 3149, 3146, 3126, 3121, 3119, 3112,
                3324, 3323, 3301, 3298, 3230, 3225, 3207, 3200,
                3200, 3207, 3225, 3230, 3298, 3301, 3323, 3324,
                3112, 3119, 3121, 3126, 3146, 3149, 3155, 3156,
                3972, 3971, 3997, 3994, 4070, 4065, 4095, 4088,
                3884, 3883, 3893, 3890, 3918, 3913, 3927, 3920,
                2448, 2455, 2441, 2446, 2546, 2549, 2539, 2540,
                2360, 2367, 2337, 2342, 2394, 2397, 2371, 2372,
                2708, 2707, 2701, 2698, 2806, 2801, 2799, 2792,
                2620, 2619, 2597, 2594, 2654, 2649, 2631, 2624,
                1728, 1735, 1753, 1758, 1698, 1701, 1723, 1724,
                1640, 1647, 1649, 1654, 1546, 1549, 1555, 1556,
                1476, 1475, 1501, 1498, 1446, 1441, 1471, 1464,
                1388, 1387, 1397, 1394, 1294, 1289, 1303, 1296,
                976, 983, 969, 974, 946, 949, 939, 940,
                888, 895, 865, 870, 794, 797, 771, 772,
                212, 211, 205, 202, 182, 177, 175, 168,
                124, 123, 101, 98, 30, 25, 7, 0};
        }

        /// <summary>
        /// Generates the offset of each corner from the center of a cube.
        /// </summary>
        protected virtual Vec3[] GenerateCorners()
		{
			//Positions of the corners relative to the center
			var pos = _step * 0.5f;
			var neg = -pos;
			Vec3[] cp = new Vec3[] {
				new Vec3(neg,neg,neg),
				new Vec3(neg,neg,pos),
				new Vec3(neg,pos,neg),
				new Vec3(neg,pos,pos),
				new Vec3(pos,neg,neg),
                new Vec3(pos,neg,pos),
                new Vec3(pos,pos,neg),
                new Vec3(pos,pos,pos),
            };
			return cp;
		}

		public override string ToString() {
			return "Naive Surface Nets";
		}
	}
}
