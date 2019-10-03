using System;
using PolygonTriangulation.Model;

namespace PolygonTriangulation.ImplicitObjects.Primitives {
	public class GoursatsSurface: SDF {
		float _a, _b, _c, _scale;
		
		/// <summary>
		/// Creates a scaled Goursats Surface based on the formula:
		/// x^4+y^4+z^4+a(x^2+y^2+z^2)^2+b(x^2+y^2+z^2)+c
		/// Where x, y and z is the vector location.
		/// </summary>
		public GoursatsSurface(float a, float b, float c, float scale) {
			_a = a; _b = b; _c = c; _scale = scale;
		}

		public override float Distance(Vec3 p) {
			//  x^4+y^4+z^4+a(x^2+y^2+z^2)^2+b(x^2+y^2+z^2)+c
			p = p / _scale;
			double x = p.X; double y = p.Y; double z = p.Z;
			var result = (Math.Pow(x, 4.0) + Math.Pow(y, 4.0) + Math.Pow(z, 4.0) + _a * Math.Pow(Math.Pow(x, 2.0) + Math.Pow(y, 2.0) + Math.Pow(z, 2.0), 2.0) + _b * (Math.Pow(x, 2.0) + Math.Pow(y, 2.0) + Math.Pow(z, 2.0)) + _c);
			return (float) result * _scale;
		}

		public override bool Precise() {
			return false;
		}

		public override string ToString() {
			return $"Goursat's Surface {_a},{_b},{_c}";
		}
	}
}
