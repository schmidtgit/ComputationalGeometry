using System;
using PolygonTriangulation.Model;

namespace PolygonTriangulation.ImplicitObjects {
	public class Subtraction: SDF {
		private SDF _fst, _snd;

		/// <summary>
		/// Substract one object from another.
		/// </summary>
		/// <param name="first">The original object.</param>
		/// <param name="second">The object to be removed.</param>
		public Subtraction(SDF first, SDF second) {
			_fst = first;
			_snd = second;
		}

		public override bool Contained(Vec3 p) {
			return _fst.Contained(p) && !_snd.Contained(p);
		}

		public override float Distance(Vec3 p) {
			return Math.Max(_fst.Distance(p), -_snd.Distance(p));
		}

		public override string ToString() {
			var first = _fst == null ? "_____" : _fst.ToString();
			var second = _snd == null ? "_____" : _snd.ToString();
			return $"({first}) substraction ({second})";
		}

		public override bool Precise() {
			return _fst.Precise() && _snd.Precise();
		}

		public override int RequiredGridSize() {
			return _fst.RequiredGridSize();
		}
	}
}
