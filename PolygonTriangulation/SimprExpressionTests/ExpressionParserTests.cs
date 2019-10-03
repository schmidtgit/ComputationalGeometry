using System;
using System.Collections.Generic;
using Xunit;

namespace SimprExpression.Tests {
	public class ExpressionParserTests {
		[Fact(DisplayName = "0.0051 = 0.0051")]
		public void Test00() {
			//Arrange
			var expr = ExpressionParser.Parse("0.0051");

			//Act
			var result = expr.Compute(null);

			//Assert
			Assert.Equal(0.0051, result, 5);
		}

		[Fact(DisplayName = "5+5 = 10")]
		public void Test01() {
			//Arrange
			var expr = ExpressionParser.Parse("5+5");

			//Act
			var result = expr.Compute(null);

			//Assert
			Assert.Equal(10, result, 5);
		}

		[Fact(DisplayName = "7-5 = 2")]
		public void Test2() {
			//Arrange
			var expr = ExpressionParser.Parse("7-5");

			//Act
			var result = expr.Compute(null);

			//Assert
			Assert.Equal(2, result, 5);
		}

		[Fact(DisplayName = "9*2 = 18")]
		public void Test03() {
			//Arrange
			var expr = ExpressionParser.Parse("9*2");

			//Act
			var result = expr.Compute(null);

			//Assert
			Assert.Equal(18, result, 5);
		}

		[Fact(DisplayName = "sqrt(25) = 5")]
		public void Test04() {
			//Arrange
			var expr = ExpressionParser.Parse("sqrt(25)");

			//Act
			var result = expr.Compute(null);

			//Assert
			Assert.Equal(5, result, 5);
		}

		[Fact(DisplayName = "9^3 = 729")]
		public void Test05() {
			//Arrange
			var expr = ExpressionParser.Parse("9^3");

			//Act
			var result = expr.Compute(null);

			//Assert
			Assert.Equal(729, result, 5);
		}

		[Fact(DisplayName = "sin(25) = -0.13235175009")]
		public void Test06() {
			//Arrange
			var expr = ExpressionParser.Parse("sin(25)");

			//Act
			var result = expr.Compute(null);

			//Assert
			Assert.Equal(-0.13235175009, result, 5);
		}

		[Fact(DisplayName = "cos(25) = 0.99120281186")]
		public void Test07() {
			//Arrange
			var expr = ExpressionParser.Parse("cos(25)");

			//Act
			var result = expr.Compute(null);

			//Assert
			Assert.Equal(0.99120281186, result, 5);
		}

		[Fact(DisplayName = "tan(25) = -0.13352640702")]
		public void Test08() {
			//Arrange
			var expr = ExpressionParser.Parse("tan(25)");

			//Act
			var result = expr.Compute(null);

			//Assert
			Assert.Equal(-0.13352640702, result, 5);
		}

		[Fact(DisplayName = "5*(5+2) = 35")]
		public void Test09() {
			//Arrange
			var expr = ExpressionParser.Parse("5*(5+2)");

			//Act
			var result = expr.Compute(null);

			//Assert
			Assert.Equal(35, result, 5);
		}

		[Fact(DisplayName = "-100 = -100")]
		public void Test10() {
			//Arrange
			var expr = ExpressionParser.Parse("-100");

			//Act
			var result = expr.Compute(null);

			//Assert
			Assert.Equal(-100, result, 5);
		}

		[Fact(DisplayName = "(a*b)^c = 18399.744 where a = 11, b = 2.4, c = 3")]
		public void Test11() {
			//Arrange
			var vars = new Dictionary<char, double>();
			vars.Add('a', 11);
			vars.Add('b', 2.4);
			vars.Add('c', 3);
			var expr = ExpressionParser.Parse("(a*b)^c");

			//Act
			var result = expr.Compute(vars);

			//Assert
			Assert.Equal(18399.744, result, 5);
		}

		[Fact(DisplayName = "(sqrt(a)*sin(b)+99)-2500+4*q = 670.161753505 where a = 9, b = 6, q = 768")]
		public void Test12() {
			//Arrange
			var vars = new Dictionary<char, double>();
			vars.Add('a', 9);
			vars.Add('b', 6);
			vars.Add('q', 768);
			var expr = ExpressionParser.Parse("(sqrt(a)*sin(b)+99)-2500+4*q");

			//Act
			var result = expr.Compute(vars);

			//Assert
			Assert.Equal(670.161753505, result, 5);
		}

        [Fact(DisplayName = "10/-2 = -5")]
        public void Test13() {
            //Arrange
            var expr = ExpressionParser.Parse("10/-2");

            //Act
            var result = expr.Compute(null);

            //Assert
            Assert.Equal(-5, result, 5);
        }

        [Fact(DisplayName = "10^2+5 = 105")]
        public void Test14() {
            //Arrange
            var expr = ExpressionParser.Parse("10^2+5");

            //Act
            var result = expr.Compute(null);

            //Assert
            Assert.Equal(105, result, 5);
        }

        [Fact(DisplayName = "10^(2+5) = 10000000")]
        public void Test15() {
            //Arrange
            var expr = ExpressionParser.Parse("10^(2+5)");

            //Act
            var result = expr.Compute(null);

            //Assert
            Assert.Equal(10000000, result, 5);
        }

        [Fact(DisplayName = "10/2*5 = 25")]
        public void Test16() {
            //Arrange
            var expr = ExpressionParser.Parse("10/2*5");

            //Act
            var result = expr.Compute(null);

            //Assert
            Assert.Equal(25, result, 5);
        }

        [Fact(DisplayName = "a(5+b)=30 where a = 2 and b = 10")]
        public void Test17() {
            //Arrange
            var vars = new Dictionary<char, double>();
            vars.Add('a', 2);
            vars.Add('b', 10);
            var expr = ExpressionParser.Parse("a(5+b)");

            //Act
            var result = expr.Compute(vars);

            //Assert
            Assert.Equal(30, result, 5);
        }

        [Fact(DisplayName = "sqrt(-x)=NaN when x = 5")]
        public void Test18() {
            //Arrange
            var vars = new Dictionary<char, double>();
            vars.Add('x', 5);
            var expr = ExpressionParser.Parse("sqrt(-x)");

            //Act
            var result = expr.Compute(vars);

            //Assert
            Assert.Equal(float.NaN, result);
        }
        
        [Fact(DisplayName = "4*(4+5 throws ArgumentException")]
        public void Test19() {
            //Act and assert
            Assert.Throws<ArgumentException>(() => ExpressionParser.Parse("4*(4+5"));
        }

        [Fact(DisplayName = "4+5) throws ArgumentException")]
        public void Test20() {
            //Act and assert
            Assert.Throws<ArgumentException>(() => ExpressionParser.Parse("4+5)"));
        }

        [Fact(DisplayName = "4**5 throws ArgumentException")]
        public void Test21() {
            //Act and assert
            Assert.Throws<ArgumentException>(() => ExpressionParser.Parse("4**4"));
        }


        [Fact(DisplayName = "42*() throws ArgumentException")]
        public void Test22() {
            //Act and assert
            Assert.Throws<ArgumentException>(() => ExpressionParser.Parse("42*()"));
        }

        [Fact(DisplayName = "empty string throws ArgumentException")]
        public void Test23() {
            //Act and assert
            Assert.Throws<ArgumentException>(() => ExpressionParser.Parse(""));
        }

        [Fact(DisplayName = "4*a+b where b is missing throws KeyNotFoundException")]
        public void Test24() {
            //Arrange
            var vars = new Dictionary<char, double>();
            vars.Add('a', 5);
            var expr = ExpressionParser.Parse("4*a+b");

            //Act and assert
            Assert.Throws<KeyNotFoundException>(() => expr.Compute(vars));
        }
    }
}