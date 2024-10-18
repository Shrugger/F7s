using F7s.Utility.Geometry;
using Stride.Core.Mathematics;
using Stride.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace F7s.Utility.Mescherei
{
    public class Vertex {

        public readonly Graph Mesch;

        public int Index = -1;
        public Vector3 Position { get; private set; }
        public PolarCoordinates Coordinates { get; private set; }
        private Vector3 Normal;
        public Vector2? UV { get; private set; }
        public Farbe Color { get; private set; }

        private static int nextNameIndex = 1;
        public readonly string name = "V" + nextNameIndex++;

        private List<Vertex> neighbors = new List<Vertex>();
        private List<Triangle> triangles = new List<Triangle>();
        private List<Edge> edges = new List<Edge>();

        public List<Edge> Edges { get { return edges; } }

        public bool Deleted { get; private set; }
        private bool dirty = true;


        public void MarkDirty () {
            if (dirty) {
                return;
            }
            Mesch.MarkDirty();
            dirty = true;
            triangles.ForEach(t => t.MarkDirty());

            foreach (Triangle triangle in triangles) {
                triangle.VerticesHaveMoved();
            }
        }

        public void CleanUp () {
            if (dirty) {
                dirty = false;
                RecalculateNormal();
            }
        }

        public override string ToString () {
            return (Deleted ? "DEL-" : "") + name;
        }

        public Vertex (Graph mesch) {
            Mesch = mesch;
            Color = Farbe.magenta;
        }

        public void Delete () {
            if (Deleted) {
                return;
            }
            Deleted = true;
            triangles.ForEach(t => t.RemoveVertex(this));
            Mesch.RemoveVertex(this);
            triangles.Clear();
            edges.ForEach(e => e.RemoveVertex(this));
        }

        public List<Vertex> Neighbors () {
            return neighbors;
        }

        public void AddTriangle (Triangle t) {
            triangles.Add(t);
            t.Vertices.ForEach(v => RegisterNeighbor(v));
        }

        private void RegisterNeighbor (Vertex other) {
            if (this == other) {
                return;
            }
            neighbors.Add(other);
            other.neighbors.Add(this);
        }

        private void DeregisterNeighbor (Vertex other) {
            neighbors.Remove(other);
            other.neighbors.Remove(this);
        }

        public void RemoveTriangle (Triangle t) {
            triangles.Remove(t);
            foreach (Vertex v in t.Vertices) {
                if (!SharesTriangleWith(v)) {
                    DeregisterNeighbor(v);
                }
            }
        }

        public bool Neighbors (Vertex other) {
            return neighbors.Contains(other);
        }

        public bool SharesTriangleWith (Vertex other) {
            return triangles.Any(t => t.ContainsVertex(other));
        }


        public Vector3 RecalculateNormal () {
            Vector3 normal = Vector3.Zero;
            foreach (Triangle triangle in triangles) {
                Vector3 triangleNormal = triangle.GetFaceNormal();
                Debug.Assert(Vector3.Zero != triangleNormal);
                normal += triangleNormal;
            }
            normal = Mathematik.Normalize(normal);
            Debug.Assert(Vector3.Zero != normal);
            Normal = normal;
            return normal;
        }

        public void AddEdge (Edge e) {
            if (edges.Contains(e)) {
                return;
            }
            edges.Add(e);
            e.Other(this).AddEdge(e);
        }

        public void RemoveEdge (Edge e) {
            if (edges.Contains(e)) {
                edges.Remove(e);
                e.Other(this).RemoveEdge(e);
            }
        }

        public static Edge SharedEdge (Vertex a, Vertex b) {
            return a.EdgeTo(b);
        }

        public Edge EdgeTo (Vertex other) {
            return edges.FirstOrDefault(edge => edge.Contains(other));
        }

        public void SetRadius (float radius) {
            MarkDirty();
            if (radius <= 0 || !float.IsFinite(radius)) {
                throw new Exception();
            }
            SetCoordinates(Coordinates.SetRadialDistance(radius));
        }
        public float GetRadius () {
            return (float) Coordinates.radialDistance;
        }

        public void ShiftRadius (float addition) {
            SetRadius(GetRadius() + addition);
        }

        public void SetColor (Farbe color) {
            MarkDirty();
            Color = color;
        }

        public void SetNormal (Vector3 normal) {
            MarkDirty();
            Normal = normal;
        }

        public void SetUV (Vector2? uv) {
            MarkDirty();
            UV = uv;
        }

        public void SetPosition (Vector3 position) {
            MarkDirty();
            Position = position;
            Coordinates = PolarCoordinates.FromCartesian(position);
        }

        public void SetCoordinates (PolarCoordinates coordinates) {
            MarkDirty();
            Coordinates = coordinates;
            Position = coordinates.ToVector3();
        }

        public int EdgeCount () {
            return edges.Count;
        }

        public static float Distance (Vertex a, Vertex b) {
            return Vector3.Distance(a.Position, b.Position);
        }

        public Vector3 GetNormal () {
            CleanUp();
            return Normal;
        }

        public Vector2 GetUV () {
            if (UV.HasValue) {
                return UV.Value;
            } else {
                return Alea.Vector2(0, 1);
            }
        }

        public VertexPositionNormalColor ToStride () {
            return new VertexPositionNormalColor(this.Position, this.Normal, this.Color);
        }
    }

}
