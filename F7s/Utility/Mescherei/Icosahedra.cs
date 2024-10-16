using Stride.Core.Mathematics;
using System;
using System.Diagnostics;

namespace F7s.Utility.Mescherei {

    public static class Icosahedra {
        public static bool forceRegenerate = false;

        private static string Filename (int resolution) {
            return "Icosahedron-" + resolution;
        }

        private static string Directory () {
            return "";
        }

        public static ArrayMesh IcosphereMesh (int resolution = 0, float? radius = 0.5f, Vector3? rotation = null) {
            return Icosphere(false, true, resolution, radius, rotation).Item2;
        }

        public static Graph IcosphereGraph (int resolution = 0, float? radius = 0.5f, Vector3? rotation = null) {
            return Icosphere(true, true, resolution, radius, rotation).Item1;
        }

        private static (Graph, ArrayMesh) Icosphere (bool generateGraph, bool generateArrayMesh, int resolution = 0, float? radius = 0.5f, Vector3? rotation = null) {
            Stoppuhr stoppuhr = new Stoppuhr();

            Graph graph = new Graph();


            ArrayMesh arrayMesh = LoadFromFile(resolution);
            bool loaded = false;
            if (!forceRegenerate && arrayMesh != null) {
                if (generateGraph == false && radius == 0.5f && rotation == null) {
                    return (graph, arrayMesh);
                }
                graph.FromArrayMesh(arrayMesh);
                loaded = true;
            } else {
                graph.SetFacing(new Facing(Facing.Direction.Outward, Vector3.Zero), false);

                float unit = 1.0f;
                float t = (1.0f + MathF.Sqrt(5.0f)) / 2.0f;

                Vertex GenerateVertex (float x, float y, float z) {
                    return graph.AddVertex(x, y, z);
                }

                Vertex v0 = GenerateVertex(x: -unit, y: t, z: 0);
                Vertex v1 = GenerateVertex(x: unit, y: t, z: 0);
                Vertex v2 = GenerateVertex(x: -unit, y: -t, z: 0);
                Vertex v3 = GenerateVertex(x: unit, y: -t, z: 0);

                Vertex v4 = GenerateVertex(x: 0, y: -unit, z: t);
                Vertex v5 = GenerateVertex(x: 0, y: unit, z: t);
                Vertex v6 = GenerateVertex(x: 0, y: -unit, z: -t);
                Vertex v7 = GenerateVertex(x: 0, y: unit, z: -t);

                Vertex v8 = GenerateVertex(x: t, y: 0, z: -unit);
                Vertex v9 = GenerateVertex(x: t, y: 0, z: unit);
                Vertex v10 = GenerateVertex(x: -t, y: 0, z: -unit);
                Vertex v11 = GenerateVertex(x: -t, y: 0, z: unit);

                Triangle AddIndex (Vertex v1, Vertex v2, Vertex v3) {
                    return graph.AddTriangle(v1, v2, v3);
                }

                // 5 faces around point 0
                AddIndex(v1: v0, v2: v11, v3: v5);
                AddIndex(v1: v0, v2: v5, v3: v1);
                AddIndex(v1: v0, v2: v1, v3: v7);
                AddIndex(v1: v0, v2: v7, v3: v10);
                AddIndex(v1: v0, v2: v10, v3: v11);

                // 5 adjacent faces
                AddIndex(v1: v1, v2: v5, v3: v9);
                AddIndex(v1: v5, v2: v11, v3: v4);
                AddIndex(v1: v11, v2: v10, v3: v2);
                AddIndex(v1: v10, v2: v7, v3: v6);
                AddIndex(v1: v7, v2: v1, v3: v8);

                // 5 faces around point 3
                AddIndex(v1: v3, v2: v9, v3: v4);
                AddIndex(v1: v3, v2: v4, v3: v2);
                AddIndex(v1: v3, v2: v2, v3: v6);
                AddIndex(v1: v3, v2: v6, v3: v8);
                AddIndex(v1: v3, v2: v8, v3: v9);

                // 5 adjacent faces
                AddIndex(v1: v4, v2: v9, v3: v5);
                AddIndex(v1: v2, v2: v4, v3: v11);
                AddIndex(v1: v6, v2: v2, v3: v10);
                AddIndex(v1: v8, v2: v6, v3: v7);
                AddIndex(v1: v9, v2: v8, v3: v1);

                foreach (Edge e in graph.Edges) {
                    Debug.Assert(2 == e.TrianglesCount());
                }

                if (resolution > 0) {
                    graph.LoopSubdivide(resolution, true);
                }

                foreach (Edge e in graph.Edges) {
                    Debug.Assert(2 == e.TrianglesCount());
                }
            }

            if (generateArrayMesh && arrayMesh == null) {
                throw new NotImplementedException()// arrayMesh = graph.ToArrayMesh();
                SaveToFile(resolution, arrayMesh);
            }

            stoppuhr.Print("Generated Icosphere-" + resolution + " from " + (loaded ? "Disk" : "Scratch") + " in", 0.05f);

            if (rotation != null) {
                graph.RotateAllVertices(rotation.Value);
            }

            if (radius.HasValue) {
                graph.SetRadiusForAllVertices(radius.Value);
            }

            return (graph, arrayMesh);
        }


        private static ArrayMesh LoadFromFile (int resolution) {
            return ArrayMeshes.Load(Filename(resolution), Directory(), true);
        }

        private static void SaveToFile (int resolution, ArrayMesh mesh) {
            ArrayMeshes.Save(mesh, Filename(resolution), Directory(), ResourceSaver.SaverFlags.None, true);
        }
    }
}
