using PolygonTriangulation.Model;
using PolygonTriangulation.Polygon;
using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;

namespace IOhandler {
	public static class ObjExporter {
		/// <summary>
		/// Exports an object to a .obj file at the specified location.
		/// </summary>
		/// <remarks>
		/// Details regarding elapsed time are optional.
		/// </remarks>
		/// <param name="filename">The desired filepath of the exported file.</param>
		/// <param name="obj">The object to export.</param>
		/// <param name="ElapsedMilliseconds">The number of milliseconds it took to create the model.</param>
		/// <returns>true if the object was successfully exported.</returns>
		public static bool Export(string filename, IPolygon obj, long ElapsedMilliseconds = 0) {
			// Check for invalid formatting of triangles
			int[] t = obj.Triangles.ToArray();
			if (t.Count() % 3 != 0)
				return false;

			// Required to ensure correct formatting in file
			var cCulture = (CultureInfo) Thread.CurrentThread.CurrentCulture.Clone();
			var oldCulture = (CultureInfo) Thread.CurrentThread.CurrentCulture.Clone();
			cCulture.NumberFormat.NumberDecimalSeparator = ".";
			Thread.CurrentThread.CurrentCulture = cCulture;

			filename = filename.EndsWith(".obj") ? filename : filename += ".obj";
			using(StreamWriter _file = new StreamWriter(filename)) {
				// .OBJ Header
				_file.WriteLine("# Computational Geometry - Polygon Triangulation of Implicit Defined Objects");
				_file.WriteLine("# - by Christian Lebeda and Jens Schmidt, IT University of Copenhagen");
				if(ElapsedMilliseconds != 0) {
					_file.WriteLine($"# Triangulation of this object took {ElapsedMilliseconds}ms");
				}
				_file.WriteLine($"# Generated at {DateTime.Now}");
				_file.WriteLine("g default");
				_file.WriteLine($"# Vertices {obj.VertexCount}");
				_file.WriteLine($"# Faces {obj.TriangleCount}");

				// All Vertices
				foreach(Vec3 v in obj.Vertices) {
					_file.WriteLine($"v  {v.X}  {v.Y}  {v.Z}");
				}

				// All Faces
				int i = 0;
				while(i < t.Count() - 2) {
					// .OBJ starts at index 1
					_file.WriteLine($"f  {t[i++] + 1}  {t[i++] + 1}  {t[i++] + 1}");
				}
			}

			// Reset culture
			Thread.CurrentThread.CurrentCulture = oldCulture;
			return true;
		}
	}
}
