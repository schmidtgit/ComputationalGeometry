using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IOhandler {
	/// <summary>
	/// Simple path chooser for finding valid .obj and .ply files.
	/// </summary>
	public class PathChooser {

		/// <summary>
		/// Let the user choose a valid save location for .obj or .ply files.
		/// </summary>
		/// <returns>Absolute path. Null if no path was choosen.</returns>
		public static string FindSaveLocation() {
			SaveFileDialog saveFileDialog1 = new SaveFileDialog();

			saveFileDialog1.Filter = "Object File Format (.obj)|*.obj|Polygon File Format (.ply)|*.ply";
			saveFileDialog1.FilterIndex = 1;
			saveFileDialog1.RestoreDirectory = true;

			if(saveFileDialog1.ShowDialog() == DialogResult.OK) {
				return saveFileDialog1.FileName;
			}

			return null;
		}

		/// <summary>
		/// Let the user choose a valid load location for .obj.
		/// </summary>
		/// <returns>Absolute path. Null if no path was choosen.</returns>
		public static string FindLoadLocation() {
			OpenFileDialog openFileDialog1 = new OpenFileDialog();

			openFileDialog1.Filter = "Object File Format (.obj)|*.obj";
			openFileDialog1.FilterIndex = 1;
			openFileDialog1.RestoreDirectory = true;

			if(openFileDialog1.ShowDialog() == DialogResult.OK) {
				return openFileDialog1.FileName;
			}

			return null;
		}
	}
}
