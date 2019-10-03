using System;
using PolygonTriangulation.Model;

namespace PolygonTriangulation.ImplicitObjects {
	public class Capsule : SDF {
		Vec3 _start, _end;
		float _radius;

		/// <summary>
		/// Creates a capsule spanning between two given point with a given radius.
		/// </summary>
		/// <param name="start">A starting point in 3D space.</param>
		/// <param name="end">An end point in 3D space.</param>
		/// <param name="radius">The desired radius of the capsule.</param>
		public Capsule(Vec3 start, Vec3 end, float radius) {
			_start = start;
			_end = end;
			_radius = radius;
		}

		/// <summary>
		/// Creates a capsule with a given height and radius.
		/// </summary>
		/// <remarks>
		/// The capsule will be centered around (0,0,0).
		/// Note that the height is excluding the sphere part of the capsule.
		/// The total height will be height + 2 * radius.
		/// </remarks>
		public Capsule(float height, float radius) {
			_start = new Vec3(0, -height / 2, 0);
			_end = new Vec3(0, height / 2, 0);
			_radius = radius;
		}
		
		public override float Distance(Vec3 p) {
			return LineDist(p, _start, _end) - _radius;
		}

		private float LineDist(Vec3 p, Vec3 a, Vec3 b) {
			Vec3 ab = b - a;
			float t = Math.Max(0, Math.Min(1, Vec3.DotProduct(p - a, ab) / Vec3.DotProduct(ab, ab)));
			var v = (new Vec3(ab.X * t, ab.Y * t, ab.Z * t) + a) - p;
			return (float)Math.Sqrt(v.X * v.X + v.Y * v.Y + v.Z * v.Z);
		}

		public override bool Precise() {
			return true;
		}

        public override int RequiredGridSize() {
            //Finds the axis value furthest from 0
            var max = Math.Max(Math.Max(
                         Math.Max(Math.Abs(_start.X), Math.Abs(_start.Y)), 
                            Math.Max(Math.Abs(_start.Z), Math.Abs(_end.X))), 
                                Math.Max(Math.Abs(_end.Y), Math.Abs(_end.Z)));
            return (int) Math.Ceiling((max + _radius) * 2 + epsilon);
        }
    }
}
