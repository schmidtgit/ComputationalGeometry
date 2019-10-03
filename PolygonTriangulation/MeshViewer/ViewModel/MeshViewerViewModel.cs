using HelixToolkit.Wpf;
using IOhandler;
using PolygonTriangulation.Model;
using PolygonTriangulation.Polygon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace MeshViewer.ViewModel {
	public class MeshViewerViewModel : ViewportViewModel{

        //Shared fields
        static PerspectiveCamera sharedCamera;

		//Backup
		Int32Collection _triangles;

        //Smoothing variables
        bool smooth = true;
        Point3DCollection smoothVerts;
        Int32Collection smoothTris;

        /// <summary>
        /// Initializes the ViewModel.
        /// </summary>
		public MeshViewerViewModel(HelixViewport3D viewport, IPolygon mesh, UIElement[] toggleButtons, UIElement[] cameraButtons, UIElement[] settingsButton, UIElement[] colorButtons) {
			_viewPort = viewport;
            tglBtns = toggleButtons;
            cameraBtns = cameraButtons;
            settingsBtns = settingsButton;
            colorBtns = colorButtons;
			InstantiateEmptyScene();
			AddEntireMesh(mesh.Vertices.ToArray(), mesh.Triangles.ToArray());
			SetCameraToDefault();
		}

		/// <summary>
		/// Changes between an individual camera and a shared one.
		/// </summary>
		public void ChangeCameraSetting() {
			if(_sharedCameraMode) {
				_viewPort.Camera = _camera;
			} else {
				if(sharedCamera == null) {
					sharedCamera = _camera.Clone();
				}
				_viewPort.Camera = sharedCamera;
			}
			_sharedCameraMode = !_sharedCameraMode;
		}

		private void InstantiateEmptyScene() {
			CreateEmptyCollection();
			AddDefaultLight();
			CreateEmptyMesh();
            CreateEmptyWireframe();
			CreateEmptyVerticesMesh();
		}

        /// <summary>
        /// Initializes the objects needed to show the mesh.
        /// </summary>
		private void CreateEmptyMesh() {
			var gm3 = new GeometryModel3D();
			_mainMesh = new MeshGeometry3D();
			gm3.Geometry = _mainMesh;
            _faceColor = Color.FromRgb(52, 152, 219);
            _faceMaterial = new SolidColorBrush(_faceColor);
            gm3.Material = new DiffuseMaterial(_faceMaterial);
			_collection.Add(gm3);
		}

        /// <summary>
        /// Shows the mesh in the scene.
        /// </summary>
		protected override void ShowMesh() {
			_mainMesh.TriangleIndices = _triangles.Clone();
		}

        /// <summary>
        /// Toggles whether or not vertices are shared between triangles or not allowing smoothing of normalvectors.
        /// </summary>
        /// <remarks>
        /// This method assumes that the original mesh is smooth.
        /// </remarks>
        public void ToggleSmooth() {
            if(smooth) {
                var newPos = new Point3DCollection();
                var newTris = new Int32Collection();
                foreach(var index in _mainMesh.TriangleIndices) {
                    newPos.Add(_mainMesh.Positions[index]);
                    newTris.Add(newTris.Count);
                }
                _mainMesh.Positions = newPos;
                _mainMesh.TriangleIndices = newTris;
            } else {
                _mainMesh.Positions = smoothVerts;
                _mainMesh.TriangleIndices = smoothTris;
            }
            smooth = !smooth;
        }

		/// <summary>
		/// Adds a mesh to the viewmodel
		/// </summary>
		/// <param name="vertices">The vertices used by the mesh</param>
		/// <param name="triangleIndices">The indices used to create the triangles</param>
		public void AddEntireMesh(Vec3[] vertices, int[] triangleIndices) {
			if(!Application.Current.Dispatcher.CheckAccess()) {
				Application.Current.Dispatcher.Invoke(() => AddEntireMesh(vertices, triangleIndices));
				return; // Important to leave the culprit thread
			}

			var pos = _mainMesh.Positions;
			var tri = _mainMesh.TriangleIndices;
			if(pos.Count != 0) {
				//Makes sure not to destroy original array
				triangleIndices = (int[])triangleIndices.Clone();
				int n = pos.Count; 
				for(int i = 0; i < triangleIndices.Length; i++) {
					triangleIndices[i] += n;
				}
			}
			foreach(Vec3 v in vertices) {
				pos.Add(new Point3D(v.X, v.Y, v.Z));
			}
			foreach(int i in triangleIndices) {
				tri.Add(i);
			}
            smoothVerts = pos.Clone();
            smoothTris = tri.Clone();
			_triangles = tri.Clone();
		}

        /// <summary>
        /// Exports the mesh to a location chosen by the user.
        /// </summary>
        /// <remarks>
        /// The method supports .obj and .ply file formats.
        /// </remarks>
		public void Export() {
			var path = PathChooser.FindSaveLocation();
			if(path == null) return;

			var vert = _mainMesh.Positions.Select(p => new Vec3((float) p.X, (float) p.Y, (float) p.Z)).ToArray();
			var tris = _mainMesh.TriangleIndices.ToArray();

			var polygon = new ExportPolygon(vert, tris);

			if(path.EndsWith(".obj")) {
				IOhandler.ObjExporter.Export(path, polygon);
			} else {
				PlyExporter.Export(path, polygon);
			}
		}
	}
}
