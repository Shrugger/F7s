using F7s.Utility.Geometry;
using Stride.Core.Mathematics;
using System;
using System.Collections.Generic;

namespace F7s.Utility.Mescherei {
    public class Edge {

        public readonly Graph Mesch;

        private readonly List<Vertex> vertices;
        private readonly List<Triangle> triangles;

        public bool IsSplit { get; private set; }
        public Vertex SplitVertex { get; private set; }

        private static int nextNameIndex = 1;
        public readonly string name = "E" + nextNameIndex++;

        public bool Deleted { get; private set; }

        public Edge (Graph mesch, Vertex a, Vertex b, Triangle t) {
            Mesch = mesch;
            vertices = new List<Vertex>() { a, b };
            vertices.ForEach(v => v.AddEdge(this));
            triangles = new List<Triangle>() { t };
            t.AddEdge(this);
        }

        public override string ToString () {
            return (Deleted ? "DEL-" : "") + name + ":" + A + "-" + B;
        }

        public bool Contains (Vertex v) {
            return vertices.Contains(v);
        }

        public void AddTriangle (Triangle t) {
            if (!triangles.Contains(t)) {
                triangles.Add(t);
                t.AddEdge(this);
            }
        }

        public void RemoveTriangle (Triangle t) {
            triangles.Remove(t);
            if (triangles.Count < 1) {
                Delete();
            }
        }

        public void Delete () {
            if (Deleted) {
                return;
            } else {
                Deleted = true;
                Mesch.RemoveEdge(this);
                vertices.ForEach(v => v.RemoveEdge(this));
                vertices.Clear();
                triangles.Clear();
            }
        }

        public Vertex A => vertices[0];
        public Vertex B => vertices[1];

        public Vertex Split (bool spherical = false) {
            if (IsSplit) {
                return SplitVertex;
            }
            if (vertices.Count != 2) {
                throw new Exception();
            }
            SplitVertex = Mesch.AddInterpolatedVertex(A, B, 0.5f, spherical);
            IsSplit = true;

            return SplitVertex;
        }

        public bool Contains (Vertex a, Vertex b) {
            return Contains(a) && Contains(b);
        }

        public Vertex Other (Vertex vertex) {
            if (vertex == A) {
                return B;
            } else if (vertex == B) {
                return A;
            } else {
                throw new Exception();
            }
        }

        public int TrianglesCount () {
            return triangles.Count;
        }

        public void RemoveVertex (Vertex vertex) {
            Delete();
        }

        public double Length (bool spherical = false, float? projectionRadius = null) {
            if (spherical) {
                PolarCoordinatesD a = A.Coordinates;
                PolarCoordinatesD b = B.Coordinates;
                return (float) PolarCoordinatesD.PolarDistanceDouble(a, b, projectionRadius ?? 1.0f);
            } else {
                return Vector3.Distance(A.Position, B.Position);
            }
        }
    }

}
