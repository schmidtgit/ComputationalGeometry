using System;

namespace PolygonTriangulation.Model {
	public class Vec3 {
		public readonly float X;
		public readonly float Y;
		public readonly float Z;

		/// <summary>
		/// Shortcut to get the zero vector.
		/// Always returns the same pointer for faster comparison.
		/// </summary>
		public static Vec3 zero { get; } = new Vec3(0, 0, 0);

		/// <summary>
		/// Returns the dot product of the two given vectors.
		/// (a.X * b.X + a.Y * b.Y + a.Z * b.Z).
		/// </summary>
		/// <param name="a">First vector.</param>
		/// <param name="b">Second vector.</param>
		/// <returns>Dot product.</returns>
		public static float DotProduct(Vec3 a, Vec3 b) {
			return a.X * b.X + a.Y * b.Y + a.Z * b.Z;
		}

		/// <summary>
		/// Returns the cross product of the two given vectors.
		/// </summary>
		/// <param name="a">First vector.</param>
		/// <param name="b">Second vector.</param>
		/// <returns>Cross product.</returns>
		public static Vec3 CrossProduct(Vec3 a, Vec3 b) {
			return new Vec3(a.Y * b.Z - a.Z * b.Y,
							a.Z * b.X - a.X * b.Z,
							a.X * b.Y - a.Y * b.X);
		}

		/// <summary>
		/// Returns the cross product as a vector with a magnitude of 0.
		/// </summary>
		/// <param name="a">First vector.</param>
		/// <param name="b">Second vector.</param>
		/// <returns>Normalized cross product.</returns>
		public static Vec3 NormalizedCrossProduct (Vec3 a, Vec3 b) {
			return CrossProduct(a, b).Normalize();
		}

		/// <summary>
		/// Default constructor.
		/// </summary>
		/// <param name="x">First coordinate.</param>
		/// <param name="y">Second coordinate.</param>
		/// <param name="z">Third coordinate.</param>
		public Vec3(float x, float y, float z) {
			X = x;
			Y = y;
			Z = z;
		}

		/// <summary>
		/// For faster comparison of magnitude.
		/// </summary>
		/// <returns>Magnitude^2</returns>
		public float SquaredMagnitude() {
			return X * X + Y * Y + Z * Z;
		}

		/// <summary>
		/// The length of the vector.
		/// </summary>
		/// <returns>Length of vector.</returns>
		public float Magnitude() {
			return (float) Math.Sqrt(X * X + Y * Y + Z * Z);
		}

		/// <summary>
		/// Returns a new vector with the length of 1.
		/// </summary>
		/// <returns>Normalized vector.</returns>
		public Vec3 Normalize() {
            var length = Magnitude();
			return length == 0 ? zero : this / length;
		}
		
		public static Vec3 operator + (Vec3 a, Vec3 b) {
			return new Vec3(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
		}

		public static Vec3 operator - (Vec3 a, Vec3 b) {
			return new Vec3(a.X - b.X, a.Y - b.Y, a.Z - b.Z);
		}

		public static Vec3 operator * (Vec3 v, int i) {
			return new Vec3(v.X * i, v.Y * i, v.Z * i);
		}

		public static Vec3 operator * (int i, Vec3 v) {
			return v * i;
		}

		public static Vec3 operator *(Vec3 v, float i) {
			return new Vec3(v.X * i, v.Y * i, v.Z * i);
		}

		public static Vec3 operator *(float i, Vec3 v) {
			return v * i;
		}

		public static Vec3 operator / (Vec3 v, int i) {
			return new Vec3(v.X / i, v.Y / i, v.Z / i);
		}

		public static Vec3 operator / (Vec3 v, float f) {
			return new Vec3(v.X / f, v.Y / f, v.Z / f);
		}

		public override bool Equals(Object other) {
			// Required to pass test, but slow?
			if (float.IsInfinity(X) || float.IsNegativeInfinity(X) || float.IsPositiveInfinity(X))
				return false;
			if (float.IsInfinity(Y) || float.IsNegativeInfinity(Y) || float.IsPositiveInfinity(Y))
				return false;
			if (float.IsInfinity(Z) || float.IsNegativeInfinity(Z) || float.IsPositiveInfinity(Z))
				return false;
			// End required to pass test

			if (other is Vec3) {
				var o = other as Vec3;
				return o != null && X == o.X && Y == o.Y && Z == o.Z;
			} else
				return false;
		}

		public override int GetHashCode() {
			return X.GetHashCode() ^ Y.GetHashCode() << 2 ^ Z.GetHashCode() >> 2;
		} 

        public override string ToString()
        {
            return $"(X:{X},Y:{Y},Z:{Z})";
        }
    }
}
