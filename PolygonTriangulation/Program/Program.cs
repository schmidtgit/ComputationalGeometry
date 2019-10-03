using PolygonTriangulation.ImplicitObjects;
using PolygonTriangulation.Polygon;
using Program.Launchers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using IOhandler;
using Program.RuntimeImplicit;

namespace Program
{
	public class Program
	{
		public static string version { get; } = Assembly.GetExecutingAssembly().GetName().Version.ToString();

		/// <summary>
		/// Launches everything!
		/// </summary>
		/// <param name="args">Ignored.</param>
		[STAThread]
		static void Main(string[] args)
		{
			do {
				Console.Title = "Computational Geometry - Polygon Triangulation of Implicit Defined Objects";
				PrintTitle("WELCOME");
			} while (!UserInstantiation<IProgramLauncher>("program launcher").Run());

			PrintTitle("PRESS ANY KEY TO TERMINATE");
			Console.ReadKey();
		}

		/// <summary>
		/// Prints a title for the program
		/// </summary>
		/// <param name="s">The title to print</param>
		/// <param name="clear">True will clear page before printing title</param>
		public static void PrintTitle(string s, bool clear = true)
		{
			if (clear) {
				Console.Clear();
				Console.WriteLine("Computational Geometry - Polygon Triangulation of Implicit Defined Objects");
				Console.WriteLine("Currently running v" + version + "\n");
				Console.WriteLine($"##### {s} #####");
			} else {
				Console.WriteLine($"\n##### {s} #####");
			}
		}

		/// <summary>
		/// Ask the user to choose a valid implementation of T.
		/// </summary>
		/// <typeparam name="T">Interface to find an implementation of.</typeparam>
		/// <param name="s">Please choose a 's', default is 'class'.</param>
		/// <returns></returns>
		public static T UserInstantiation<T>(string s = "class") where T : class
		{
			T result = default(T);
			var valid = AvailableImplementations<T>().ToArray();

			if (valid.Length == 0)
				throw new NotSupportedException();

			while (result == null)
			{
				int index = 0;
				Console.WriteLine("Please choose a " + s + ":\n");
				foreach (string name in valid)
				{
					Console.WriteLine($"\t {index} | {name.Beautify()}");
					index++;
				}
				Console.Write("> ");
				var input = Console.ReadLine();
				Console.WriteLine();

				if (Int32.TryParse(input, out index) && index < valid.Length)
					result = InstantiateImplementation<T>(valid[index]);
			}

			return result is IRuntimeSDF ? UserSetupSDF(result as IRuntimeSDF) as T : result; // Check for special case!
		}

		/// <summary>
		/// Exports all given meshes.
		/// </summary>
		/// <param name="mesh">Meshes to export.</param>
		/// <param name="elapsedTime">Elapsed creation time for each mesh, expected to match mesh in length.</param>
		public static void Export(IPolygon[] mesh, long[] elapsedTime)
		{
			int n = mesh.Count();
			var paths = new string[n];
			var exportSuccess = new bool[n];
			PrintTitle("EXPORTING");
			Console.WriteLine();
			for (int i = 0; i < n; i++)
			{
				Console.WriteLine($"Choose location for export {i + 1} of {n}\n");
				paths[i] = PathChooser.FindSaveLocation();
			}

			Parallel.For(0, n, i => {
				if (paths[i] != null) 
					exportSuccess[i] = paths[i].EndsWith("obj") ? ObjExporter.Export(paths[i], mesh[i], elapsedTime[i]) : PlyExporter.Export(paths[i], mesh[i], elapsedTime[i]);
			});

			for (int i = 0; i < n; i++) {
				if (paths[i] == null) {
					Console.WriteLine($"\nExporting of file {i + 1} of {n} failed!");
					continue;
				}

				var s = paths[i].Split('\\');
				Console.WriteLine($"\nOpen generated '{s[s.Length - 1]}' file? Y/N");
				Console.Write("> ");
				if (Console.ReadLine().ToLower().Contains("y"))
					Process.Start(paths[i]);
			}
		}

		/// <summary>
		/// Returns a IEnumerable string collection of all available implementations of type T.
		/// that have an empty constructor.
		/// </summary>
		/// <typeparam name="T">Interface expected.</typeparam>
		/// <returns></returns>
		public static IEnumerable<string> AvailableImplementations<T>() where T : class
		{
			var typeInfo = typeof(T).GetTypeInfo();
            return typeInfo.Assembly
                    .GetTypes()
                    .Where(x => x != typeof(T))
                    .Where(x => typeInfo.IsAssignableFrom(x))
                    .Where(x => x.GetConstructor(Type.EmptyTypes) != null)
                    .Where(x => x.GetCustomAttributes(typeof(ObsoleteAttribute), false).Length == 0)
                    .OrderBy(x => x.Name)
					.Select(x => x.Name);
		}

		/// <summary>
		/// Instantiate an object of the class with name S, that implements T.
		/// Only works if an empty constructor is available!
		/// </summary>
		/// <typeparam name="T">Interface that object implements.</typeparam>
		/// <param name="s">Class name.</param>
		/// <returns></returns>
		public static T InstantiateImplementation<T>(string s) where T : class
		{
			// Check to prevent invalid operations
			if (!AvailableImplementations<T>().Contains(s))
				return default(T);

			// Find the type
			var typeInfo = typeof(T).GetTypeInfo();
			var type = typeInfo.Assembly
					.GetTypes()
					.Where(x => x.Name.Equals(s, StringComparison.OrdinalIgnoreCase))
					.FirstOrDefault();

			// Instantiate
			return type == null ? default(T) : Activator.CreateInstance(type) as T;
		}

		/// <summary>
		/// Handles the special case of a user instantiated SDF, since it's quite common.
		/// </summary>
		/// <param name="obj">The RuntimeSDF to setup.</param>
		/// <returns></returns>
		private static IRuntimeSDF UserSetupSDF(IRuntimeSDF obj)
		{
			var parems = obj.Parameters();
			var f = new float[parems.Length];
			for (int i = 0; i < parems.Length; i++) {
				Console.WriteLine($"\n[{obj.GetType().Name.Beautify()}] Please specify { parems[i] }:");
				Console.Write("> ");
				var input = Console.ReadLine();
				if (!float.TryParse(input, out f[i]))
					i--; // Retry if it failed
			}
			obj.Setup(f);
			return obj;
		}
	}
}
