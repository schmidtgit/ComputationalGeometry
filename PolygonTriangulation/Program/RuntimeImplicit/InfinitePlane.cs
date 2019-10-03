using PolygonTriangulation.ImplicitObjects;

namespace Program.RuntimeImplicit {
	class InfinitePlane : IRuntimeSDF {
		SDF obj;
		public SDF Instance() {
			return obj;
		}

		public string[] Parameters() {
			return new string[] { "ground level" };
		}

		public bool Setup(params float[] f) {
			if (f.Length != 1)
				return false;

			obj = new PolygonTriangulation.ImplicitObjects.InfinitePlane(f[0]);
			return true;
		}

		public override string ToString() {
			if (obj != null)
				return obj.ToString();
			return "- null -";
		}
	}
}
