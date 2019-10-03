using System;
using PolygonTriangulation.ImplicitObjects;

namespace Program.RuntimeImplicit {
	class Sine : IRuntimeSDF {
		SDF obj;

		public SDF Instance() {
			return obj;
		}

		string[] IRuntimeSDF.Parameters() {
			return new string[] { "scale" };
		}
		bool IRuntimeSDF.Setup(params float[] f) {
			if (f.Length != 1)
				return false;

			obj = new PolygonTriangulation.ImplicitObjects.Sine(f[0]);
			return true;
		}

		public override string ToString() {
			if (obj != null)
				return obj.ToString();
			return "- null -";
		}
	}
}
