using System;
using PolygonTriangulation.Model;

namespace PolygonTriangulation.ImplicitObjects {
	public class Cone : SDF {
		private float _height, _radius;
        //Used to center the cone around (0,0,0)
        private Vec3 _offset;

		/// <summary>
		/// Creates a cone with the desired height and radius.
		/// </summary>
		/// <remarks>
		/// The cone is centered around (0,0,0).
		/// The cone points upwards in the 3D space.
		/// </remarks>
		/// <param name="height">The total height of the cone.</param>
		/// <param name="radius">The radius of the bottom of the cone.</param>
		public Cone(float height, float radius) {
			_height = height;
			_radius = radius;
            _offset = new Vec3(0, _height / 2, 0);
		}
		
		public override float Distance(Vec3 pp) {
            var p = pp + _offset;
			//From http://mercury.sexy/hg_sdf/
			var q = new Vec2(new Vec2(p.X, p.Z).Magnitude(), p.Y);
			var tip = q - new Vec2(0, _height);
			var mantleDir = new Vec2(_height, _radius).Normalize();

			var mantle = Vec2.DotProduct(tip, mantleDir);
			var d = Math.Max(mantle, -q.Y);
			var projected = Vec2.DotProduct(tip, new Vec2(mantleDir.Y, mantleDir.X));

			if(q.Y > _height && projected < 0) {
				d = (float) Math.Max(d, tip.Magnitude());
			}

			if(q.X > _radius && projected > new Vec2(_height, _radius).Magnitude()) {
				d = (float) Math.Max(d, (q - new Vec2(_radius, 0)).Magnitude());
			}

			return d;
		}

		public override string ToString() {
			return $"Cone h{_height}/r{_radius}";
		}

		public override bool Precise() {
			return true;
		}

        public override int RequiredGridSize() {
            return (int) Math.Ceiling(Math.Max(_height, _radius * 2) + epsilon);
        }
    }
}
