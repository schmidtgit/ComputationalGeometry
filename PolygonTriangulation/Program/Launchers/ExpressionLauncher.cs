using PolygonTriangulation.ImplicitObjects;
using PolygonTriangulation.Model;
using PolygonTriangulation.Polygon;
using PolygonTriangulation.Triangulation;
using SimprExpression;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Program.Launchers {
	/// <summary>
	/// Triangulates based on arithmic expression from the user, using SimplrExpression
	/// </summary>
	class ExpressionLauncher : BaseLauncher, IProgramLauncher {
		public bool Run() {
			// Start up!
			Program.PrintTitle("EXPRESSION TRIANGULATION");
			Console.WriteLine("This launcher run all the following algorithms:\n");
			ITriangulator[] t = new ITriangulator[] { Program.UserInstantiation<ITriangulator>("triangulation method") };

			// Equation
			Console.WriteLine("Please enter equation containing x, y and z:");
            var values = new HashSet<char>(new[] { 'x', 'y', 'z' });
            IExpression expr = null;
            bool parsed = false;
            while(!parsed) {
                try {
                    expr = ExpressionParser.Parse(Console.ReadLine());
                    if(expr.CanCompute(values)) {
                        parsed = true;
                    } else {
                        Console.WriteLine("\nInvalid input! The only valid variables are x, y and z");
                    }
                } catch(ArgumentException) {
                    Console.WriteLine("\nInvalid input! Please enter a valid expression");
                }
            }
            
            Console.WriteLine("Understood that as:");
			Console.WriteLine(expr.ToString());
			SDF[] sdf = new SDF[] { new ParsedExpression(expr) };
			
			// Debug?
			var debugView = AskUser("Enable step visualizer triangulation?");

            // Triangulate!
            Tuple<Log, IPolygon, string, long>[] result;
            if (debugView) {
                result = TriangulateWithLog(t, sdf);
            } else {
                result = Triangulate(t, sdf);
            }

            // Show result!
            if (debugView) {
				if (!LaunchLogView(result))
					Export(result);
			} else {
				if (!LaunchView(result))
					Export(result);
			}

			return !AskUser("Restart the application?");
		}
	}
}
