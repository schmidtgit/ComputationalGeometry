using Xunit;
using PolygonTriangulation.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PolygonTriangulation.Model.Tests {
	public class Vec3Tests {
		[Fact(DisplayName = "PLUS (0,0,-1) + (1,1,0) => (1,1,-1)")]
		public void OperatorPlus0() {
			// Arrange
			Vec3 a = new Vec3(0, 0, -1);
			Vec3 b = new Vec3(1, 1, 0);
			// Act
			Vec3 r1 = a + b;
			Vec3 r2 = b + a;
			// Assert
			Assert.Equal(r2.X, r1.X);
			Assert.Equal(r2.Y, r1.Y);
			Assert.Equal(r2.Z, r1.Z);
			Assert.Equal(1f, r1.X);
			Assert.Equal(1f, r1.Y);
			Assert.Equal(-1f, r1.Z);
		}

		[Fact(DisplayName = "PLUS (1f,5f,2f) + (3f,-4f,-3f) => (4f,1f,-1f)")]
		public void OperatorPlus1() {
			// Arrange
			Vec3 a = new Vec3(1f, 5f, 2f);
			Vec3 b = new Vec3(3f, -4f, -3f);
			// Act
			Vec3 r1 = a + b;
			Vec3 r2 = b + a;
			// Assert
			Assert.Equal(r2.X, r1.X);
			Assert.Equal(r2.Y, r1.Y);
			Assert.Equal(r2.Z, r1.Z);
			Assert.Equal(4f, r1.X);
			Assert.Equal(1f, r1.Y);
			Assert.Equal(-1f, r1.Z);
		}

		[Fact(DisplayName = "PLUS (10.5f,0f,2f) + (0.6f,0f,0f) => (11.1f,0f,2f)")]
		public void OperatorPlus2() {
			// Arrange
			Vec3 a = new Vec3(10.5f, 0f, 2f);
			Vec3 b = new Vec3(0.6f, 0f, 0f);
			// Act
			Vec3 r1 = a + b;
			Vec3 r2 = b + a;
			// Assert
			Assert.Equal(r2.X, r1.X);
			Assert.Equal(r2.Y, r1.Y);
			Assert.Equal(r2.Z, r1.Z);
			Assert.Equal(11.1f, r1.X);
			Assert.Equal(0f, r1.Y);
			Assert.Equal(2f, r1.Z);
		}

		[Fact(DisplayName = "MINUS (10.5f,0f,2f) - (0.6f,0f,0f) => (9.9f,0f,2f)")]
		public void OperatorMinus0() {
			// Arrange
			Vec3 a = new Vec3(10.5f, 0f, 2f);
			Vec3 b = new Vec3(0.6f, 0f, 0f);
			// Act
			Vec3 r = a - b;
			// Assert
			Assert.Equal(9.9f, r.X);
			Assert.Equal(0f, r.Y);
			Assert.Equal(2f, r.Z);
		}

		[Fact(DisplayName = "MINUS (0f,0f,2f) - (5f,0f,1f) => (-5f,0f,1f)")]
		public void OperatorMinus1() {
			// Arrange
			Vec3 a = new Vec3(0f, 0f, 2f);
			Vec3 b = new Vec3(5f, 0f, 1f);
			// Act
			Vec3 r = a - b;
			// Assert
			Assert.Equal(-5f, r.X);
			Assert.Equal(0f, r.Y);
			Assert.Equal(1f, r.Z);
		}

		[Fact(DisplayName = "MINUS (-5f,-3f,-1f) - (-1f,0f,1f) => (-4f,-3f,-2f)")]
		public void OperatorMinus2() {
			// Arrange
			Vec3 a = new Vec3(-5f, -3f, -1f);
			Vec3 b = new Vec3(-1f, 0f, 1f);
			// Act
			Vec3 r = a - b;
			// Assert
			Assert.Equal(-4f, r.X);
			Assert.Equal(-3f, r.Y);
			Assert.Equal(-2f, r.Z);
		}

		[Fact(DisplayName = "DIVIDE (10f, 4f, -2f) / 2 => (5f,2f,-1f)")]
		public void OperatorDivide0() {
			// Arrange
			Vec3 a = new Vec3(10f, 4f, -2f);
			// Act
			Vec3 r = a / 2;
			// Assert
			Assert.Equal(5f, r.X);
			Assert.Equal(2f, r.Y);
			Assert.Equal(-1f, r.Z);
		}

		[Fact(DisplayName = "DIVIDE (10f, 4f, -2f) / 0 => NaN")]
		public void OperatorDivide1() {
			// Arrange
			Vec3 a = new Vec3(10f, 4f, -2f);
			// Act
			Vec3 r = a / 0;
			// Assert
			Assert.Equal(float.PositiveInfinity, r.X);
			Assert.Equal(float.PositiveInfinity, r.Y);
			Assert.Equal(float.NegativeInfinity, r.Z);
		}

		[Fact(DisplayName = "DIVIDE (2f, 3.3f, 0f) / 0.5f => (4f, 6.6f, 0f)")]
		public void OperatorDivide2() {
			// Arrange
			Vec3 a = new Vec3(2f, 3.3f, 0f);
			// Act
			Vec3 r = a / 0.5f;
			// Assert
			Assert.Equal(4f, r.X);
			Assert.Equal(6.6f, r.Y);
			Assert.Equal(0f, r.Z);
		}

		[Fact(DisplayName = "MULTIPLY (2f, 3.3f, 0f) * 0.5f => (1f, 1.65f, 0f)")]
		public void OperatorMultiply0() {
			// Arrange
			Vec3 a = new Vec3(2f, 3.3f, 0f);
			// Act
			Vec3 r1 = a * 0.5f;
			Vec3 r2 = 0.5f * a;
			// Assert
			Assert.Equal(1f, r1.X);
			Assert.Equal(1.65f, r1.Y);
			Assert.Equal(0f, r1.Z);
			Assert.Equal(1f, r2.X);
			Assert.Equal(1.65f, r2.Y);
			Assert.Equal(0f, r2.Z);
		}

		[Fact(DisplayName = "MULTIPLY (-1f, 7.7f, 0f) * 10 => (-10f, 77f, 0f)")]
		public void OperatorMultiply1() {
			// Arrange
			Vec3 a = new Vec3(-1f, 7.7f, 0f);
			// Act
			Vec3 r1 = a * 10;
			Vec3 r2 = 10 * a;
			// Assert
			Assert.Equal(-10f, r1.X);
			Assert.Equal(77f, r1.Y);
			Assert.Equal(0f, r1.Z);
			Assert.Equal(-10f, r2.X);
			Assert.Equal(77f, r2.Y);
			Assert.Equal(0f, r2.Z);
		}

		[Fact(DisplayName = "DotProduct (0,0,0)(0,0,0) => 0)")]
		public void DotProductTest0() {
			// Arrange
			Vec3 a = new Vec3(0, 0, 0);
			Vec3 b = new Vec3(0, 0, 0);
			// Act
			var r1 = Vec3.DotProduct(a, b);
			var r2 = Vec3.DotProduct(b, a);
			// Assert
			Assert.Equal(0f, r1);
			Assert.Equal(0f, r2);
		}

		[Fact(DisplayName = "DotProduct (0,19f,0)(-7f,0,0) => 0)")]
		public void DotProductTest1() {
			// Arrange
			Vec3 a = new Vec3(0, 19f, 0);
			Vec3 b = new Vec3(-7f, 0, 0);
			// Act
			var r1 = Vec3.DotProduct(a, b);
			var r2 = Vec3.DotProduct(b, a);
			// Assert
			Assert.Equal(0f, r1);
			Assert.Equal(0f, r2);
		}

		[Fact(DisplayName = "DotProduct (22,19f,-2f)(-7f,492f,2f) => 9190)")]
		public void DotProductTest2() {
			// Arrange
			Vec3 a = new Vec3(22f, 19f, -2f);
			Vec3 b = new Vec3(-7f, 492f, 2f);
			// Act
			var r1 = Vec3.DotProduct(a, b);
			var r2 = Vec3.DotProduct(b, a);
			// Assert
			Assert.Equal(9190f, r1, 5);
			Assert.Equal(9190f, r2, 5);
		}

		[Fact(DisplayName = "DotProduct (22,-19f,-2f)(7f,492f,-2f) => -9190)")]
		public void DotProductTest3() {
			// Arrange
			Vec3 a = new Vec3(22f, -19f, -2f);
			Vec3 b = new Vec3(7f, 492f, -2f);
			// Act
			var r1 = Vec3.DotProduct(a, b);
			var r2 = Vec3.DotProduct(b, a);
			// Assert
			Assert.Equal(-9190f, r1, 5);
			Assert.Equal(-9190f, r2, 5);
		}

		[Fact(DisplayName = "CrossProduct (1,4,-5)(-2,2,1) => (14,9,10)")]
		public void CrossProductTest1() {
			// Arrange
			Vec3 a = new Vec3(1f, 4f, -5f);
			Vec3 b = new Vec3(-2f, 2f, 1f);
			// Act
			var r = Vec3.CrossProduct(a, b);
			// Assert
			Assert.Equal(14f, r.X, 5);
			Assert.Equal(9f, r.Y, 5);
			Assert.Equal(10f, r.Z, 5);
		}

		[Fact(DisplayName = "CrossProduct (-100,4.23,-5)(-2,0,0) => (0,10,8.46)")]
		public void CrossProductTest2() {
			// Arrange
			Vec3 a = new Vec3(-100f, 4.23f, -5f);
			Vec3 b = new Vec3(-2f, 0f, 0f);
			// Act
			var r = Vec3.CrossProduct(a, b);
			// Assert
			Assert.Equal(0f, r.X, 5);
			Assert.Equal(10f, r.Y, 5);
			Assert.Equal(8.46f, r.Z, 5);
		}

		[Fact(DisplayName = "CrossProduct (1,1,1)(2,2,2) => (0,0,0)")]
		public void CrossProductTest3() {
			// Arrange
			Vec3 a = new Vec3(1f, 1f, 1f);
			Vec3 b = new Vec3(2f, 2f, 2f);
			// Act
			var r = Vec3.CrossProduct(a, b);
			// Assert
			Assert.Equal(0f, r.X, 5);
			Assert.Equal(0f, r.Y, 5);
			Assert.Equal(0f, r.Z, 5);
		}

		[Fact(DisplayName = "Magnitude (0,0,0) => 0f")]
		public void MagnitudeTest0() {
			// Arrange & Act
			var v = new Vec3(0, 0, 0);
			// Assert
			Assert.Equal(0f, v.Magnitude());
		}

		[Fact(DisplayName = "Magnitude (1,17,-3) => 17.29162f")]
		public void MagnitudeTest1() {
			// Arrange && Act
			var v = new Vec3(1, 17, -3);
			// Assert
			Assert.Equal(17.29162f, v.Magnitude(), 5);
		}

		[Fact(DisplayName = "Magnitude (398554f, -2414f, 0f) => 398561.3125f")]
		public void MagnitudeTest2() {
			// Arrange && Act
			var v = new Vec3(398554f, -2414f, 0f);
			// Assert
			Assert.Equal(398561.3125f, v.Magnitude(), 5);
		}

		[Fact(DisplayName = "Normalize (7f, 7f, 7f) => (0.5773503f, 0.5773503f, 0.5773503f) => Magnitude 1f")]
		public void NormalizeTest0() {
			// Arrange && Act
			var v = new Vec3(7f, 7f, 7f).Normalize();
			// Assert
			Assert.Equal(0.5773503f, v.X, 5);
			Assert.Equal(0.5773503f, v.Y, 5);
			Assert.Equal(0.5773503f, v.Z, 5);
			Assert.Equal(1f, v.Magnitude(), 5);
		}

		[Fact(DisplayName = "Normalize (9f, -5f, 0f) => (0.8741572f, -0.4856429f, 0f) => Magnitude 1f")]
		public void NormalizeTest1() {
			// Arrange && Act
			var v = new Vec3(9f, -5f, 0f).Normalize();
			// Assert
			Assert.Equal(0.8741572f, v.X, 5);
			Assert.Equal(-0.4856429f, v.Y, 5);
			Assert.Equal(0f, v.Z);
			Assert.Equal(1f, v.Magnitude(), 5);
		}

		[Fact(DisplayName = "Normalize (0f, 0f, 0f) => (0f, 0f, 0f) => Magnitude 0f")]
		public void NormalizeTest2() {
			// Arrange && Act
			var v = new Vec3(0f, 0f, 0f).Normalize();
			// Assert
			Assert.Equal(0f, v.X);
			Assert.Equal(0f, v.Y);
			Assert.Equal(0f, v.Z);
			Assert.Equal(0f, v.Magnitude());
		}

		[Fact(DisplayName = "Vector a'.Equals(Vector b') => True")]
		public void EqualsTest0() {
			// Arrange
			Vec3 a1 = new Vec3(1f, 2f, 3f);
			Vec3 a2 = new Vec3(1f, 2f, 3f);

			Vec3 b1 = new Vec3(0f, -2f, 3f);
			Vec3 b2 = new Vec3(0f, -2f, 3f);

			Vec3 c1 = new Vec3(90001f, 2.1f, 39.99f);
			Vec3 c2 = new Vec3(90001f, 2.1f, 39.99f);

			Vec3 d1 = new Vec3(0f, 0f, 0f);
			Vec3 d2 = new Vec3(0f, 0f, 0f);

			Vec3 e1 = new Vec3(float.MaxValue, float.MinValue, float.MaxValue);
			Vec3 e2 = new Vec3(float.MaxValue, float.MinValue, float.MaxValue);

			Vec3 f1 = new Vec3(90f, 4.000f, 2.2f);
			Vec3 f2 = new Vec3(90.0f, 4f, 2.20f);
			
			// Act
			var a = a1.Equals(a2);
			var b = b1.Equals(b2);
			var c = c1.Equals(c2);
			var d = d1.Equals(d2);
			var e = e1.Equals(e2);
			var f = f1.Equals(f2);

			// Assert
			Assert.True(a);
			Assert.True(b);
			Assert.True(c);
			Assert.True(d);
			Assert.True(e);
			Assert.True(f);
		}

		[Fact(DisplayName = "Vector a'.Equals(Vector b') => False")]
		public void EqualsTest1() {
			// Arrange
			Vec3 a1 = new Vec3(1f, 0f, 3f);
			Vec3 a2 = new Vec3(1f, 2f, 3f);

			Vec3 b1 = new Vec3(0f, 2f, 3f);
			Vec3 b2 = new Vec3(0f, -2f, 3f);

			Vec3 c1 = new Vec3(90002f, 2.1f, float.NegativeInfinity);
			Vec3 c2 = new Vec3(90002f, 2.1f, float.NegativeInfinity);

			Vec3 d1 = new Vec3(float.PositiveInfinity, 0f, 0f);
			Vec3 d2 = new Vec3(float.PositiveInfinity, 0f, 0f);

			Vec3 e1 = new Vec3(float.MinValue, float.MaxValue, float.MaxValue);
			Vec3 e2 = new Vec3(float.MaxValue, float.MinValue, float.MaxValue);

			Vec3 f1 = new Vec3(float.NaN, 4f, 2.2f);
			Vec3 f2 = new Vec3(float.NaN, 4f, 2.20f);

			// Act
			var a = a1.Equals(a2);
			var b = b1.Equals(b2);
			var c = c1.Equals(c2);
			var d = d1.Equals(d2);
			var e = e1.Equals(e2);
			var f = f1.Equals(f2);

			// Assert
			Assert.False(a);
			Assert.False(b);
			Assert.False(c);
			Assert.False(d);
			Assert.False(e);
			Assert.False(f);
		}
	}
}