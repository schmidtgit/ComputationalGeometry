using PolygonTriangulation.ImplicitObjects;
using System;

namespace Program.RuntimeOperator {
	class Rotation : ISDFOperator {
		SDF obj;
		public bool Setup(float[] f, SDF[] s) {
			if (s.Length != 1 && f.Length != 3)
				return false;
			var x = (float) (f[0] % 360 * Math.PI) / 180;
			var y = (float) (f[1] % 360 * Math.PI) / 180;
			var z = (float) (f[2] % 360 * Math.PI) / 180;
			obj = new PolygonTriangulation.ImplicitObjects.Rotation(s[0], x, y, z);
			return true;
		}

		public string[] SDFParameters() {
			return new string[] { "signed distance function to rotate" };
		}

		public string[] FloatParameters() {
			return new string[] { "degrees to rotate around the x-axis", "degrees to rotate around the y-axis", "degrees to rotate around the z-axis" };
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
