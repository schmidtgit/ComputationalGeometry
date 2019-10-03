using System;
using PolygonTriangulation.ImplicitObjects;

namespace Program.RuntimeOperator {
	class Scaling : ISDFOperator {
		SDF obj;
		public bool Setup(float[] f, SDF[] s) {
			if (s.Length != 1 && f.Length != 1)
				return false;
			obj = new PolygonTriangulation.ImplicitObjects.Scaling(s[0], f[0]);
			return true;
		}

		public string[] SDFParameters() {
			return new string[] { "signed distance function to scale" };
		}

		public string[] FloatParameters() {
			return new string[] { "scale" };
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
