namespace PolygonTriangulation.Model {
	/// <summary>
	/// Alternative way to store polygons
	/// </summary>
	public class Triangle {
		public Vec3 First { get; set; }
		public Vec3 Second { get; set; }
		public Vec3 Third { get; set; }

		public Triangle(Vec3 first, Vec3 second, Vec3 third) {
			First = first;
			Second = second;
			Third = third;
		}
	}
}
