using PolygonTriangulation.ImplicitObjects;

namespace Program.RuntimeImplicit {
	class Cone: IRuntimeSDF {
		SDF obj;
		public SDF Instance() {
			return obj;
		}

		public string[] Parameters() {
			return new string[] { "height", "bottom radius" };
		}

		public bool Setup(params float[] f) {
			if (f.Length != 2)
				return false;

			obj = new PolygonTriangulation.ImplicitObjects.Cone(f[0], f[1]);
			return true;
		}

		public override string ToString() {
			if (obj != null)
				return obj.ToString();
			return "- null -";
		}
	}
}
