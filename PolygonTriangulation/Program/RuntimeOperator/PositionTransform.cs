using PolygonTriangulation.ImplicitObjects;

namespace Program.RuntimeOperator {
	class PositionTransform : ISDFOperator {
		SDF obj;
		public string[] FloatParameters() {
			return new string[] { "units to move along x-axis", "units to move along y-axis", "units to move along z-axis" };
		}

		public string[] SDFParameters() {
			return new string[] { "signed distance function to move" };
		}

		public bool Setup(float[] f, SDF[] s) {
			if (s.Length != 1 && f.Length != 3)
				return false;

			obj = new PolygonTriangulation.ImplicitObjects.Transformation(s[0], new PolygonTriangulation.Model.Vec3(f[0],f[1],f[2]));
			return true;
		}

		public SDF Instance() {
			return obj;
		}

		public override string ToString() {
			if (obj != null)
				return obj.ToString();
			return "- null -";
		}
	}
}
