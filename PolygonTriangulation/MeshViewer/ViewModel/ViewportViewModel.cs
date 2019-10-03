using HelixToolkit.Wpf;
using MeshViewer.Wireframe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace MeshViewer.ViewModel {
    /// <summary>
    /// A class used as superclass for LogViewerViewModel and MeshViewerViewModel.
    /// </summary>
	public abstract class ViewportViewModel : BaseViewModel{
		//Shared commands used for both views
		public ICommand CameraControl { get; set; }
		public ICommand CameraResetControl { get; set; }
		public ICommand WireframeControl { get; set; }
		public ICommand FacesControl { get; set; }
		public ICommand VertControl { get; set; }
		public ICommand ExportControl { get; set; }
        public ICommand SmoothControl { get; set; }


        //Properties and values for buttons
        public bool CameraMode {
            get { return cameraMode; }
            set {
                SetVisiblity(value, cameraBtns);
                cameraMode = value;
            }
        }
        private bool cameraMode = true;
        protected UIElement[] cameraBtns;
        public bool SettingsMode {
            get { return settingsMode; }
            set {
                SetVisiblity(value, settingsBtns);
                settingsMode = value;
            }
        }
        private bool settingsMode;
        protected UIElement[] settingsBtns;
        public bool ColorMode {
            get { return colorMode; }
            set {
                SetVisiblity(value, colorBtns);
                colorMode = value;
            }
        }
        private bool colorMode;
        protected UIElement[] colorBtns;
        protected UIElement[] tglBtns;

        //Properties and values for colors
        public Color FaceColor {
            get { return _faceColor; }
            set {
                if(value != null) {
                    _faceColor = value;
                    _faceMaterial.Color = value;
                }
            }
        }
        protected Color _faceColor;
        protected SolidColorBrush _faceMaterial;
        public Color VertColor {
            get { return _vertColor; }
            set {
                if(value != null) {
                    _vertColor = value;
                    _vertMaterial.Color = value;

                }
            }
        }
        protected Color _vertColor;
        protected SolidColorBrush _vertMaterial;
        public Color WireColor {
            get { return _wireColor; }
            set {
                if(value != null) {
                    _wireColor = value;
                    _wireMaterial.Color = value;
                }
            }
        }
        protected Color _wireColor;
        protected SolidColorBrush _wireMaterial;

        //Toggle variables
        protected PerspectiveCamera _camera;
		protected bool _sharedCameraMode;
        protected WireframeBuilder _wireframe;
		protected bool _wiresActive;
		protected bool _facesActive = true;
		protected bool _showVerts;

		//Standard variables
		protected HelixViewport3D _viewPort;
		protected Model3DCollection _collection;
		protected MeshGeometry3D _mainMesh;
		protected MeshGeometry3D _vertices;
		protected Model3DGroup _group;

        /// <summary>
        /// Toggles whether or not the faces of the mesh is visible.
        /// </summary>
		public void ToggleFaces() {
			if(_facesActive) {
				ClearMesh();
			} else {
				ShowMesh();
			}
			_facesActive = !_facesActive;
		}

        /// <summary>
        /// Clears the current mesh.
        /// </summary>
		protected void ClearMesh() {
			_mainMesh.TriangleIndices.Clear();
			_mainMesh.TriangleIndices.Add(0); _mainMesh.TriangleIndices.Add(0); _mainMesh.TriangleIndices.Add(0);
		}

        /// <summary>
        /// Shows the mesh in the scene.
        /// </summary>
		protected abstract void ShowMesh();

        /// <summary>
        /// Adds ambientlight and directionallight to the scene
        /// </summary>
        protected void AddDefaultLight() {
            var dir = new DirectionalLight(Color.FromRgb(255, 255, 255), new Vector3D(1, -5, 1));
            var amb = new AmbientLight(Color.FromRgb(170, 170, 170));
            _collection.Add(dir);
            _collection.Add(amb);
        }

        /// <summary>
        /// Initializes the objects needed to show vertices.
        /// </summary>
		protected void CreateEmptyVerticesMesh() {
			var gm3 = new GeometryModel3D();
			_vertices = new MeshGeometry3D();
			gm3.Geometry = _vertices;
            _vertColor = Colors.Black;
            _vertMaterial = new SolidColorBrush(_vertColor);
			gm3.Material = new DiffuseMaterial(_vertMaterial);
			_collection.Add(gm3);
		}

		/// <summary>
		/// Toggles whether vertices are visible or not.
		/// </summary>
		/// <remarks>A tetrahedra is created for each vertex, because the viewport only supports drawing triangles.</remarks>
		public void ToggleVertices() {
			if(_showVerts) {
				ClearVerts();
			} else {
				CreateVerts();
			}
			_showVerts = !_showVerts;
		}

        /// <summary>
        /// Hides all vertices.
        /// </summary>
		private void ClearVerts() {
			_vertices.Positions.Clear();
			_vertices.TriangleIndices.Clear();
		}

        /// <summary>
        /// Shows all vertices of the mesh.
        /// </summary>
        /// <remarks>
        /// The vertices are created as tetrahedras based on https://en.wikipedia.org/wiki/Tetrahedron#Formulas_for_a_regular_tetrahedron
        /// </remarks>
        private void CreateVerts() {
			for(int i = 0; i < _mainMesh.Positions.Count; i++) {
				var vert = _mainMesh.Positions[i];
				double x = vert.X;
				double y = vert.Y;
				double z = vert.Z;
				var offset = i * 4;

				var num = 0.1 / Math.Sqrt(2);
				Point3D p1 = new Point3D(x + 0.1, y, z - num);
				Point3D p2 = new Point3D(x - 0.1, y, z - num);
				Point3D p3 = new Point3D(x, y + 0.1, z + num);
				Point3D p4 = new Point3D(x, y - 0.1, z + num);

				_vertices.Positions.Add(p1);
				_vertices.Positions.Add(p2);
				_vertices.Positions.Add(p3);
				_vertices.Positions.Add(p4);
				_vertices.TriangleIndices.Add(0 + offset);
				_vertices.TriangleIndices.Add(1 + offset);
				_vertices.TriangleIndices.Add(2 + offset);
				_vertices.TriangleIndices.Add(0 + offset);
				_vertices.TriangleIndices.Add(3 + offset);
				_vertices.TriangleIndices.Add(1 + offset);
				_vertices.TriangleIndices.Add(0 + offset);
				_vertices.TriangleIndices.Add(2 + offset);
				_vertices.TriangleIndices.Add(3 + offset);
				_vertices.TriangleIndices.Add(3 + offset);
				_vertices.TriangleIndices.Add(2 + offset);
				_vertices.TriangleIndices.Add(1 + offset);
			}
		}

        /// <summary>
        /// Initializes the objects needed to show a wireframe.
        /// </summary>
        protected void CreateEmptyWireframe() {
            _wireframe = new WireframeBuilder(_mainMesh);

            var gm3 = new GeometryModel3D();
            gm3.Geometry = _wireframe.Wires;
            _wireColor = Colors.White;
            _wireMaterial = new SolidColorBrush(_wireColor);
            gm3.Material = new EmissiveMaterial(_wireMaterial);
            _collection.Add(gm3);
        }
        
        /// <summary>
        /// Toggles a wireframe of the mesh.
        /// </summary>
        public void ToggleWireframe() {
            if(_wiresActive) {
                _wireframe.ClearWireframe();
            } else {
                _wireframe.GenerateWireframe(this is LogViewerViewModel);
            }
            _wiresActive = !_wiresActive;
        }

		/// <summary>
		/// Resets the current camera to the standard position.
		/// </summary>
		public void ResetCamera() {
			var top = new Point3D(-_mainMesh.Bounds.SizeX * 2, _mainMesh.Bounds.SizeY * 2, _mainMesh.Bounds.SizeZ * 2);
			var cam = _viewPort.Camera;
			cam.Position = top;
			cam.LookDirection = new Vector3D(-cam.Position.X, -cam.Position.Y, -cam.Position.Z);
			cam.UpDirection = new Vector3D(0, 1, 0);
		}

		/// <summary>
		/// Adds a default camera to the scene.
		/// </summary>
		protected void SetCameraToDefault() {
			var top = new Point3D(-_mainMesh.Bounds.SizeX * 2, _mainMesh.Bounds.SizeY * 2, _mainMesh.Bounds.SizeZ * 2);
			_camera = new PerspectiveCamera();
			_camera.Position = top;
			_camera.LookDirection = new Vector3D(-_camera.Position.X, -_camera.Position.Y, -_camera.Position.Z);
			_camera.UpDirection = new Vector3D(0, 1, 0);
			_viewPort.Camera = _camera;
		}

		/// <summary>
		/// Creates on empty collection for the viewport.
		/// </summary>
		protected void CreateEmptyCollection() {
			_collection = new Model3DCollection();
			_group = new Model3DGroup();
			_group.Children = _collection;
			var mw3 = new ModelVisual3D();
			mw3.Content = _group;
			_viewPort.Children.Add(mw3);
		}

        /// <summary>
        /// Used to hide or show a group of UIElements.
        /// </summary>
        /// <param name="visible">Whether or not the UIElements should be visible.</param>
        /// <param name="elements">The elements to hide or show.</param>
        protected void SetVisiblity(bool visible, IEnumerable<UIElement> elements) {
            foreach(UIElement ele in elements) {
                if(visible) {
                    ele.Visibility = Visibility.Visible;
                } else {
                    ele.Visibility = Visibility.Hidden;
                }
            }

        }
    }
}
