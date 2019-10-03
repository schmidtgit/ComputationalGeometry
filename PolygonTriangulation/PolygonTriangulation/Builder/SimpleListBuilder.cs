using System;
using System.Collections.Generic;
using PolygonTriangulation.Model;
using PolygonTriangulation.Polygon;

namespace PolygonTriangulation.Builder {
	/// <summary>
	/// Optimized for quick insertion, does not reuse vertices.
	/// </summary>
	public class SimpleListBuilder : IPolygonBuilder {
		private List<Triangle> triangles;

		public SimpleListBuilder() {
			triangles = new List<Triangle>();
		}

		public void Append(Vec3 a, Vec3 b, Vec3 c) {
			triangles.Add(new Triangle(a, b, c));
		}

		public void Append(Vec3 a, Vec3 b, Vec3 c, Vec3 origin) {
			triangles.Add(new Triangle(a + origin, b + origin, c + origin));
		}

		public IPolygon Build() {
			return new ListPolygon(triangles);
		}

		public void Clear() {
			triangles.Clear();
		}
	}
}
