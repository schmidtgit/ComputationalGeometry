using PolygonTriangulation.Model;
using System;

namespace PolygonTriangulation.ImplicitObjects {
	public class Transformation : SDF {
		private SDF _original;
		private Vec3 _tranform;

		/// <summary>
		/// Moves an object the desired distance.
		/// </summary>
		/// <param name="original">The original object.</param>
		/// <param name="transform">The x, y and z distances used to move the object.</param>
		public Transformation(SDF original, Vec3 transform) {
			_original = original;
			_tranform = transform;
		}

		public override float Distance(Vec3 p) {
			return _original.Distance(p - _tranform);
		}

		public override string ToString() {
			var first = _original == null ? "_____" : _original.ToString();
			return $"{first} moved by {_tranform.X}|{_tranform.Y}|{_tranform.Z}";
		}

		public override bool Precise() {
			return _original.Precise();
		}

        public override int RequiredGridSize() {
            var maxoffset = Math.Max(Math.Max(_tranform.X, _tranform.Y), _tranform.Z);
            return (int) Math.Ceiling(_original.RequiredGridSize() + 2 * maxoffset);
        }
    }
}
