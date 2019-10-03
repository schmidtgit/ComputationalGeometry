using PolygonTriangulation.ImplicitObjects;

namespace Program.RuntimeImplicit {
	class Cube : IRuntimeSDF {
		SDF obj;
		SDF IRuntimeSDF.Instance() {
			return obj;
		}

		string[] IRuntimeSDF.Parameters() {
			return new string[] { "side length" };
		}

		bool IRuntimeSDF.Setup(params float[] f) {
			if (f.Length != 1)
				return false;

			obj = new PolygonTriangulation.ImplicitObjects.Cube(f[0]);
			return true;
		}

		public override string ToString() {
			if (obj != null)
				return obj.ToString();
			return "- null -";
		}
	}
}
