using System;
using PolygonTriangulation.Model;

namespace PolygonTriangulation.ImplicitObjects.Complex {
	public class AtomCube : SDF {
		SDF _complex;
		float _scale;
		public AtomCube() {
			_complex = ACube();
			_scale = 75.0f;
		}

		public AtomCube(float scale) {
			_scale = scale / 75.0f;
			_complex = new Scaling(ACube(), _scale);
		}

		private SDF ACube() {
			SDF obj = new Cylinder(25f, 125f);
			var c1 = new Cylinder(25f, 125f);
			var c2 = new Rotation(c1, 90, 0, 0);
			var c3 = new Rotation(c1, 0, 0, 90);
			var cylinders = new Union(c1, new Union(c2, c3));

			var t1 = new Torus(24f, 5f);
			var t2 = new Rotation(t1, 90, 0, 0);
			var t3 = new Rotation(t1, 0, 0, 90);
			var torus = new Union(t1, new Union(t2, t3));

			SDF cube = new Cube(75f);
			cube = new Subtraction(cube, cylinders);
			return new Union(cube, torus);
		}

		public override float Distance(Vec3 p) {
			return _complex.Distance(p);
		}

		public SDF Instance() {
			return this;
		}

		public override string ToString() {
			return $"AtomCube";
		}

		public override int RequiredGridSize() {
			return (int)Math.Ceiling(_scale * 75f);
		}

		public override bool Precise() {
			return true;
		}
	}
}
