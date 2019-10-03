using PolygonTriangulation.Builder;
using PolygonTriangulation.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Program.Import {
	[Obsolete]
	public static class ObjParser {
		public static bool Import(string filename, IPolygonBuilder builder) {
			var verts = new List<Vec3>();
			using (StreamReader _stream = new StreamReader(filename)) {
				while (!_stream.EndOfStream) {
					string[] subString = _stream.ReadLine().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
					if (subString.Count() == 0)
						continue;

					switch (subString[0]) {
						case "v":
							float x, y, z;
							if (float.TryParse(subString[1], out x) && float.TryParse(subString[2], out y) && float.TryParse(subString[3], out z))
								verts.Add(new Vec3(x, y, z));
							break;
						case "f":
							if (subString.Count() > 5)
								throw new NotImplementedException("Only triangles and squares supported");
							int f1, f2, f3, f4;
							if (int.TryParse(subString[1].Split('/')[0], out f1) && int.TryParse(subString[2].Split('/')[0], out f2) && int.TryParse(subString[3].Split('/')[0], out f3))
								if (subString.Length == 5 && int.TryParse(subString[4].Split('/')[0], out f4)) {
									builder.Append(verts[f1 - 1], verts[f2 - 1], verts[f3 - 1]);
									builder.Append(verts[f3 - 1], verts[f4 - 1], verts[f1 - 1]);
								} else
									builder.Append(verts[f1 - 1], verts[f2 - 1], verts[f3 - 1]);
							break;
					}
				}
			}
			return true;
		}
	}
}
