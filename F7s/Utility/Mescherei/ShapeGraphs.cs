using Stride.Core.Mathematics; using F7s.Utility.Geometry.Double;
using System; using F7s.Utility.Geometry.Double;

namespace F7s.Utility.Mescherei {
    public static class ShapeGraphs {

        public static Graph TetrahedronGraph () {
            Graph graph = new Graph();
            Vertex v0 = graph.AddVertex(new Vector3(0, MathF.Sqrt(2.0f / 3.0f), 0));
            Vertex v1 = graph.AddVertex(new Vector3(1 / MathF.Sqrt(3.0f), 0, 0));
            Vertex v2 = graph.AddVertex(new Vector3(-1.0f / (2.0f * MathF.Sqrt(3)), 0, -0.5f));
            Vertex v3 = graph.AddVertex(new Vector3(-1.0f / (2.0f * MathF.Sqrt(3)), 0, 0.5f));
            graph.AddTriangle(v0, v1, v2);
            graph.AddTriangle(v0, v1, v3);
            graph.AddTriangle(v0, v2, v3);
            graph.AddTriangle(v1, v2, v3);
            return graph;
        }

        public static Graph TriangleGraph (Vector3 a, Vector3 b, Vector3 c, int resolution = 0) {

            Graph graph = new Graph();

            graph.AddTriangle(a, b, c);

            if (resolution > 0) {
                graph.LoopSubdivide(resolution);
            }

            return graph;
        }
    }
}
