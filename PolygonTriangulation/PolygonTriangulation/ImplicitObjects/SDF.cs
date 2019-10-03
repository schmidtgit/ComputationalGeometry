using PolygonTriangulation.Model;

namespace PolygonTriangulation.ImplicitObjects {
	public abstract class SDF {
		protected float epsilon = 0.1f;
		/// <summary>
		/// Returns the distance to the surface of the SDF from the given point in space.
		/// Might not give the true distance to the surface.
		/// </summary>
		/// <remarks>
		/// Returns a negative value, if the point is inside the object.
		/// Returns a positive value, if the point is outside the object.
		/// Returns 0, if the point is on the surface of the object.
		/// </remarks>
		/// <param name="p">A point in 3D space</param>
		/// <returns>Distance as a float value</returns>
		public abstract float Distance(Vec3 p);

		/// <summary>
		/// Returns whether or not the point is inside the object.
		/// </summary>
		/// <remarks>
		/// This call is equal to 0 >= Distance(p)
		/// Use this if the actual distance is not important.
		/// </remarks>
		/// <param name="p">A point in 3D space</param>
		/// <returns>True if the point is contained within the object.</returns>
		public virtual bool Contained(Vec3 p) {
			return Distance(p) <= 0;
		}

		/// <summary>
		/// Returns the minimum required grid size to triangulate this SDF correctly.
		/// Returns 0 for infinite (unknown size)
		/// </summary>
		public virtual int RequiredGridSize() {
			return 0;
		}

		/// <summary>
		/// Returns whether or not the distance-function is precise.
		/// </summary>
		public abstract bool Precise();

		public static SDF operator + (SDF a, SDF b) {
			return new Union(a,b);
		}

		public static SDF operator - (SDF a, SDF b) {
			return new Subtraction(a, b);
		}

		public static SDF operator & (SDF a, SDF b) {
			return new Intersection(a, b);
		}

		public static SDF operator - (SDF a, Vec3 v) {
			return new Transformation(a, -1*v);
		}

		public static SDF operator + (SDF a, Vec3 v) {
			return new Transformation(a, v);
		}

		public static SDF operator * (SDF a, float f) {
			return new Scaling(a, f);
		}

		public static SDF operator ^(SDF a, Vec3 v) {
			return new Rotation(a, v.X, v.Y, v.Z);
		}
	}
}
