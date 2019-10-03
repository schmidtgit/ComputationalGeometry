using PolygonTriangulation.ImplicitObjects;

namespace Program.RuntimeImplicit {
	class Box : IRuntimeSDF {
		SDF obj;

		public SDF Instance() {
			return obj;
		}

		public string[] Parameters() {
			return new string[] { "height", "width", "length" };
		}

		public bool Setup(params float[] f) {
			if (f.Length <= 2)
				return false;
			obj = new PolygonTriangulation.ImplicitObjects.Box(f[1], f[0], f[2]);
			return true;
		}

		public override string ToString() {
			if (obj != null)
				return obj.ToString();
			return "- null -";
		}
	}
}
