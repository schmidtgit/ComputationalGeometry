using System;
using PolygonTriangulation.Model;

namespace PolygonTriangulation.ImplicitObjects {
	public class Union: SDF {
		private SDF _fst, _snd;

		/// <summary>
		/// Combines two objects.
		/// </summary>
		/// <param name="first">The original object.</param>
		/// <param name="second">The object to be added.</param>
		public Union(SDF first, SDF second) {
			_fst = first;
			_snd = second;
		}

		public override bool Contained(Vec3 p) {
			return _fst.Contained(p) || _snd.Contained(p);
		}

		public override float Distance(Vec3 p) {
			return Math.Min(_fst.Distance(p), _snd.Distance(p));
		}

		public override string ToString() {
			var first = _fst == null ? "_____" : _fst.ToString();
			var second = _snd == null ? "_____" : _snd.ToString();
			return $"({first}) union ({second})";
		}

		public override int RequiredGridSize() {
			if (_fst.RequiredGridSize() == 0 || _snd.RequiredGridSize() == 0)
				return 0;

			return Math.Max(_fst.RequiredGridSize(), _snd.RequiredGridSize());
		}

		public override bool Precise() {
			return _fst.Precise() && _snd.Precise();
		}
	}
}
