using PolygonTriangulation.Model;
using PolygonTriangulation.Polygon;
using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Media;

namespace IOhandler {
	public static class PlyExporter {
		/// <summary>
		/// Exports an object to a .ply file at the specified location.
		/// </summary>
		/// <remarks>
		/// Details regarding elapsed time are optional.
		/// The object can optionally be colored using a coloring function.
		/// </remarks>
		/// <param name="filename">The desired filepath of the new file.</param>
		/// <param name="obj">The object to export.</param>
		/// <param name="coloring">A coloring function mapping a Vec3 to a desired color. If this is null, no coloring will be added to the ply file.</param>
		/// <returns>true if the object was successfully exported.</returns>
		public static bool Export(string filename, IPolygon obj, Func<Vec3, Color> coloring) {
			return Export(filename, obj, 0, coloring);
		}

		/// <summary>
		/// Exports an object to a .ply file at the specified location.
		/// </summary>
		/// <remarks>
		/// Details regarding elapsed time are optional.
		/// The object can optionally be colored using a coloring function.
		/// </remarks>
		/// <param name="filename">The desired filepath of the new file.</param>
		/// <param name="obj">The object to export.</param>
		/// <param name="ElapsedMilliseconds">The number of milliseconds it took to create the model.</param>
		/// <param name="coloring">A coloring function mapping a Vec3 to a desired color. If this is null, no coloring will be added to the ply file.</param>
		/// <returns>true if the object was successfully exported.</returns>
		public static bool Export(string filename, IPolygon obj, long ElapsedMilliseconds = 0, Func<Vec3, Color> coloring = null) {
			// Check for invalid formatting of triangles
			int[] t = obj.Triangles.ToArray();
			if (t.Count() % 3 != 0)
				return false;

			// Required to ensure correct formatting in file
			var cCulture = (CultureInfo) Thread.CurrentThread.CurrentCulture.Clone();
			var oldCulture = (CultureInfo) Thread.CurrentThread.CurrentCulture.Clone();
			cCulture.NumberFormat.NumberDecimalSeparator = ".";
			Thread.CurrentThread.CurrentCulture = cCulture;

			filename = filename.EndsWith(".ply") ? filename : filename += ".ply";
			using(BinaryWriter _file = new BinaryWriter(new FileStream(filename, FileMode.Create), Encoding.ASCII)) {
				// .PLY Header
				_file.Write(StringConvert("ply\n"));
				_file.Write(StringConvert("format ascii 1.0\n"));
				_file.Write(StringConvert($"comment Computational Geometry - Polygon Triangulation of Implicit Defined Objects\n"));
				_file.Write(StringConvert($"comment - by Christian Lebeda and Jens Schmidt, IT University of Copenhagen\n"));
				if(ElapsedMilliseconds != 0) {
					_file.Write(StringConvert($"comment Triangulation of this object took {ElapsedMilliseconds}ms\n"));
				}
				_file.Write(StringConvert($"comment generated at {DateTime.Now}\n"));
				_file.Write(StringConvert($"element vertex {obj.VertexCount}\n"));
				_file.Write(StringConvert("property float32 x\n"));
				_file.Write(StringConvert("property float32 y\n"));
				_file.Write(StringConvert("property float32 z\n"));
				if(coloring != null) {
					_file.Write(StringConvert("property uchar red\n"));
					_file.Write(StringConvert("property uchar green\n"));
					_file.Write(StringConvert("property uchar blue\n"));
				}
				_file.Write(StringConvert($"element face {obj.TriangleCount}\n"));
				_file.Write(StringConvert("property list uint8 int32 vertex_indices\n"));
				_file.Write(StringConvert("end_header\n"));

				// All Vertices
				foreach(Vec3 v in obj.Vertices) {
					string vert = $"{v.X} {v.Y} {v.Z}";
					if(coloring != null) {
						var c = coloring(v);
						vert += $" {c.R} {c.G} {c.B}";
					}
					_file.Write(StringConvert(vert + "\n"));
				}

				// All Faces
				int i = 0;
				while(i < t.Count() - 2) {
					_file.Write(StringConvert($"3 {t[i++]} {t[i++]} {t[i++]}\n"));
				}
			}

			// Reset culture
			Thread.CurrentThread.CurrentCulture = oldCulture;
			return true;
		}

		/// <summary>
		/// Converts a string to byte[].
		/// </summary>
		/// <param name="theString">String to convert.</param>
		private static byte[] StringConvert(string theString) {
			return Encoding.ASCII.GetBytes(theString);
		}
	}
}
