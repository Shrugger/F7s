﻿using Stride.Core.Mathematics;
using Stride.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace F7s.Utility.Mescherei {

    public class Graph {

        private List<Vertex> vertices = new List<Vertex>();
        private List<Triangle> triangles = new List<Triangle>();
        private List<Edge> edges = new List<Edge>();
        public List<Vertex> Vertices { get { return vertices; } }
        private VertexPositionNormalColor[] vertexArray = null;
        private int[] indexArray = null;

        public List<Triangle> Triangles { get { return triangles; } }
        public List<Edge> Edges { get { return edges; } }

        public Facing Facing { get; private set; }

        public bool Dirty { get; private set; } = false;
        public bool Deleted { get; private set; } = false;
        public int VertexCount => vertices.Count;
        public int EdgeCount => edges.Count;
        public int TriangleCount => triangles.Count;

        public Graph () { }

        public Graph (Action<Graph> shapeGenerator = null) : this() {
            if (shapeGenerator != null) {
                shapeGenerator.Invoke(this);
            }
        }

        public VertexPositionNormalColor[] GetVertexArray () {
            if (vertexArray == null) {
                vertexArray = BakeVertexArray();
                if (vertexArray == null) {
                    throw new Exception();
                }
            }
            return vertexArray;
        }
        private VertexPositionNormalColor[] BakeVertexArray () {
            return Vertices.ConvertAll(v => v.ToStrideVertexPositionNormalColor()).ToArray();
        }
        public int[] GetIndexArray () {
            if (indexArray == null) {
                indexArray = BakeIndexArray();
                if (indexArray == null) {
                    throw new Exception();
                }
            }
            return indexArray;
        }
        private int[] BakeIndexArray () {
            List<int> indices = new List<int>();
            foreach (Triangle t in triangles) {
                indices.AddRange(t.VertexIndices());
            }
            int[] indexArray = indices.ToArray();
            return indexArray;
        }

        public override string ToString () {
            return (Deleted ? "DELETED " : "") + "Graph " + "V" + vertices.Count + " T" + triangles.Count;
        }

        public void MarkDirty () {
            if (!Dirty) {
                Dirty = true;
            }
        }

        public void MarkClean () {
            Dirty = false;
        }

        public void Validate () {
            if (Vertices.Count == 0) {
                throw new Exception();
            }
            if (Triangles.Count == 0) {
                throw new Exception();
            }
            if (Edges.Count == 0) {
                throw new Exception();
            }
            if (vertices.All(v => v.Color.A < 0.1f)) {
                throw new Exception();
            }
        }

        public void LoopSubdivide (int recursions = 1, bool spherical = false) {

            if (recursions < 1) {
                return;
            }

            int oldVertexCount = Vertices.Count;
            int oldTriangleCount = Triangles.Count;
            int oldEdgeCount = Edges.Count;
            Collections.SafeForEach(Triangles, t => t.Divide(spherical));

            Debug.Assert(oldTriangleCount * 4 == Triangles.Count);
            Debug.Assert(oldEdgeCount * 4 == Edges.Count);
            Debug.Assert(oldVertexCount + oldEdgeCount == Vertices.Count);

            LoopSubdivide(recursions - 1, spherical);
        }

        public void SetFacing (Facing facing, bool updateTriangles = true) {
            MarkDirty();
            Facing = facing;
            if (updateTriangles) {
                Triangles.ForEach(t => t.SetFacing(facing.relativeTo, facing.direction));
            }
        }

        public bool Populated () {
            return vertices.Count > 0 || edges.Count > 0 || triangles.Count > 0;
        }


        public Vertex AddVertex (float x, float y, float z) {
            MarkDirty();
            Vector3 v = new Vector3(x, y, z);
            return AddVertex(v);
        }

        public Vertex AddVertex (Vector3 v) {
            MarkDirty();
            return AddVertex(v, null, null, new Farbe(1, 1, 1, 0.5f));
        }

        public Vertex AddVertex (Vector3 v, Vector3? normal, Vector2? uv, Farbe color) {
            MarkDirty();
            Vertex vertex = new Vertex(this);
            vertex.SetNormal(normal ?? default);
            vertex.SetUV(uv);
            vertex.SetPosition(v);
            vertex.SetColor(color);
            Vertices.Add(vertex);
            return vertex;
        }

        private void DeleteAllTriangles () {
            MarkDirty();
            while (Triangles.Count > 0) {
                Triangles.First().Delete();
            }
        }

        public Vertex GetVertex (int index) {
            return Vertices[index];
        }

        public Triangle AddTriangle (Vertex v0, Vertex v1, Vertex v2) {
            MarkDirty();
            Triangle t = new Triangle(this, v0, v1, v2);
            Triangles.Add(t);
            return t;
        }

        public Edge GenerateEdge (Vertex a, Vertex b, Triangle t) {
            Edge e = Vertex.SharedEdge(a, b);
            if (e == null) {
                e = new Edge(this, a, b, t);
                Edges.Add(e);
            }
            e.AddTriangle(t);
            return e;
        }

        public void RemoveVertex (Vertex v) {
            MarkDirty();
            Vertices.Remove(v);
            v.Delete();
        }

        public void RemoveTriangle (Triangle triangle) {
            MarkDirty();
            Triangles.Remove(triangle);
            triangle.Delete();
        }
        public void RotateAllVertices (Vector3 rotation) {
            ApplyToAllVertices(v => RotatedVertexPosition(v, rotation));
        }

        public Vector3 RotatedVertexPosition (Vector3 position, Vector3 rotation) {
            Matrix old = MM.Transformation(position, Quaternion.Identity);
            Matrix transformation = MM.Transformation(Vector3.Zero, MM.DegreesToQuaternion(rotation));
            Vector3 result = (transformation * old).TranslationVector;
            return result;
        }

        public void SetRadiusForAllVertices (float radius) {
            ApplyToAllVertices(v => v.SetRadius(radius));
        }

        public Vector3 AlteredVertexRadius (Vector3 vertex, float radius) {
            return MM.Normalize(vertex) * radius;
        }

        public void ApplyToAllVertices (Func<Vector3, Vector3> action) {
            MarkDirty();
            if (Vertices.Count == 0) {
                throw new Exception();
            }
            foreach (Vertex vertex in Vertices) {
                vertex.SetPosition(action(vertex.Position));
            }
        }

        public void ApplyToAllVertices (Action<Vertex> action) {
            MarkDirty();
            if (Vertices.Count == 0) {
                throw new Exception();
            }
            Vertices.ForEach(v => action(v));
        }

        public Vertex AddInterpolatedVertex (Vertex a, Vertex b, float weight = 0.5f, bool spherical = false) {
            Vector3 position;
            if (!spherical) {
                position = Vector3.Lerp(a.Position, b.Position, weight);
            } else {
                position = MM.Slerp(a.Position, b.Position, weight);
            }
            Vertex interpolated = AddVertex(position, null, null, Farbe.Lerp(a.Color, b.Color, weight));
            if (!spherical) {
                Debug.Assert(MM.ApproximatelyEqual(Vertex.Distance(a, b), Vertex.Distance(a, interpolated) + Vertex.Distance(interpolated, b), 0.001f));
            }
            return interpolated;
        }

        public void Delete () {
            if (Deleted) {
                return;
            }
            Deleted = true;
            vertices.Clear();
            vertices = null;
            edges.Clear();
            edges = null;
            triangles.Clear();
            triangles = null;
        }

        public void AddTriangle (Vector3 a, Vector3 b, Vector3 c) {
            AddTriangle(AddVertex(a), AddVertex(b), AddVertex(c));
        }

        public void ColorAllVertices (Farbe color) {
            ApplyToAllVertices(v => v.SetColor(color));
        }

        public void RemoveEdge (Edge edge) {
            edges.Remove(edge);
        }
    }

}
