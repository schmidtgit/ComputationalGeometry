using MeshViewer.ViewModel;
using PolygonTriangulation.Polygon;
using System.Windows;

namespace MeshViewer.View {
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MeshViewer3D : Window {

        /// <summary>
        /// Initializes a MeshViewer for the given mesh
        /// </summary>
		public MeshViewer3D(IPolygon mesh, string title = null) {
			InitializeComponent();

            if(title != null) {
                Title = title;
            }

            //Categorizes buttons to enable hiding of groups
            var toggleBtns = new[] { settingsToggle, cameraToggle };

            var camBtns = new UIElement[] { cameraChange, cameraReset };

            var settingBtns = new UIElement[] { faceToggle, vertToggle, wireframeToggle, export, smoothToggle };

            var colorBtns = new UIElement[] { faceColor, vertColor, wireColor, colTxtFace, colTxtVert, colTxtWire };

            //Initializes the ViewModel and sets up Commands
            var vm = new MeshViewerViewModel(Viewer, mesh, toggleBtns, camBtns, settingBtns, colorBtns);
			vm.CameraControl = new RelayCommand(o => vm.ChangeCameraSetting());
			vm.WireframeControl = new RelayCommand(o => vm.ToggleWireframe());
			vm.CameraResetControl = new RelayCommand(o => vm.ResetCamera());
			vm.FacesControl = new RelayCommand(o => vm.ToggleFaces());
			vm.VertControl = new RelayCommand(o => vm.ToggleVertices());
			vm.ExportControl = new RelayCommand(o => vm.Export());
            vm.SmoothControl = new RelayCommand(o => vm.ToggleSmooth());

			DataContext = vm;
		}
	}
}
