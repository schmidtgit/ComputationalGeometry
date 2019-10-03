using System;
using System.Collections.Generic;
using PolygonTriangulation.Model;
using SimprExpression;

namespace PolygonTriangulation.ImplicitObjects {
	public class ParsedExpression : SDF {
		IExpression _expr;
		
		/// <summary>
		/// Creates a SDF based on the given IExpression.
		/// </summary>
		public ParsedExpression(IExpression expr) {
			_expr = expr;
		}

		public override float Distance(Vec3 p) {
			var d = new Dictionary<char, double>();
			d.Add('x', p.X);
			d.Add('y', p.Y);
			d.Add('z', p.Z);
			return (float) _expr.Compute(d);
		}

		public override string ToString() {
			return "User Expression";
		}
		
		public override bool Precise() {
			return false;
		}
	}
}
