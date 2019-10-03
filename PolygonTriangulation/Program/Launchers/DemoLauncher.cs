using PolygonTriangulation.Builder;
using PolygonTriangulation.ImplicitObjects;
using PolygonTriangulation.Model;
using PolygonTriangulation.Polygon;
using PolygonTriangulation.PostProcessing;
using PolygonTriangulation.Triangulation;
using Program.RuntimeImplicit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Program.Launchers {
    public class DemoLauncher : BaseLauncher, IProgramLauncher {
        public bool Run() {
            Program.PrintTitle("DEMO LAUNCHER");

            Console.WriteLine("\nWelcome to the demo launcher!");
            Console.WriteLine("The demo launcher consists of a total of 4 steps displaying the capabilities of the program");

            Console.WriteLine("\nPRESS ANY KEY TO CONTINUE");
            Console.ReadKey();

            Program.PrintTitle("STEP 1");
            if(AskUser("Run step 1: Triangulation an intersection between a sphere and a cube?"))
                CubeSphereIntersection();

            Program.PrintTitle("STEP 2");
            if(AskUser("Run step 2: Run multiple algorithms with step visualizer?"))
                MultipleAlgorithms();

            Program.PrintTitle("STEP 3");
            if(AskUser("Run step 3: Triangulate a cube and a sphere with applied Perlin Noise?"))
                NoiseDemo();

            Program.PrintTitle("STEP 4");
            if(AskUser("Run step 4: Run the PolyChop on the mesh of a cow?"))
                Polychop();

            Program.PrintTitle("DEMO LAUNCHER FINISHED");
            return !AskUser("Restart application?");
        }

        private void CubeSphereIntersection() {
            Program.PrintTitle("RUNNING STEP 1");
            var cube = new PolygonTriangulation.ImplicitObjects.Cube(60);
            var sphere = new PolygonTriangulation.ImplicitObjects.Sphere(35);
            var intersection = new Intersection(cube, sphere);
            var algo = new WeightedCubes();

            var result = Triangulate(new[] { algo }, new[] { intersection });

            if(!LaunchView(result))
                Export(result);

            Console.WriteLine("\nStep 1 finished");
            Console.WriteLine("\nPRESS ANY KEY TO CONTINUE");
        }

        private void MultipleAlgorithms() {
            Program.PrintTitle("RUNNING STEP 2");
            Console.WriteLine("Choose an SDF to be run with Weighted Marching Cubes, Weighted Marching Tetrahedra and Weighted Naive Surface Nets");

            var sdf = Program.UserInstantiation<IRuntimeSDF>().Instance();

            var result = TriangulateWithLog(new ITriangulator[] { new WeightedCubes(), new WeightedTetrahedra(), new WeightedNaiveSurfaceNets() }, 
                                                    new[] { sdf, sdf, sdf });

            if(!LaunchLogView(result))
                Export(result);

            Console.WriteLine("\nStep 2 finished");
            Console.WriteLine("\nPRESS ANY KEY TO CONTINUE");
        }

        private void NoiseDemo() {
            Program.PrintTitle("RUNNING STEP 3");

            var noise = new Func<Vec3, float>(p => Noise.PerlinNoise(p * 0.08f) * 4);

            var cube = new PolygonTriangulation.ImplicitObjects.Cube(50);
            var noisecube = new Displacement(cube, noise);

            var sphere = new PolygonTriangulation.ImplicitObjects.Sphere(25);
            var noisesphere = new Displacement(sphere, noise);

            var algo = new WeightedCubes(64);
            var algo2 = new WeightedCubes(64);

            var result = Triangulate(new[] { algo, algo2 }, new[] { noisecube, noisesphere });
            
            if(!LaunchView(result))
                Export(result);

            Console.WriteLine("\nStep 3 finished");
            Console.WriteLine("\nPRESS ANY KEY TO CONTINUE");
        }

        private void Polychop() {
            Program.PrintTitle("RUNNING STEP 4");
            Console.WriteLine("PolyChop is running");

            var cow = Figures.Cow();
            
            var details = new[] { 100, 25, 10, 2, 1 };

            var resulttup = new Tuple<IPolygon, string>[details.Length];
            var resultarray = new IPolygon[details.Length];

            for(int i = 0; i < details.Length; i++) {
                resultarray[i] = new Polychop().Run(cow, new ExportBuilder(), (int)(cow.VertexCount * (100f - details[i])/100f));
                resulttup[i] = Tuple.Create(resultarray[i], "Cow mesh with " + (100 - details[i]) + " % edges removed by PolyChop");
            }

            if(!LaunchView(resulttup))
                Export(resultarray, new long[resultarray.Length]);

            Console.WriteLine("\nStep 4 finished");
            Console.WriteLine("\nPRESS ANY KEY TO CONTINUE");
        }
    }
}
