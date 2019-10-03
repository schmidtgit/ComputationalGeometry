using PolygonTriangulation.ImplicitObjects;
using PolygonTriangulation.Model;
using PolygonTriangulation.Polygon;
using PolygonTriangulation.Triangulation;
using Program.RuntimeImplicit;
using System;

namespace Program.Launchers
{
	/// <summary>
	/// Default launcher that run N triangulations
	/// </summary>
	public class DefaultLauncher : BaseLauncher, IProgramLauncher {
		public bool Run() {
			Program.PrintTitle("SETUP - DEFAULT LAUNCHER");
			
			// Debug?
			var debugView = AskUser("Enable step visualizer triangulation?");

			// # of triangulations?
			Console.WriteLine("\nNumber of simultaneous triangulations?");
			Console.Write("> ");
			var n = 0;
            while(!int.TryParse(Console.ReadLine(), out n)) {
                Console.WriteLine("Invalid input! Please enter a number");
                Console.Write("\n> ");
            }

			// Setup...
			var triangulator = new ITriangulator[n];
			var sdf = new SDF[n];
			for (int i = 0; i < n; i++) {
				Program.PrintTitle($"PERFORM SETUP {i + 1} of {n}");
				triangulator[i] = Program.UserInstantiation<ITriangulator>("triangulation method");
				sdf[i] = Program.UserInstantiation<IRuntimeSDF>("signed distance function").Instance();
			}


			// Triangulate!
			Tuple<Log, IPolygon, string, long>[] result;
			if (debugView) {
				result = TriangulateWithLog(triangulator, sdf);
			} else {
				result = Triangulate(triangulator, sdf);
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
