using F7s.Utility.Geometry.Double;
using Stride.Core.Mathematics; using F7s.Utility.Geometry.Double;
using System; using F7s.Utility.Geometry.Double;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace F7s.Utility.Mescherei {
    public class Triangle {

        public readonly Graph Mesch;

        public int Index;
        public List<Vertex> Vertices;
        public List<Edge> Edges = new List<Edge>();

        private Vector3 faceNormal;

        private static int nextNameIndex = 1;
        public readonly string name = "T" + nextNameIndex++;

        public bool Deleted { get; private set; }
        public bool Dirty { get; private set; } = true;

        public Triangle (Graph mesch, Vertex v0, Vertex v1, Vertex v2) {

            Debug.Assert(!Mathematik.ApproximatelyEqual(v0.Position, v1.Position, 0.0001f));
            Debug.Assert(!Mathematik.ApproximatelyEqual(v1.Position, v2.Position, 0.0001f));
            Debug.Assert(!Mathematik.ApproximatelyEqual(v2.Position, v0.Position, 0.0001f));

            Mesch = mesch;
            Vertices = new List<Vertex>() { v0, v1, v2 };
            Vertices.ForEach(v => v.AddTriangle(this));
            Debug.Assert(0 == Edges.Count);
            mesch.GenerateEdge(v0, v1, this);
            Debug.Assert(1 == Edges.Count);
            mesch.GenerateEdge(v1, v2, this);
            Debug.Assert(2 == Edges.Count);
            mesch.GenerateEdge(v2, v0, this);
            Debug.Assert(3 == Edges.Count);
            SetFacing(Mesch.Facing.relativeTo, Mesch.Facing.direction);
        }

        public void VerticesHaveMoved () {
            MarkDirty();
        }

        public void MarkDirty () {
            if (Dirty) {
                return;
            }
            Dirty = true;
            Vertices.ForEach(v => v.MarkDirty());
            Mesch.MarkDirty();
        }

        public void CleanUp () {
            if (Dirty) {
                Dirty = false;
                CalculateFaceNormal();
            }
        }

        public override string ToString () {
            return (Deleted ? "DEL-" : "") + name + ":" + V0 + "-" + V1 + "-" + V2;
        }

        public void AddVertex (Vertex v) {
            Vertices.Add(v);
        }

        public bool ContainsVertex (Vertex v) {
            return Vertices.Contains(v);
        }

        public void Delete () {
            if (Deleted) {
                return;
            }
            Deleted = true;
            Mesch.RemoveTriangle(this);
            Vertices.ForEach(v => v.RemoveTriangle(this));
            Debug.Assert(3 == Edges.Count);
            Edges.ForEach(e => e.RemoveTriangle(this));
            Vertices.Clear();
            Edges.Clear();
            Vertices = null;
            Edges = null;
        }

        public IEnumerable<int> VertexIndices () {
            IEnumerable<int> vertexIndices = Vertices.Select(v => v.Index);
            return vertexIndices;
        }

        public void RemoveVertex (Vertex vertex) {
            Vertices.Remove(vertex);
        }

        public void Divide (bool spherical = false) {

            Edge e01 = V0.EdgeTo(V1);
            Edge e12 = V1.EdgeTo(V2);
            Edge e20 = V2.EdgeTo(V0);

            Vertex v01 = e01.Split(spherical);
            Vertex v12 = e12.Split(spherical);
            Vertex v20 = e20.Split(spherical);

            Triangle t0 = Mesch.AddTriangle(V0, v01, v20);
            Triangle t1 = Mesch.AddTriangle(V1, v01, v12);
            Triangle t2 = Mesch.AddTriangle(V2, v12, v20);
            Triangle tc = Mesch.AddTriangle(v01, v12, v20);

            if (!spherical) {
                Debug.Assert(Mathematik.IsEqualApprox(SurfaceArea(), t0.SurfaceArea() + t1.SurfaceArea() + t2.SurfaceArea() + tc.SurfaceArea(), 0.001));
            }

            Delete();
        }

        public float SurfaceArea () {
            return Mathematik.TriangleSurfaceArea(V0.Position, V1.Position, V2.Position);
        }

        public void AddEdge (Edge edge) {
            if (!Edges.Contains(edge)) {
                Edges.Add(edge);
            }
        }

        public void RemoveEdge (Edge edge) {
            Delete();
        }

        public Vertex V0 => Vertices[0];
        public Vertex V1 => Vertices[1];
        public Vertex V2 => Vertices[2];

        public void SetFacing (Vector3 shapeCentre, Facing.Direction desiredDirection) {
            if (desiredDirection == Facing.Direction.TwoSided) {
                throw new Exception(message: "Cannot sensibly invert two-sided triangles.");
            }

            bool inversionRequired = (desiredDirection == Facing.Direction.Outward && FacesInward(shapeCentre))
                                  || (desiredDirection == Facing.Direction.Inward && FacesOutward(shapeCentre));

            if (inversionRequired) {
                Invert();
            }
        }
        public bool FacesOutward (Vector3 shapeCentre) {
            return CurrentFacing(shapeCentre) == Facing.Direction.Outward;
        }
        public bool FacesInward (Vector3 shapeCentre) {
            return CurrentFacing(shapeCentre) == Facing.Direction.Inward;
        }

        public Facing.Direction CurrentFacing (Vector3 shapeCentre) {
            Vector3 triangleCenter = CartesianCenter();
            Vector3 surfaceNormal = GetFaceNormal();
            Vector3 shapeToTriangleVector = triangleCenter - shapeCentre;

            double angle = Vector3d.Angle(from: Mathematik.Normalize(shapeToTriangleVector), to: surfaceNormal); // Using double-precision angle calculation works better for very small angles.

            if (angle < 0) {
                throw new Exception("Angle " + angle + " < 0.");
            }

            if (angle > 90.0) {
                return Facing.Direction.Inward;
            } else if (angle < 90) {
                return Facing.Direction.Outward;
            } else {
                throw new Exception("Angle == " + angle + ", given triangle center " + triangleCenter + ", shape center " + shapeCentre + ", difference vector " + shapeToTriangleVector + ", face normal " + surfaceNormal + ".");
            }
        }

        public Vector3 CartesianCenter () {
            return Mathematik.Average(Vertices.Select(v => v.Position));
        }

        private Vector3 CalculateFaceNormal () {
            Vector3 pos0 = V0.Position;
            Vector3 pos1 = V1.Position;
            Vector3 pos2 = V2.Position;

            Vector3 vector1 = pos1 - pos0;
            Vector3 vector2 = pos2 - pos0;
            Vector3 cross = Mathematik.Cross(vector2, vector1);
            Vector3 faceNormal = Mathematik.Normalize(cross);

            Debug.Assert(Vector3.Zero != faceNormal);
            this.faceNormal = faceNormal;
            return faceNormal;
        }

        public Vector3 GetFaceNormal () {
            CleanUp();
            return faceNormal;
        }

        public void Invert () {
            Console.WriteLine("Inverting " + this + ".");
            Mesch.MarkDirty();
            Vertices.Reverse();
            faceNormal *= -1;
        }

    }

}
