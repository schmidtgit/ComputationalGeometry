using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace MeshViewer.Wireframe {
    public class WireframeBuilder {
        public MeshGeometry3D Wires { get; private set; }

        private Dictionary<Tuple<int, int>, int> _drawn;
        private MeshGeometry3D _original;
        private double[] costhetas, sinthetas;

        public WireframeBuilder(MeshGeometry3D original) {
            _original = original;
            _drawn = new Dictionary<Tuple<int, int>, int>();
            Wires = new MeshGeometry3D();
            var dtheta = Math.PI / 1.5;
            costhetas = new double[4];
            sinthetas = new double[4];
            for(int i = 0; i < 4; i++) {
                costhetas[i] = Math.Cos(dtheta * i);
                sinthetas[i] = Math.Sin(dtheta * i);
            }
        }

        /// <summary>
        /// Generates a wireframe.
        /// </summary>
        /// <param name="logbased">Whether or not the wireframe is used for a logbased mesh or not. This is used to avoid the fake edge of the logbased mesh.</param>
        public void GenerateWireframe(bool logbased) {
            for(int triangle = logbased ? 3 : 0; 
                triangle < _original.TriangleIndices.Count;
                triangle += 3) {
                // Get the triangle's corner indices.
                int index1 = _original.TriangleIndices[triangle];
                int index2 = _original.TriangleIndices[triangle + 1];
                int index3 = _original.TriangleIndices[triangle + 2];

                // Make the triangle's three segments.
                AddLine(index1, index2);
                AddLine(index2, index3);
                AddLine(index3, index1);
            }
        }

        /// <summary>
        /// Clears the entire wireframe.
        /// </summary>
        public void ClearWireframe() {
            Wires.TriangleIndices.Clear();
            Wires.Positions.Clear();
            _drawn.Clear();
        }

        /// <summary>
        /// Removes a single triangle from the wireframe. Used by the logbased view.
        /// </summary>
        public void RemoveTriangle(int index1, int index2, int index3) {
            RemoveLine(index1, index2);
            RemoveLine(index2, index3);
            RemoveLine(index3, index1);
        }

        /// <summary>
        /// Tries to remove a single edge from the wireframe.
        /// The edge is not removed, if it is used by another triangle.
        /// </summary>
        private void RemoveLine(int index1, int index2) {
            if(index2 < index1) {
                var tmp = index1;
                index1 = index2;
                index2 = tmp;
            }

            var pair = Tuple.Create(index1, index2);
            int timesDrawn = _drawn[pair];
            if(timesDrawn < 2) {
                var tris = Wires.TriangleIndices;
                var pos = Wires.Positions;
                for(int i = 0; i < 18; i++) {
                    tris.RemoveAt(tris.Count - 1);
                    pos.RemoveAt(pos.Count - 1);
                }
                _drawn.Remove(pair);
            } else {
                _drawn[pair] = timesDrawn - 1;
            }
        }

        /// <summary>
        /// Adds a single triangle to the wireframe. Used by the logbased view.
        /// </summary>
        public void AddTriangle(int index1, int index2, int index3) {
            AddLine(index1, index2);
            AddLine(index2, index3);
            AddLine(index3, index1);
        }

        /// <summary>
        /// Tries to add a single line to the wireframe. If it has already been drawn it is not added.
        /// </summary>
        private void AddLine(int index1, int index2) {
            if(index2 < index1) {
                var tmp = index1;
                index1 = index2;
                index2 = tmp;
            }

            var pair = Tuple.Create(index1, index2);
            int timesDrawn;
            if(_drawn.TryGetValue(pair, out timesDrawn)) {
                _drawn[pair] = timesDrawn + 1;
            } else {
                _drawn.Add(pair, 1);
                AddLine(_original.Positions[index1], _original.Positions[index2] - _original.Positions[index1]);
            }
        }

        /// <summary>
        /// Adds a line to the wireframe. 
        /// </summary>
        /// <remarks>
        /// A line is represented as a triangle cylinder.
        /// The code is based on code from http://csharphelper.com/blog/2015/04/draw-cylinders-using-wpf-and-c/
        /// </remarks>
        private void AddLine(Point3D end_point, Vector3D axis) {
            Vector3D v1;
            if((axis.Z < -0.01) || (axis.Z > 0.01))
                v1 = new Vector3D(axis.Z, axis.Z, -axis.X - axis.Y);
            else
                v1 = new Vector3D(-axis.Y - axis.Z, axis.X, axis.X);
            Vector3D v2 = Vector3D.CrossProduct(v1, axis);

            // Make the vectors have length radius.
            v1 *= (0.03 / v1.Length);
            v2 *= (0.03 / v2.Length);

            // Make the sides.
            for(int i = 0; i < 3; i++) {
                Point3D p1 = end_point +
                    costhetas[i] * v1 +
                    sinthetas[i] * v2;

                Point3D p2 = end_point +
                    costhetas[i+1] * v1 +
                    sinthetas[i+1] * v2;

                Point3D p3 = p1 + axis;
                Point3D p4 = p2 + axis;

                AddTriangle(p1, p3, p2);
                AddTriangle(p2, p3, p4);
            }
        }

        /// <summary>
        /// Adds a triangle to the mesh.
        /// </summary>
        private void AddTriangle(Point3D p1, Point3D p2, Point3D p3) {
            Wires.TriangleIndices.Add(Wires.TriangleIndices.Count);
            Wires.Positions.Add(p1);
            Wires.TriangleIndices.Add(Wires.TriangleIndices.Count);
            Wires.Positions.Add(p2);
            Wires.TriangleIndices.Add(Wires.TriangleIndices.Count);
            Wires.Positions.Add(p3);
        }
    }
}
