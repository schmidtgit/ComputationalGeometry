using PolygonTriangulation.Builder;
using PolygonTriangulation.Model;
using PolygonTriangulation.Polygon;
using Priority_Queue;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PolygonTriangulation.PostProcessing {
	// Inspired by Polychop, Stan Melax 1998
	// Translatede from C to C# to Object Oriented C# 
	public class Polychop {
		private List<T> _triangles;
		private StablePriorityQueue<V> _pq;

		/// <summary>
		/// Correct vertices reuse is required, which is ensured by using the ExportPolygon builder.
		/// </summary>
		/// <param name="mesh">Any polygon, preferable a ExportPolygon</param>
		private void Import(IPolygon mesh) {
			mesh = ExportPolygon.Convert(mesh);
			_pq = new StablePriorityQueue<V>(mesh.VertexCount);
			_triangles = new List<T>();

			var verts = new List<V>();
			foreach (Vec3 position in mesh.Vertices)
				verts.Add(new V(position, this));

			int index = 0;
			var t = mesh.Triangles.ToArray();
			while (index < t.Length)
				_triangles.Add(new T(verts[t[index++]], verts[t[index++]], verts[t[index++]], this));

			foreach (V v in verts)
				v.ComputeEdgeCostAtVertex();
		}

		/// <summary>
		/// Runs Polychop on the given mesh until the cost of removing a vertex exceeds the tolerance.
		/// Cost is calculated as curvature * length of edge to collapse.
		/// </summary>
		/// <param name="mesh">The Polygon to run the algorithm on.</param>
		/// <param name="builder">Preferred builder.</param>
		/// <param name="tolerance">Negative tolerance will result in unwanted behavior.</param>
		/// <returns></returns>
		public IPolygon Run(IPolygon mesh, IPolygonBuilder builder, float tolerance = 0.00001f) {
			Import(mesh);

			// Run algorithm
			var useless = _pq.Count > 0 ? _pq.Dequeue() : null;
			while (useless != null && useless.objDist <= tolerance) {
				useless.Collapse();
				useless = _pq.Count > 0 ? _pq.Dequeue() : null;
			}

			// Build
			foreach (T tris in _triangles)
				if(tris.normal != Vec3.zero)
					builder.Append(tris.vert[0].position, tris.vert[1].position, tris.vert[2].position);

			return builder.Build();
		}

		/// <summary>
		/// Runs Polychop on the given mesh until the cost of removing a vertex exceeds the tolerance.
		/// Cost is calculated as curvature * length of edge to collapse.
		/// </summary>
		/// <param name="mesh">The Polygon to run the algorithm on.</param>
		/// <param name="builder">Preferred builder.</param>
		/// <param name="iterations">Number of iterations to run.</param>
		/// <returns></returns>
		public IPolygon Run(IPolygon mesh, IPolygonBuilder builder, int iterations) {
			Import(mesh);

			// Run algorithm
			var useless = _pq.Count > 0 ? _pq.Dequeue() : null;
			for(int i = 0; i < iterations; i++) {
				useless.Collapse();
				useless = _pq.Count > 0 ? _pq.Dequeue() : null;
			}

			// Build
			foreach (T tris in _triangles)
				if (tris.normal != Vec3.zero)
					builder.Append(tris.vert[0].position, tris.vert[1].position, tris.vert[2].position);

			return builder.Build();
		}

		/// <summary>
		/// Enqueues or updates the vertex in the priority queue.
		/// Crashes if update is called on a not already enqued object.
		/// </summary>
		/// <param name="v">Vertex to enqueue / update.</param>
		/// <param name="b">True if object has never been enqued before.</param>
		void EnqueuePQ(V v, bool b) {
			if (b)
				_pq.Enqueue(v, v.objDist);
			else
				_pq.UpdatePriority(v, v.objDist);
		}

		/// <summary>
		/// Removes a triangle from the mesh, called by T.Dispose();
		/// </summary>
		/// <param name="t">Triangle to remove.</param>
		void TrianglesRemove(T t) { _triangles.Remove(t); }

		/// <summary>
		/// This algorithm focus on removing vertices.
		/// V functions as a node in a large graph (the mesh).
		/// The main functionality of the algorithm is stored here.
		/// </summary>
		class V : StablePriorityQueueNode, IDisposable
		{
			Polychop parent;
			public Vec3 position { get; set; }
			public float centerCost { get; set; }
			public HashSet<V> neighbor { get; set; } = new HashSet<V>();
			public HashSet<T> face { get; set; } = new HashSet<T>();
			public float objDist { get; set; }
			V collapse;

			/// <summary>
			/// V is an enhanced vector 3, heavily used by the algorithm.
			/// </summary>
			/// <param name="position">Vertex position eular space.</param>
			/// <param name="parent">Polychop parent - to prevent having static variables.</param>
			public V(Vec3 position, Polychop parent) {
				this.parent = parent;
				this.position = position;
				centerCost = position.SquaredMagnitude();
				objDist = float.NaN;
			}

			/// <summary>
			/// Collapses the current V with the current best candidate.
			/// This will remove or update all connected triangles.
			/// </summary>
			public void Collapse() {
				if (collapse == null) {
					Dispose();
					return;
				}

				// Temp array required, since triangles will manipulate adjacent vertices!
				var tmp = neighbor.ToArray();

				foreach (T tris in face.ToArray()) { 
					if (tris.HasVertex(collapse))
						tris.Dispose();
					else
						tris.ReplaceVertex(this, collapse);
				}

				Dispose();
				foreach (V vert in tmp)
					vert.ComputeEdgeCostAtVertex();
			}

			/// <summary>
			/// Computes the worst case cost of collapsing into the vertex V.
			/// </summary>
			/// <param name="v">Connected vertex considered for collapse candidate.</param>
			/// <returns></returns>
			float ComputeEdgeCollapseCost(V v)
			{
				float edgelength = (v.position - position).Magnitude();
				float curvature = 0;

				// Find triangles touching the edge from this to v.
				List<T> edgeTris = new List<T>();
				foreach (T tris in face)
					if (tris.HasVertex(v))
						edgeTris.Add(tris);

				// Find worst-case curvature
				foreach (T tris1 in face) {
					float minCurvature = 1;
					foreach (T tris2 in edgeTris)
						minCurvature = Math.Min(minCurvature, (1 - Vec3.DotProduct(tris1.normal, tris2.normal)) / 2f);
					curvature = Math.Max(curvature, minCurvature);
				}

				// Not in original Polychop!
				// Makes the algorithm favor collapsing away from center,
				// often resulting in fewer polygons.
				if (curvature == 0)
					return -v.centerCost;

				return edgelength * curvature;
			}

			/// <summary>
			/// Computes the best case cost of collapsing this vertex.
			/// Updates the collapse candidate and current position in pq.
			/// </summary>
			public void ComputeEdgeCostAtVertex() {
				// Used to check if this vertex have been enqueued before
				var tmp = objDist;

				collapse = null;
				objDist = float.MinValue;

				if (neighbor.Count == 0)
					return;

				objDist = float.MaxValue;
				foreach (V v in neighbor) {
					float dist;
					dist = ComputeEdgeCollapseCost(v);
					if (dist < objDist){
						collapse = v;
						objDist = dist;
					}
				}

				parent.EnqueuePQ(this, float.IsNaN(tmp));
			}

			/// <summary>
			/// Removes the vert from neighbors if no triangles are shared!
			/// </summary>
			/// <param name="vert">Vert to remove.</param>
			public void RemoveIfNonNeighbor(V vert) {
				foreach (T tris in face)
					if (tris.HasVertex(vert))
						return;
				neighbor.Remove(vert);
			}

			/// <summary>
			/// Dispose properly of the vertex when it becomes useless.
			/// Only called on collapse, so it does not remove itself from the pq.
			/// </summary>
			public void Dispose() {
				foreach (V vert in neighbor)
					vert.neighbor.Remove(this);
			}
		}

		/// <summary>
		/// Used to connect vertices in the graph and calculating curvature.
		/// </summary>
		class T : IDisposable
		{
			Polychop parent;
			public V[] vert { get; set; }
			public Vec3 normal { get; set; }
			/// <summary>
			/// Creates a triangle, which is basicly a V[3] with a extra few functionalities.
			/// Counter-clockwise vertex order expected.
			/// </summary>
			/// <param name="firstVertex">First vertex</param>
			/// <param name="secondVertex">Second</param>
			/// <param name="thirdVertex">Third</param>
			/// <param name="p"></param>
			public T(V firstVertex, V secondVertex, V thirdVertex, Polychop p) {
				parent = p;
				vert = new V[] { firstVertex, secondVertex, thirdVertex };
				vert[0].face.Add(this);
				vert[1].face.Add(this);
				vert[2].face.Add(this);
				vert[0].neighbor.Add(vert[1]);
				vert[0].neighbor.Add(vert[2]);
				vert[1].neighbor.Add(vert[0]);
				vert[1].neighbor.Add(vert[2]);
				vert[2].neighbor.Add(vert[0]);
				vert[2].neighbor.Add(vert[1]);
				ComputeNormal();
			}
			
			/// <summary>
			/// Replaces a vertex. Might result in disposal of this triangle.
			/// </summary>
			/// <param name="old">Vertex to replace.</param>
			/// <param name="replacement">Vertex to replace with.</param>
			public void ReplaceVertex(V old, V replacement) {
				// Replace
				vert[0] = vert[0] == old ? replacement : vert[0];
				vert[1] = vert[1] == old ? replacement : vert[1];
				vert[2] = vert[2] == old ? replacement : vert[2];
				// Update graph
				old.face.Remove(this);
				replacement.face.Add(this);
				foreach (V vert in vert) {
					old.RemoveIfNonNeighbor(vert);
					vert.RemoveIfNonNeighbor(old);
				}
				vert[0].neighbor.Add(vert[1]);
				vert[0].neighbor.Add(vert[2]);
				vert[1].neighbor.Add(vert[0]);
				vert[1].neighbor.Add(vert[2]);
				vert[2].neighbor.Add(vert[0]);
				vert[2].neighbor.Add(vert[1]);
				// Recompute normal after change!
				ComputeNormal();
			}
			/// <summary>
			/// Returns true if the given vertex is required in order to draw this triangle.
			/// </summary>
			/// <param name="vert">Vertex.</param>
			/// <returns></returns>
			public bool HasVertex(V vert) { return this.vert.Contains(vert); }
			/// <summary>
			/// Computes the normalized vector of the face direction.
			/// </summary>
			public void ComputeNormal() { normal = Vec3.NormalizedCrossProduct(vert[1].position - vert[0].position, vert[2].position - vert[1].position); }
			/// <summary>
			/// Dispose of this triangle correctly.
			/// Triangles auto-dispose if they are invalid.
			/// </summary>
			public void Dispose() {
				// Disconnect from graph
				foreach (V v in vert)
					v.face.Remove(this);
				vert[0].RemoveIfNonNeighbor(vert[1]);
				vert[0].RemoveIfNonNeighbor(vert[2]);
				vert[1].RemoveIfNonNeighbor(vert[0]);
				vert[1].RemoveIfNonNeighbor(vert[2]);
				vert[2].RemoveIfNonNeighbor(vert[0]);
				vert[2].RemoveIfNonNeighbor(vert[1]);
				// Remove from arrays
				parent.TrianglesRemove(this);
			}
		}
	}
}
