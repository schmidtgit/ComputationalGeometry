using System;
using PolygonTriangulation.ImplicitObjects;

namespace Program.RuntimeOperator {
	public class Union : ISDFOperator {
		SDF obj;
		public string[] FloatParameters() {
			return new string[] { };
		}

		public string[] SDFParameters() {
			return new string[] { "first SDF", "second SDF" };
		}

		public bool Setup(float[] f, SDF[] s) {
			if (s.Length != 2)
				return false;
			obj = new PolygonTriangulation.ImplicitObjects.Union(s[0], s[1]);
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
