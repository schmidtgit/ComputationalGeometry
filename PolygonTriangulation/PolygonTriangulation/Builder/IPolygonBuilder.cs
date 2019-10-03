using PolygonTriangulation.Model;
using PolygonTriangulation.Polygon;

namespace PolygonTriangulation.Builder {
	/// <summary>
	/// Facilitates creation of IPolygons.
	/// </summary>
	public interface IPolygonBuilder {
		/// <summary>
		/// Prepares the builder for reuse.
		/// </summary>
		void Clear();
		/// <summary>
		/// Adds a triangle, represented by the three vertices, to the polygon.
		/// Expected counter-clockwise direction. 
		/// </summary>
		/// <param name="a">First point in triangle.</param>
		/// <param name="b">Second point in triangle.</param>
		/// <param name="c">Third point in triangle.</param>
		void Append(Vec3 a, Vec3 b, Vec3 c);
		/// <summary>
		/// Created for easier appending when doing gridbased algorithms.
		/// Applies offset to each vertex to each vector before adding them.
		/// </summary>
		/// <param name="a">First point in triangle.</param>
		/// <param name="b">Second point in triangle.</param>
		/// <param name="c">Third point in triangle.</param>
		/// <param name="offset">Constant to add to each vector.</param>
		void Append(Vec3 a, Vec3 b, Vec3 c, Vec3 offset);
		/// <summary>
		/// Builds an IPolygon from all given vertices.
		/// </summary>
		IPolygon Build();
	}
}
