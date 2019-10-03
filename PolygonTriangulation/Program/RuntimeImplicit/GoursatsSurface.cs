using PolygonTriangulation.ImplicitObjects;

namespace Program.RuntimeImplicit {
	class GoursatsSurface : IRuntimeSDF {
		SDF obj;
		public SDF Instance() {
			return obj;
		}

		public string[] Parameters() {
			return new string[] { "a", "b", "c", "scale" };
		}

		public bool Setup(params float[] f) {
			if (f.Length != 4)
				return false;

			obj = new PolygonTriangulation.ImplicitObjects.Primitives.GoursatsSurface(f[0], f[1], f[2], f[3]);
			return true;
		}

		public override string ToString() {
			if (obj != null)
				return obj.ToString();
			return "- null -";
		}
	}
}
