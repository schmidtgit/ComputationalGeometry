using PolygonTriangulation.Model;
using PolygonTriangulation.Polygon;
using PolygonTriangulation.Triangulation;
using Program.RuntimeImplicit;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Program.Launchers
{
	/// <summary>
	/// Runs all triangulations with the same SDF
	/// </summary>
	public class CompareAllLauncher : BaseLauncher, IProgramLauncher {
		public bool Run() {
			// Start up!
			Program.PrintTitle("COMPARE ALL LAUNCHER");
			Console.WriteLine("This launcher run all the following algorithms:\n");
			var all = new List<ITriangulator>();
			var index = 0;
			foreach (string name in Program.AvailableImplementations<ITriangulator>()) {
				Console.WriteLine($"\t {index} | {name.Beautify()}");
				all.Add(Program.InstantiateImplementation<ITriangulator>(name));
				index++;
			}

			// SDF
			var sdf = Program.UserInstantiation<IRuntimeSDF>().Instance();

			// Debug?
			var debugView = AskUser("Enable step visualizer triangulation?");

			// Triangulate!
			Tuple<Log, IPolygon, string, long>[] result;
			if (debugView) {
				result = TriangulateWithLog(all.ToArray(), Enumerable.Repeat(sdf, all.Count).ToArray());
			} else {
				result = Triangulate(all.ToArray(), Enumerable.Repeat(sdf, all.Count).ToArray());
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
