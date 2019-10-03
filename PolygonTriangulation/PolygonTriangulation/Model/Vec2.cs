using System;
namespace PolygonTriangulation.Model {
	public class Vec2 {
		public readonly float X;
		public readonly float Y;

		/// <summary>
		/// Returns the dot product of the two given vectors.
		/// (a.X * b.X + a.Y * b.Y).
		/// </summary>
		/// <param name="a">First vector.</param>
		/// <param name="b">Second vector.</param>
		/// <returns>Dot product.</returns>
		public static float DotProduct(Vec2 a, Vec2 b) {
			return a.X * b.X + a.Y * b.Y;
		}

		/// <summary>
		/// Default constructor.
		/// </summary>
		/// <param name="x">First coordinate.</param>
		/// <param name="y">Second coordinate.</param>
		public Vec2(float x, float y) {
			X = x;
			Y = y;
		}

		/// <summary>
		/// The length of the vector.
		/// </summary>
		/// <returns>Length of vector.</returns>
		public float Magnitude() {
			return (float) Math.Sqrt(X * X + Y * Y);
		}

		/// <summary>
		/// Returns a new vector with the length of 1.
		/// </summary>
		/// <returns>Normalized vector.</returns>
		public Vec2 Normalize() {
			return this / Magnitude();
		}

		public static Vec2 operator +(Vec2 a, Vec2 b) {
			return new Vec2(a.X + b.X, a.Y + b.Y);
		}

		public static Vec2 operator -(Vec2 a, Vec2 b) {
			return new Vec2(a.X - b.X, a.Y - b.Y);
		}

		public static Vec2 operator *(Vec2 v, int i) {
			return new Vec2(v.X * i, v.Y * i);
		}

		public static Vec2 operator *(int i, Vec2 v) {
			return v * i;
		}

		public static Vec2 operator /(Vec2 v, int i) {
			return new Vec2(v.X / i, v.Y / i);
		}

		public static Vec2 operator /(Vec2 v, float f) {
			return new Vec2(v.X / f, v.Y / f);
		}

		public override bool Equals(Object other) {
			if(other is Vec2) {
				var o = other as Vec2;
				return o != null && X == o.X && Y == o.Y;
			} else
				return false;
		}

		public override int GetHashCode() {
			return X.GetHashCode() ^ Y.GetHashCode();
		}
	}
}
