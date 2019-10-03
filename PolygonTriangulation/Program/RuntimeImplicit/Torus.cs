using PolygonTriangulation.ImplicitObjects;

namespace Program.RuntimeImplicit {
	class Torus : IRuntimeSDF{
		SDF obj;
		SDF IRuntimeSDF.Instance() {
			return obj;
		}

		string[] IRuntimeSDF.Parameters() {
			return new string[] { "donut hole radius", "radius ('thickness') of torus" };
		}

		bool IRuntimeSDF.Setup(params float[] f) {
			if (f.Length <= 1)
				return false;

			obj = new PolygonTriangulation.ImplicitObjects.Torus(f[0], f[1]);
			return true;
		}

		public override string ToString() {
			if (obj != null)
				return obj.ToString();
			return "- null -";
		}
	}
}
