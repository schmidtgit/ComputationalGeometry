using System;
using PolygonTriangulation.Model;

namespace PolygonTriangulation.ImplicitObjects.Complex {
	public class AtomCubeSliced : SDF {
		SDF _complex;
		float _scale;
		public AtomCubeSliced() {
			_scale = 75.0f;
			_complex = ACube();
		}

		public AtomCubeSliced(float scale) {
			_scale = scale / 75.0f;
			_complex = new Scaling(ACube(), _scale);
		}

		private SDF ACube() {
			SDF atom = new AtomCube();
			SDF plane = new Rotation(new InfinitePlane(0), -45, 0, 0);
			return new Subtraction(atom, plane);
		}

		public override float Distance(Vec3 p) {
			return _complex.Distance(p);
		}

		public override string ToString() {
			return $"AtomCube Sliced";
		}

		public override int RequiredGridSize() {
			return (int) Math.Ceiling(_scale * 75f);
		}

		public override bool Precise() {
			return true;
		}
	}
}
