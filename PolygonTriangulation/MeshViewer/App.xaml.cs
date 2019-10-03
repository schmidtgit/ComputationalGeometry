using MeshViewer.View;
using PolygonTriangulation.Model;
using PolygonTriangulation.Polygon;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace MeshViewer {
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application {
        public static App Instance {
            get {
                if(_instance == null) {
                    Thread meshviewThread = new Thread(() => {
                        _instance = new MeshViewer.App();
                        System.Windows.Threading.Dispatcher.Run();
                    });
                    meshviewThread.SetApartmentState(ApartmentState.STA);
                    meshviewThread.IsBackground = true;
                    meshviewThread.Start();
                    while(_instance == null) { }
                }
                return _instance;
            }
        }
        private static App _instance;

        public void ShowLogs(params Log[] logs) {
            var tups = new Tuple<Log, string>[logs.Length];
            for(int i = 0; i < logs.Length; i++) {
                tups[i] = new Tuple<Log, string>(logs[i], null);
            }
            ShowLogs(tups);
        }

        public void ShowLogs(params Tuple<Log, string>[] logs) {
            if(!Application.Current.Dispatcher.CheckAccess()) {
                Application.Current.Dispatcher.Invoke(() => ShowLogs(logs));
                return; // Important to leave the culprit thread
            }

            if(logs == null) {
                throw new ArgumentException("This method should only be called using an Log array");
            }
            
            foreach(Tuple<Log, string> tup in logs) {
                var l = tup.Item1;
                var view = new LogViewer(l.vert, l.steps, tup.Item2);
                view.Show();
            }
        }

        public void ShowMeshes(params IPolygon[] meshes) {
            if(!Application.Current.Dispatcher.CheckAccess()) {
                Application.Current.Dispatcher.Invoke(() => ShowMeshes(meshes));
                return; // Important to leave the culprit thread
            }

            if(meshes == null) {
                throw new ArgumentException("This method should only be called using an IPolygon array");
            }
                
            foreach(IPolygon mesh in meshes) {
                var view = new MeshViewer3D(mesh);
                view.Show();
            }
        }

        public void ShowMeshes(params Tuple<IPolygon, string>[] meshes) {
            if(!Application.Current.Dispatcher.CheckAccess()) {
                Application.Current.Dispatcher.Invoke(() => ShowMeshes(meshes));
                return; // Important to leave the culprit thread
            }

            if(meshes == null) {
                throw new ArgumentException("This method should only be called using an IPolygon array");
            }

            foreach(Tuple<IPolygon, string> tup in meshes) {
                var mesh = tup.Item1;
                var view = new MeshViewer3D(mesh, tup.Item2);
                view.Show();
            }
        }
    }
}
