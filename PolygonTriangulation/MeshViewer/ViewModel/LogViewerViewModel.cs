using HelixToolkit.Wpf;
using IOhandler;
using PolygonTriangulation.Model;
using PolygonTriangulation.Polygon;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace MeshViewer.ViewModel {
	public class LogViewerViewModel : ViewportViewModel{
		//Commands
		public ICommand IncrementControl { get; set; }
		public ICommand DecrementControl { get; set; }
		public ICommand FullMeshControl { get; set; }
        public ICommand AutoRunControl { get; set; }

		//Properties for the view
		public int StepValue {
			get { return _stepValue; }
			set {
				if(_stepValue != value) {
					_stepValue = value;
					ChangeStepValue(value);
				}
				OnPropertyChanged();
			}
		}
		private int _stepValue = 0;

        //Color properties and values
        public Color CubeColor {
            get { return _cubeColor; }
            set {
                if(value != null) {
                    _cubeColor = value;
                    _cubeMaterial.Color = value;
                }
            }
        }
        private Color _cubeColor;
        private SolidColorBrush _cubeMaterial;

        //Properties and values for the log button
        public bool LogMode {
            get { return logMode; }
            set {
                SetVisiblity(value, logBtns);
                logMode = value;
            }
        }
        private bool logMode;
        private UIElement[] logBtns;

        //Property for the textfield to show max value
        public int MaxValue { get; private set; }

		//Shared fields
		static PerspectiveCamera sharedCamera;

        //Smooth variables
        bool smooth = true;
        Point3DCollection smoothPos;
        StepInfo[] smoothSteps;

        //Logbased variables
        TextBox _seconds;
        Stopwatch _watch;
		MeshGeometry3D _cube;
		MeshGeometry3D _currentTriangle;
		StepInfo[] _steps;
		int _stepcount;
		bool _showFull;

        /// <summary>
        /// Initializes the ViewModel
        /// </summary>
		public LogViewerViewModel(HelixViewport3D viewport, TextBox seconds, Vec3[] vertices, 
                                        StepInfo[] steps, UIElement[] toggleButtons, UIElement[] camButtons, UIElement[] settingButtons, UIElement[] colorButtons, UIElement[] logButtons) {
            _viewPort = viewport;
            _seconds = seconds;
            cameraBtns = camButtons;
            settingsBtns = settingButtons;
            colorBtns = colorButtons;
            logBtns = logButtons;
            tglBtns = toggleButtons;
            _watch = new Stopwatch();
            _stepcount = -1;
			InstantiateEmptyScene();
			AddLogDetails(vertices, steps);
			SetCameraToDefault();
		}

		/// <summary>
		/// Changes between an individual camera and a shared one
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

        /// <summary>
        /// Initializes all the objects needed for the view
        /// </summary>
		private void InstantiateEmptyScene() {
			CreateEmptyCollection();
			AddDefaultLight();
			CreateEmptyMesh();
            CreateEmptyWireframe();
			CreateEmptyTriangles();
			CreateEmptyCube();
			CreateEmptyVerticesMesh();
		}

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
        /// Initializes the objects needed to show the debugcube
        /// </summary>
		private void CreateEmptyCube() {
            var gm3 = new GeometryModel3D();
            _cube = new MeshGeometry3D();
            gm3.Geometry = _cube;
            _cubeColor = Color.FromRgb(192, 57, 43); 
            _cubeMaterial = new SolidColorBrush(_cubeColor);
            _cubeMaterial.Opacity = 0.6;
            gm3.Material = new DiffuseMaterial(_cubeMaterial);
            _collection.Add(gm3);
        }

        /// <summary>
        /// Initializes the objects needed to show the triangles inside the debugcube
        /// </summary>
		private void CreateEmptyTriangles() {
			var gm3 = new GeometryModel3D();
			_currentTriangle = new MeshGeometry3D();
			gm3.Geometry = _currentTriangle;
			gm3.Material = new DiffuseMaterial(Brushes.Black);
			_collection.Add(gm3);
		}

		/// <summary>
		/// Adds the details of a log to the viewmodel
		/// </summary>
		/// <param name="vertices">The vertices used by the mesh</param>
		/// <param name="steps">The steps taken by the algorithm</param>
		private void AddLogDetails(Vec3[] vertices, StepInfo[] steps) {
			if(!Application.Current.Dispatcher.CheckAccess()) {
				Application.Current.Dispatcher.Invoke(() => AddLogDetails(vertices, steps));
				return; // Important to leave the culprit thread
			}

            MaxValue = steps.Count() - 1;

			var pos = _mainMesh.Positions;
			var tri = _mainMesh.TriangleIndices;
			
			foreach(Vec3 v in vertices) {
				pos.Add(new Point3D(v.X, v.Y, v.Z));
			}

            //Invisible triangle added to avoid drawing anything
            if(pos.Count != 0) {
                tri.Add(0);
                tri.Add(0);
                tri.Add(0);
            }
			_steps = steps;
            smoothSteps = steps;
            smoothPos = pos.Clone();
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
                var newSteps = new StepInfo[_steps.Length];
                var index = 0;
                for(int i = 0; i < _steps.Length; i++) {
                    newSteps[i] = new StepInfo { Center = _steps[i].Center, HalfDist = _steps[i].HalfDist, triangles = new List<int[]>() };
                    for(int j = 0; j < _steps[i].triangles.Count; j++) {
                        var old = _steps[i].triangles[j];
                        foreach(var ind in old) {
                            newPos.Add(_mainMesh.Positions[ind]);
                        }
                        newSteps[i].triangles.Add(new[] { index++, index++, index++ });
                    }
                }
                _mainMesh.Positions = newPos;
                _steps = newSteps;
            } else {
                _mainMesh.Positions = smoothPos.Clone();
                _steps = smoothSteps;                
            }
            _mainMesh.TriangleIndices.Clear();
            if(_mainMesh.Positions.Count != 0) {
                _mainMesh.TriangleIndices.Add(0);
                _mainMesh.TriangleIndices.Add(0);
                _mainMesh.TriangleIndices.Add(0);
            }
            _stepcount = 0;
            if(_showFull) {
                IncrementTo(MaxValue);
                _stepcount = _stepValue;
            } else {
                IncrementTo(_stepValue);
            }
            smooth = !smooth;
        }

        /// <summary>
        /// Toggles whether or not the entire mesh is shown.
        /// </summary>
		public void ToggleFullMesh() {
            if(_showFull) {
                _wireframe.ClearWireframe();
                ClearMesh();
                _showFull = !_showFull;
                ShowMesh();
			} else {
				for(int i = _stepcount; i <= MaxValue; i++) {
					AddToMain(i);
				}
                _showFull = !_showFull;
            }
		}

        /// <summary>
        /// Shows the mesh up to the current step.
        /// </summary>
		protected override void ShowMesh() {
            var steps = _showFull ? MaxValue + 1 : _stepcount;
			for(int i = 0; i < steps; i++) {
				AddToMain(i);
			}
		}

		/// <summary>
		/// Rewinds the algorithm one step.
		/// </summary>
		public void DecrementStep() {
			if(_stepcount < 1) {
				//Return to make sure the button doesnt do anything
				return;
			}
			DecrementTo(_stepcount - 1);
		}

        /// <summary>
        /// Removes all triangles at the specified step from the mesh and wireframe.
        /// </summary>
		private void RemoveFromMain(int stepcount) {
            if(_showFull) return;
            if(stepcount < 0 || stepcount > MaxValue) return;
			var tris = _mainMesh.TriangleIndices;
			foreach(int[] tri in _steps[stepcount].triangles) {
				foreach(int i in tri) {
					tris.RemoveAt(tris.Count - 1);
				}
                if(_wiresActive && !_showFull) {
                    _wireframe.RemoveTriangle(tri[0], tri[1], tri[2]);
                }
            }
		}

        /// <summary>
        /// Add all triangles at the specified step to the mesh and wireframe.
        /// </summary>
		private void AddToMain(int stepcount) {
            if(_showFull) return;
            if(stepcount < 0 || stepcount > MaxValue) return;
			var tris = _mainMesh.TriangleIndices;
			foreach(int[] tri in _steps[stepcount].triangles) {
				foreach(int i in tri) {
					tris.Add(i);
				}
                if(_wiresActive && !_showFull) {
                    _wireframe.AddTriangle(tri[0], tri[1], tri[2]);
                }
			}
		}

		/// <summary>
		/// Advances the algorithm one step.
		/// </summary>
		public void IncrementStep() {
			if(_stepcount >= _steps.Count() - 1) {
				//Return to make sure the button doesnt do anything
				return;
			}
			IncrementTo(_stepcount + 1);
		}

        /// <summary>
        /// Clears the triangles inside the debugcube.
        /// </summary>
		private void ClearTmpTriangles() {
			_currentTriangle.Positions.Clear();
			_currentTriangle.TriangleIndices.Clear();
		}

        /// <summary>
        /// Shows the triangles that was added at the specified step.
        /// </summary>
		private void ShowTriangles(int stepcount) {
			var pos = _currentTriangle.Positions;
			pos.Clear();
			var tris = _currentTriangle.TriangleIndices;
			tris.Clear();
			if(stepcount < 0 || stepcount > MaxValue) return;
			int index = 0;
			foreach(int[] tri in _steps[stepcount].triangles) {
				foreach(int i in tri) {
					pos.Add(_mainMesh.Positions[i]);
					tris.Add(index++);
				}
			}
		}

		private void ClearCube() {
			_cube.Positions.Clear();
			_cube.TriangleIndices.Clear();
		}

		private void ShowCube(int stepcount) {
			var pos = _cube.Positions;
			pos.Clear();
			var tris = _cube.TriangleIndices;
			tris.Clear();
			if(stepcount < 0 || stepcount > MaxValue) return;
			var center = _steps[stepcount].Center;
			var halfstep = _steps[stepcount].HalfDist;
			foreach(Vec3 corner in GenerateCorners(halfstep, center)) {
				pos.Add(new Point3D(corner.X, corner.Y, corner.Z));
			}
			foreach(int i in CubeIndices()) {
				tris.Add(i);
			}
		}

		private void ChangeStepValue(int i) {
			if(i < 0) i = 0;
			if(i > MaxValue) i = MaxValue;
			if(i < _stepcount) {
				DecrementTo(i);
			} else if (i > _stepcount) {
				IncrementTo(i);
			}
		}

		private void DecrementTo(int i) {
			for(_stepcount--; _stepcount > i; _stepcount--) {
				RemoveFromMain(_stepcount);
			}
			RemoveFromMain(_stepcount);
			ShowCube(_stepcount);
			ShowTriangles(_stepcount);
			StepValue = _stepcount;
		}

		private void IncrementTo(int i) {
			if(_stepcount < 0) { _stepcount = 0; }
			for(; _stepcount < i; _stepcount++) {
				AddToMain(_stepcount);
			}
			ShowCube(_stepcount);
			ShowTriangles(_stepcount);
			StepValue = _stepcount;
		}

        private int[] CubeIndices() {
			return new[] {
				0, 1, 2,
				0, 2, 3,
				0, 3, 7,
				0, 7, 4,
				0, 4, 5,
				0, 5, 1,
				1, 5, 6,
				1, 6, 2,
				2, 6, 7,
				2, 7, 3,
				4, 7, 6,
				4, 6, 5
			};
		}

		private Vec3[] GenerateCorners(float halfstep, Vec3 center) {
			var pos = halfstep;
			var neg = -pos;
			Vec3[] cp = new Vec3[] {
				new Vec3(neg,neg,neg) + center,
				new Vec3(pos,neg,neg) + center,
				new Vec3(pos,neg,pos) + center,
				new Vec3(neg,neg,pos) + center,
				new Vec3(neg,pos,neg) + center,
				new Vec3(pos,pos,neg) + center,
				new Vec3(pos,pos,pos) + center,
				new Vec3(neg,pos,pos) + center
			};
			return cp;
		}

        /// <summary>
        /// Exports the currently shown mesh to a location chosen by the user.
        /// </summary>
        /// <remarks>
        /// The method supports .obj and .ply file formats.
        /// </remarks>
		public void Export() {
			var path = PathChooser.FindSaveLocation();
			if(path == null) return;

			var vert = _mainMesh.Positions.Select(p => new Vec3((float) p.X, (float) p.Y, (float) p.Z)).ToArray();
			var tris = new List<int>();

			for(int i = 0; i < _stepcount; i++) {
				var step = _steps[i];
				foreach(int[] tri in step.triangles) {
					foreach(int ii in tri) {
						tris.Add(ii);
					}
				}
			}

			var polygon = new ExportPolygon(vert, tris.ToArray());

			if(path.EndsWith(".obj")) {
				IOhandler.ObjExporter.Export(path, polygon);
			} else {
				PlyExporter.Export(path, polygon);
			}
		}

        /// <summary>
        /// Runs through the entire algorithm.
        /// </summary>
        public void AutoRun() {
            int sec;
            if(!int.TryParse(_seconds.Text, out sec)) return;
            HideButtons();
            StepValue = 0;
            RunStep(0, sec*1000/MaxValue);
        }

        private void HideButtons() {
            SetVisiblity(false, logBtns);
            SetVisiblity(false, tglBtns);
        }

        private void ShowButtons() {
            SetVisiblity(true, logBtns);
            SetVisiblity(true, tglBtns);
        }
        
        /// <summary>
        /// Runs one step of the autorun function and recursively calls the next step with a delay.
        /// </summary>
        private void RunStep(int step, int delay) {
            _watch.Stop();
            var time = (int) _watch.ElapsedMilliseconds;
            _watch.Restart();
            Application.Current.Dispatcher.Invoke(() => {
                if(step > MaxValue) {
                    ShowButtons();
                    return;
                }
                IncrementTo(step);
                
                Task.Delay(Math.Max(delay-time,0)).ContinueWith(t => RunStep(step + 1, delay));
            });
        }
	}
}
