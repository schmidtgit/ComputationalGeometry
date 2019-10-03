using PolygonTriangulation.Model;
using System;

namespace PolygonTriangulation.Triangulation {
    /// <summary>
    /// Triangulates an object using the Marching Tetrahedra algorithm.
    /// </summary>
    /// <remarks> 
    /// Based on code made by Paul Bourke.
    /// http://paulbourke.net/geometry/polygonise/ 
    /// </remarks>
    public class MarchingTetrahedra : GridBased {
		protected Vec3[] _corners;
		protected int[][] _tetras;

		public MarchingTetrahedra() : this(128) { }
		public MarchingTetrahedra(int resolution = 128) : this(resolution, resolution, resolution, 1) {}
		public MarchingTetrahedra(int x, int y, int z, int step) {
			_gridX = x;
			_gridY = y;
			_gridZ = z;
			_step = step;
		}

		protected override void Initialize() {
			_corners = GenerateCorners();
            _tetras = GenerateTetraIndices();
        }

        protected override void PostStep(int x, int y, int z, Vec3 origin) {
            var values = new float[8];
            for(int i = 0; i < values.Length; i++) {
                values[i] = _obj.Distance(_corners[i] + origin);
            }
            Vec3 a, b, c, d;

            foreach(int[] tetra in _tetras) {
                int c0 = tetra[0];
                int c1 = tetra[1];
                int c2 = tetra[2];
                int c3 = tetra[3];

                // Check distance to the vertices in the cube!
                byte _type = 0;
                if(values[c0] <= 0) { _type |= 1; }
                if(values[c1] <= 0) { _type |= 2; }
                if(values[c2] <= 0) { _type |= 4; }
                if(values[c3] <= 0) { _type |= 8; }

                // Switch over type
                switch(_type) {
                    case 0:
                    case 15:
                        // case 0 = none contained
                        // case 15 = 0, 1, 2, 3 contained
                        break;
                    case 1:
                        // case 1 = 0 contained
                        a = CalculateVertex(_corners[c0], _corners[c1], values[c0], values[c1]);
                        b = CalculateVertex(_corners[c0], _corners[c2], values[c0], values[c2]);
                        c = CalculateVertex(_corners[c0], _corners[c3], values[c0], values[c3]);
                        _builder.Append(a, b, c, origin);
                        break;
                    case 14:
                        // case 14 = 1, 2, 3 contained
                        a = CalculateVertex(_corners[c0], _corners[c1], values[c0], values[c1]);
                        b = CalculateVertex(_corners[c0], _corners[c3], values[c0], values[c3]);
                        c = CalculateVertex(_corners[c0], _corners[c2], values[c0], values[c2]);
                        _builder.Append(a, b, c, origin);
                        break;
                    case 2:
                        // case 2 = 1 contained
                        a = CalculateVertex(_corners[c1], _corners[c0], values[c1], values[c0]);
                        b = CalculateVertex(_corners[c1], _corners[c3], values[c1], values[c3]);
                        c = CalculateVertex(_corners[c1], _corners[c2], values[c1], values[c2]);
                        _builder.Append(a, b, c, origin);
                        break;
                    case 13:
                        // case 13 = 0, 2, 3 contained
                        a = CalculateVertex(_corners[c1], _corners[c0], values[c1], values[c0]);
                        b = CalculateVertex(_corners[c1], _corners[c2], values[c1], values[c2]);
                        c = CalculateVertex(_corners[c1], _corners[c3], values[c1], values[c3]);
                        _builder.Append(a, b, c, origin);
                        break;
                    case 4:
                        // case 4 = 2 contained
                        a = CalculateVertex(_corners[c2], _corners[c0], values[c2], values[c0]);
                        b = CalculateVertex(_corners[c2], _corners[c1], values[c2], values[c1]);
                        c = CalculateVertex(_corners[c2], _corners[c3], values[c2], values[c3]);
                        _builder.Append(a, b, c, origin);
                        break;
                    case 11:
                        // case 11 = 0, 1, 3 contained 
                        a = CalculateVertex(_corners[c2], _corners[c0], values[c2], values[c0]);
                        b = CalculateVertex(_corners[c2], _corners[c3], values[c2], values[c3]);
                        c = CalculateVertex(_corners[c2], _corners[c1], values[c2], values[c1]);
                        _builder.Append(a, b, c, origin);
                        break;
                    case 8:
                        // case 8 = 3 contained
                        b = CalculateVertex(_corners[c3], _corners[c0], values[c3], values[c0]);
                        c = CalculateVertex(_corners[c3], _corners[c2], values[c3], values[c2]);
                        a = CalculateVertex(_corners[c3], _corners[c1], values[c3], values[c1]);
                        _builder.Append(a, b, c, origin);
                        break;
                    case 7:
                        // case 7 = 0, 1, 2 contained
                        b = CalculateVertex(_corners[c3], _corners[c0], values[c3], values[c0]);
                        c = CalculateVertex(_corners[c3], _corners[c1], values[c3], values[c1]);
                        a = CalculateVertex(_corners[c3], _corners[c2], values[c3], values[c2]);
                        _builder.Append(a, b, c, origin);
                        break;
                    case 12:
                        // case 12 = 2 and 3 contained
                        a = CalculateVertex(_corners[c0], _corners[c2], values[c0], values[c2]);
                        b = CalculateVertex(_corners[c0], _corners[c3], values[c0], values[c3]);

                        // 1
                        c = CalculateVertex(_corners[c1], _corners[c2], values[c1], values[c2]);
                        d = CalculateVertex(_corners[c1], _corners[c3], values[c1], values[c3]);

                        // Append both
                        _builder.Append(a, d, b, origin);
                        _builder.Append(a, c, d, origin);
                        break;
                    case 3:
                        // case 3 = 0 and 1 contained
                        a = CalculateVertex(_corners[c0], _corners[c2], values[c0], values[c2]);
                        b = CalculateVertex(_corners[c0], _corners[c3], values[c0], values[c3]);

                        // 1
                        c = CalculateVertex(_corners[c1], _corners[c2], values[c1], values[c2]);
                        d = CalculateVertex(_corners[c1], _corners[c3], values[c1], values[c3]);

                        // Append both
                        _builder.Append(a, b, d, origin);
                        _builder.Append(a, d, c, origin);
                        break;
                    case 10:
                        // case 10 = 1 and 3 contained
                        a = CalculateVertex(_corners[c0], _corners[c1], values[c0], values[c1]);
                        b = CalculateVertex(_corners[c0], _corners[c3], values[c0], values[c3]);

                        // 2
                        c = CalculateVertex(_corners[c2], _corners[c1], values[c2], values[c1]);
                        d = CalculateVertex(_corners[c2], _corners[c3], values[c2], values[c3]);

                        // Append both
                        _builder.Append(a, b, c, origin);
                        _builder.Append(b, d, c, origin);
                        break;
                    case 5:
                        // case 5 = 0 and 2 contained
                        a = CalculateVertex(_corners[c0], _corners[c1], values[c0], values[c1]);
                        b = CalculateVertex(_corners[c0], _corners[c3], values[c0], values[c3]);

                        // 2
                        c = CalculateVertex(_corners[c2], _corners[c1], values[c2], values[c1]);
                        d = CalculateVertex(_corners[c2], _corners[c3], values[c2], values[c3]);

                        // Append both
                        _builder.Append(a, c, b, origin);
                        _builder.Append(b, c, d, origin);
                        break;
                    case 9:
                        // case 9 = 0 and 3 contained
                        a = CalculateVertex(_corners[c1], _corners[c0], values[c1], values[c0]);
                        b = CalculateVertex(_corners[c1], _corners[c3], values[c1], values[c3]);

                        // 2
                        c = CalculateVertex(_corners[c2], _corners[c0], values[c2], values[c0]);
                        d = CalculateVertex(_corners[c2], _corners[c3], values[c2], values[c3]);

                        // Append both
                        _builder.Append(a, d, b, origin);
                        _builder.Append(a, c, d, origin);
                        break;
                    case 6:
                        // case 6 = 1 and 2 contained
                        a = CalculateVertex(_corners[c1], _corners[c0], values[c1], values[c0]);
                        b = CalculateVertex(_corners[c1], _corners[c3], values[c1], values[c3]);

                        // 2
                        c = CalculateVertex(_corners[c2], _corners[c0], values[c2], values[c0]);
                        d = CalculateVertex(_corners[c2], _corners[c3], values[c2], values[c3]);

                        // Append both
                        _builder.Append(a, b, d, origin);
                        _builder.Append(a, d, c, origin);
                        break;
                    default:
                        throw new NotImplementedException();
                }
            }
        }

        protected virtual Vec3 CalculateVertex(Vec3 a, Vec3 b, float af, float bf) {
            return (a + b) * 0.5f;
        }

        /// <summary>
        /// Generates the offset of each corner from the center of a cube.
        /// </summary>
		protected virtual Vec3[] GenerateCorners() {
			// Array of all positions in the cubes
			var pos = _step * 0.5f;
			var neg = -pos;
			Vec3[] cp = new Vec3[] {
				new Vec3(neg,neg,neg),
				new Vec3(pos,neg,neg),
				new Vec3(pos,pos,neg),
				new Vec3(neg,pos,neg),
				new Vec3(neg,neg,pos),
				new Vec3(pos,neg,pos),
				new Vec3(pos,pos,pos),
				new Vec3(neg,pos,pos)
			};
			return cp;
		}

        [Obsolete]
		protected virtual int[][] GenerateOldTetraIndices() {
			// All tetrahedras in the cube
			return new int[][] {
				new [] { 0, 5, 1, 6 },
				new [] { 0, 1, 2, 6 },
				new [] { 0, 2, 3, 6 },
				new [] { 0, 3, 7, 6 },
				new [] { 0, 7, 4, 6 },
				new [] { 0, 4, 5, 6 }
			}; 
		} 

        /// <summary>
        /// Generates tetrahedras used to divide a cube.
        /// </summary>
        protected virtual int[][] GenerateTetraIndices() {
            // All tetrahedras in the cube
            return new int[][] {
                new [] { 0, 2, 3, 7 },
                new [] { 0, 6, 2, 7 },
                new [] { 0, 4, 6, 7 },
                new [] { 0, 6, 1, 2 },
                new [] { 0, 1, 6, 4 },
                new [] { 5, 6, 1, 4 }
            };
        }

        public override string ToString() {
			return "Marching Tetrahedra";
		}
	}
}
