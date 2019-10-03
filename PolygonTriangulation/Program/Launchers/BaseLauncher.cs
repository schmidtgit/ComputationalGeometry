using MeshViewer;
using PolygonTriangulation.Builder;
using PolygonTriangulation.ImplicitObjects;
using PolygonTriangulation.Model;
using PolygonTriangulation.Polygon;
using PolygonTriangulation.Triangulation;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Program.Launchers {
	/// <summary>
	/// Used as a template
	/// </summary>
	public abstract class BaseLauncher {
		protected bool AskUser(string s) {
			while (true) {
				Console.WriteLine($"\n{s} Y/N");
				Console.Write("> ");
				var input = Console.ReadKey().KeyChar;
				if (input == 'y' || input == 'Y')
					return true;
				if (input == 'n' || input == 'N')
					return false;
                Console.Write("\nInvalid input! Press Y for yes or N for no\n");
			}
		}

		protected Tuple<Log, IPolygon, string, long>[] Triangulate(ITriangulator[] triangulator, SDF[] obj) {
			Program.PrintTitle("TRIANGULATING...");
			var result = new Tuple<Log, IPolygon, string, long>[triangulator.Length];
			Parallel.For(0, triangulator.Length, i => {
				Stopwatch t = new Stopwatch();
				var builder = new ExportBuilder();
				t.Start();
				var mesh = triangulator[i].Run(obj[i], builder);
				t.Stop();
				var name = $"[{obj[i].ToString()}] {triangulator[i].ToString()}";
				var time = t.ElapsedMilliseconds;
				result[i] = Tuple.Create<Log, IPolygon, string, long>(null, mesh, name, time);
				Console.WriteLine($"\n{name} took {time}ms\nCreated {mesh.VertexCount} vertices and {mesh.TriangleCount} triangles");
			});
			Console.WriteLine();
			Console.WriteLine("PRESS ANY KEY TO CONTINUE...");
			Console.ReadKey();
			return result;
		}

		protected Tuple<Log, IPolygon, string, long>[] TriangulateWithLog(ITriangulator[] triangulator, SDF[] obj) {
			Program.PrintTitle("TRIANGULATING...");
			var result = new Tuple<Log, IPolygon, string, long>[triangulator.Length];
			Parallel.For(0, triangulator.Length, i => {
				Stopwatch t = new Stopwatch();
				GridLog builder = new GridLog();
				t.Start();
				var mesh = triangulator[i].Run(obj[i], builder);
				t.Stop();
				var name = $"[{obj[i].ToString()}] {triangulator[i].ToString()}";
				var time = t.ElapsedMilliseconds;
				result[i] = Tuple.Create((mesh as LogPolygon).GetLog(), mesh, name, time);
				Console.WriteLine($"\n{name} took {time}ms\nCreated {mesh.VertexCount} vertices and {mesh.TriangleCount} triangles");
			});
			Console.WriteLine("\nPRESS ANY KEY TO CONTINUE");
			Console.ReadKey();
			return result;
		}

		protected bool LaunchView(params Tuple<Log, IPolygon, string, long>[] tuples) {
			var result = new Tuple<IPolygon, string>[tuples.Length];
			for(int i = 0; i < tuples.Length; i++)
				result[i] = Tuple.Create(tuples[i].Item2, tuples[i].Item3);
			return LaunchView(result);
		}

		protected bool LaunchView(params Tuple<IPolygon, string>[] tuples) {
            if(App.Instance != null && System.Windows.Application.Current == null) return false;
            Program.PrintTitle("MESH VIEWER");
			if (AskUser("Show meshes?")) {
				MeshView.ShowMeshes(tuples);
				return true;
			}
			return false;	
		}

		protected bool LaunchLogView(params Tuple<Log, IPolygon, string, long>[] tuples) {
			var result = new Tuple<Log, string>[tuples.Length];
			for (int i = 0; i < tuples.Length; i++)
				result[i] = Tuple.Create(tuples[i].Item1, tuples[i].Item3);
			return LaunchLogView(result);
		}

		protected bool LaunchLogView(params Tuple<Log, string>[] tuples) {
            if(App.Instance != null && System.Windows.Application.Current == null) return false;
            Program.PrintTitle("STEP VISUALIZER");
			if (AskUser("Show algorithm step visualizers?")) {
				MeshView.ShowLogs(tuples);
				return true;
			} else {
				return false;
			}
		}

		protected bool Export(params Tuple<Log, IPolygon, string, long>[] tuples) {
			var result = new Tuple<Log, string>[tuples.Length];
			IPolygon[] mesh = new IPolygon[tuples.Length];
			long[] elapsedTime = new long[tuples.Length];
			for (int i = 0; i < tuples.Length; i++) {
				mesh[i] = tuples[i].Item2;
				elapsedTime[i] = tuples[i].Item4;
			}
			return Export(mesh, elapsedTime);
		}

		protected bool Export(IPolygon[] meshes, long[] elapsedTime) {
			Program.PrintTitle("EXPORTING");
			if (AskUser("Export all meshes?")) {
				Program.Export(meshes, elapsedTime);
				return true;
			} else {
				return false;
			}
		}
	}
}
