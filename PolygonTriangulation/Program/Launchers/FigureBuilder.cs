using PolygonTriangulation.ImplicitObjects;
using PolygonTriangulation.Model;
using PolygonTriangulation.Polygon;
using PolygonTriangulation.Triangulation;
using Program.RuntimeImplicit;
using Program.RuntimeOperator;
using System;

namespace Program.Launchers {
	public class FigureBuilder: BaseLauncher, IProgramLauncher {
		public bool Run() {
			Program.PrintTitle("SETUP - FIGURE BUILDER");
			Console.WriteLine("This launcher let's you create a SDF operator tree");
			SDF figure = userSDF();


			Program.PrintTitle("FIGURE BUILDER");
			Console.WriteLine("You choose to create:");
			Console.WriteLine(figure.ToString());
			Console.WriteLine("Nice choice!");

			// # of triangulations?
			Console.WriteLine("\nNumber of simultaneous triangulations?");
			Console.Write("> ");
			var n = 1;
			int.TryParse(Console.ReadLine(), out n);

			// Setup...
			var triangulator = new ITriangulator[n];
			var sdf = new SDF[n];
			for (int i = 0; i < n; i++) {
				Program.PrintTitle($"PERFORM SETUP {i + 1} of {n}");
				triangulator[i] = Program.UserInstantiation<ITriangulator>("triangulation method");
				sdf[i] = figure;
			}

			// Debug?
			var debugView = AskUser("Enable step visualizer triangulation?");

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

		SDF userSDF() {
			if (!AskUser("Choose an SDF operator?"))
				return Program.UserInstantiation<IRuntimeSDF>("signed distance function").Instance();

			var obj = Program.UserInstantiation<ISDFOperator>("operator");

			// Set all float values
			var parems = obj.FloatParameters();
			var f = new float[parems.Length];
			for (int i = 0; i < parems.Length; i++) {
				Console.WriteLine($"\n[{obj.GetType().Name.Beautify()}] Please specify { parems[i] }:");
				Console.Write("> ");
				var input = Console.ReadLine();
				if (!float.TryParse(input, out f[i]))
					i--; // Retry if it failed
			}

			// Set all sdf values
			var SDFparems = obj.SDFParameters();
			var s = new SDF[SDFparems.Length];
			for (int i = 0; i < SDFparems.Length; i++) {
				Console.WriteLine($"\n[{obj.GetType().Name.Beautify()}] Please specify { SDFparems[i] }:");
				Console.Write("> ");
				s[i] = userSDF();
			}
			obj.Setup(f, s);
			Console.WriteLine($"Done with {obj.ToString()}");

			SDF result = null;
			if (obj is IRuntimeSDF)
				result = (obj as IRuntimeSDF).Instance();

			if (obj is ISDFOperator)
				result = (obj as ISDFOperator).Instance();

			return result;
		}
	}
}
