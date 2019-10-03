using Xunit;
using PolygonTriangulation.ImplicitObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PolygonTriangulation.Model;

namespace PolygonTriangulation.ImplicitObjects.Tests {
	public class SubtractionTests {
		[Fact(DisplayName = "Distance(point inside) => correct distance")]
		public void DistanceTestInsideCloseToSphere() {
			//Arrange
			var c = new Cube(100);
			var s = new Sphere(35);
			var sub = new Subtraction(c, s);
			var p = new Vec3(0,0, 40);

			//Act
			var dist = sub.Distance(p);

			//Assert
			Assert.Equal(-5, dist, 5);
		}

		[Fact(DisplayName = "Distance(point inside) => correct distance")]
		public void DistanceTestInsideCloseToCube() {
			//Arrange
			var c = new Cube(100);
			var s = new Sphere(35);
			var sub = new Subtraction(c, s);
			var p = new Vec3(15, 20, 45);

			//Act
			var dist = sub.Distance(p);

			//Assert
			Assert.Equal(-5, dist, 5);
		}

		[Fact(DisplayName = "Distance((0,0,0)) => distance to inner value")]
		public void DistanceTestCentered() {
			//Arrange
			var c = new Cube(100);
			var s = new Sphere(35);
			var sub = new Subtraction(c, s);
			var p = new Vec3(0, 0, 0);

			//Act
			var dist = sub.Distance(p);

			//Assert
			Assert.Equal(35, dist, 5);
		}

		[Fact(DisplayName = "Distance(point outside) => correct distance")]
		public void DistanceTestOutside() {
			//Arrange
			var c = new Cube(100);
			var s = new Sphere(35);
			var sub = new Subtraction(c, s);
			var p = new Vec3(20, 30, 70);

			//Act
			var dist = sub.Distance(p);

			//Assert
			Assert.Equal(20, dist, 5);
		}

		[Fact(DisplayName = "Distance(point on inner surface) => 0")]
		public void DistanceTestSurfaceSphere() {
			//Arrange
			var c = new Cube(100);
			var s = new Sphere(35);
			var sub = new Subtraction(c, s);
			var p = new Vec3(0, 0, 35);

			//Act
			var dist = sub.Distance(p);

			//Assert
			Assert.Equal(0, dist, 5);
		}

		[Fact(DisplayName = "Distance(point on outer surface) => 0")]
		public void DistanceTest() {
			//Arrange
			var c = new Cube(100);
			var s = new Sphere(35);
			var sub = new Subtraction(c, s);
			var p = new Vec3(20, 30, 50);

			//Act
			var dist = sub.Distance(p);

			//Assert
			Assert.Equal(0, dist, 5);
		}
	}
}