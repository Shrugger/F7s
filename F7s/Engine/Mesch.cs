using F7s.Mains;
using F7s.Utility.Mescherei;
using Stride.CommunityToolkit.Rendering.Utilities;
using Stride.Core.Mathematics;
using Stride.Graphics;
using Stride.Rendering;
using Buffer = Stride.Graphics.Buffer;

namespace F7s.Engine {
    public class Mesch {
        public readonly Model model;
        public readonly Mesh mesh;

        public Mesch () {
            model = new Model {
                StrideUtilities.PlaceholderMaterial(),

            };
            mesh = new Mesh() {
                MaterialIndex = 0
            };
            model.Meshes.Add(mesh);
        }

        public static Mesch FromGraph (Graph graph, GraphicsResourceUsage graphicsResourceUsage) {
            Mesch mesch = new Mesch();
            mesch.mesh.Name = "Graph Mesch";

            mesch.mesh.Draw = GenerateDraw();
            MeshDraw GenerateDraw (bool useCommunityToolkit = true) {
                if (useCommunityToolkit) {
                    using MeshBuilder meshBuilder = new MeshBuilder();
                    meshBuilder.WithPrimitiveType(PrimitiveType.TriangleList);
                    meshBuilder.WithIndexType(IndexingType.Int32);
                    int position = meshBuilder.WithPosition<Vector3>();
                    int color = meshBuilder.WithColor<Color>();
                    for (int index = 0; index < graph.VertexCount; index++) {
                        Vertex v = graph.GetVertex(index);
                        meshBuilder.AddVertex();
                        meshBuilder.SetElement(position, v.Position);
                        meshBuilder.SetElement(color, v.Color.ToStrideColor());
                        v.Index = index;
                    }
                    foreach (Triangle t in graph.Triangles) {
                        foreach (int index in t.VertexIndices()) {
                            meshBuilder.AddIndex(index);
                        }
                    }

                    MeshDraw draw = meshBuilder.ToMeshDraw(MainSync.GraphicsDevice);
                    return draw;

                } else {

                    MeshDraw draw = new MeshDraw();
                    mesch.mesh.Draw = draw;

                    draw.PrimitiveType = PrimitiveType.TriangleList;
                    int indicesCount = graph.TriangleCount * 3;
                    draw.DrawCount = indicesCount;

                    VertexPositionNormalColor[] vertices = graph.GetVertexArray();
                    int[] indices = graph.GetIndexArray();

                    Buffer vertexBuffer = Buffer.Vertex.New(MainSync.GraphicsDevice, vertices, graphicsResourceUsage);
                    Buffer indexBuffer = Buffer.Index.New(MainSync.GraphicsDevice, indices, graphicsResourceUsage);

                    draw.VertexBuffers = new VertexBufferBinding[] { new VertexBufferBinding(vertexBuffer, VertexPositionNormalColor.Layout, vertexBuffer.ElementCount) };
                    draw.IndexBuffer = new IndexBufferBinding(indexBuffer, true, indexBuffer.ElementCount);

                    return draw;
                }
            }

            return mesch;
        }
    }
}
