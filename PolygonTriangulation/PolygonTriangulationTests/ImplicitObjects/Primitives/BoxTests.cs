using Xunit;
using PolygonTriangulation.ImplicitObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PolygonTriangulation.Model;

namespace PolygonTriangulation.ImplicitObjects.Tests {
	public class BoxTests {
		[Fact(DisplayName = "Distance((0,0,0)) => -smallest length/2")]
		public void DistanceTestCentered() {
			//Arrange
			var b = new Box(30, 25, 50);
			var v = new Vec3(0, 0, 0);

			//Act
			var dist = b.Distance(v);

			//Assert
			Assert.Equal(-12.5, dist, 5);
		}

		[Fact(DisplayName = "Distance(point on surface) => 0f")]
		public void DistanceTestSurface() {
			//Arrange
			var b = new Box(30, 25, 50);
			var v = new Vec3(15, 12.5f, 25);

			//Act
			var dist = b.Distance(v);

			//Assert
			Assert.Equal(0, dist, 5);
		}

		[Fact(DisplayName = "Distance(point outside box) => correct distance")]
		public void DistanceTest() {
			//Arrange
			var b = new Box(30, 25, 50);
			var v = new Vec3(5, 10, 37.5f);

			//Act
			var dist = b.Distance(v);

			//Assert
			Assert.Equal(12.5, dist, 5);
		}
	}
}