using PolygonTriangulation.ImplicitObjects;
using PolygonTriangulation.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace PolygonTriangulationTests.ImplicitObjects.Primitives {
	public class SphereTests {
		[Fact(DisplayName = "Distance(Point on surface) => 0f")]
		public void DistanceTestSurface() {
			//Arrange
			var s = new Sphere(25);
			var v = new Vec3(0, 0, 25);

			//Act
			var dist = s.Distance(v);

			//Assert
			Assert.Equal(0, dist);
		}

		[Fact(DisplayName = "Distance((0,0,0)) => -radius")]
		public void DistanceTestCenter() {
			//Arrange
			var s = new Sphere(25);
			var v = new Vec3(0, 0, 0);

			//Act
			var dist = s.Distance(v);

			//Assert
			Assert.Equal(-25, dist);
		}

		[Fact(DisplayName = "Distance((0,0,2r)) => radius")]
		public void DistanceTestOutside() {
			//Arrange
			var s = new Sphere(25);
			var v = new Vec3(0, 0, 50);

			//Act
			var dist = s.Distance(v);

			//Assert
			Assert.Equal(25, dist);
		}

		[Fact(DisplayName = "Distance(Point on surface) => 0f")]
		public void DistanceTest() {
			//Arrange
			var s = new Sphere(25);
			var v = new Vec3(20, 30, 25);

			//Act
			var dist = s.Distance(v);

			//Assert
			Assert.Equal(43.87482 - 25, dist, 5);
		}
	}
}
