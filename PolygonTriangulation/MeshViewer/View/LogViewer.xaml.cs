using MeshViewer.ViewModel;
using PolygonTriangulation.Model;
using System.Windows;

namespace MeshViewer.View {
	/// <summary>
	/// Interaction logic for LogViewer.xaml
	/// </summary>
	public partial class LogViewer : Window {
        /// <summary>
        /// Initializes a LogViewer window with the given vertices and steps
        /// </summary>
		public LogViewer(Vec3[] vertices, StepInfo[] steps, string title = null) {
			InitializeComponent();

            if(title != null) {
                Title = title;
            }

            //Categorizes buttons to enable hiding of groups
            var toggleBtns = new[] { settingsToggle, cameraToggle, logToggle, colorToggle };

            var camBtns = new[] { cameraChange, cameraReset };

            var settingBtns = new UIElement[] { faceToggle, vertToggle, meshToggle, wireframeToggle, export, smoothToggle };

            var colorBtns = new UIElement[] { faceColor, vertColor, wireColor, cubeColor, colTxtFace, colTxtVert, colTxtWire, colTxtCube };

            var logBtns = new UIElement[] { logIncrement, logDecrement, seconds, secText, logRun, logMaxvalue, logCurrent };

            //Initializes the ViewModel and sets up Commands
            var vm = new LogViewerViewModel(Viewer, seconds, vertices, steps, toggleBtns, camBtns, settingBtns, colorBtns, logBtns);
			vm.CameraControl = new RelayCommand(o => vm.ChangeCameraSetting());
			vm.CameraResetControl = new RelayCommand(o => vm.ResetCamera());
            vm.WireframeControl = new RelayCommand(o => vm.ToggleWireframe());
            vm.IncrementControl = new RelayCommand(o => vm.IncrementStep());
			vm.DecrementControl = new RelayCommand(o => vm.DecrementStep());
			vm.FacesControl = new RelayCommand(o => vm.ToggleFaces());
			vm.FullMeshControl = new RelayCommand(o => vm.ToggleFullMesh());
			vm.VertControl = new RelayCommand(o => vm.ToggleVertices());
			vm.ExportControl = new RelayCommand(o => vm.Export());
            vm.AutoRunControl = new RelayCommand(o => vm.AutoRun());
            vm.SmoothControl = new RelayCommand(o => vm.ToggleSmooth());

			DataContext = vm;
		}
    }
}
