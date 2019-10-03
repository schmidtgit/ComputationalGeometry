using MeshViewer.View;
using PolygonTriangulation.Model;
using PolygonTriangulation.Polygon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MeshViewer
{
	public static class MeshView
	{
		public static void ShowLogs(params Log[] logs)
		{
            App.Instance.ShowLogs(logs);
		}

		public static void ShowLogs(params Tuple<Log, string>[] logs) {
            App.Instance.ShowLogs(logs);
        }
        
		public static void ShowMeshes(params IPolygon[] meshes) {
            App.Instance.ShowMeshes(meshes);
		}

		public static void ShowMeshes(params Tuple<IPolygon, string>[] meshes) {
            App.Instance.ShowMeshes(meshes);
        }
	}
}
