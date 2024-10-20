using F7s.Utility.Mescherei;
using Stride.Graphics;
using Stride.Rendering;
using Buffer = Stride.Graphics.Buffer;

namespace F7s.Engine {
    public class Mesch {
        public readonly Model model;
        public readonly Mesh mesh;

        public Mesch () {
            model = new Model();
            mesh = new Mesh();
            model.Meshes.Add(mesh);
            mesh.Draw = new MeshDraw();
        }

        public static Mesch FromGraph (Graph graph, GraphicsDevice graphicsDevice, GraphicsResourceUsage graphicsResourceUsage) {
            Mesch mesch = new Mesch();
            mesch.mesh.Name = "Graph Mesch";

            MeshDraw draw = mesch.mesh.Draw;

            draw.PrimitiveType = PrimitiveType.TriangleList;
            int indicesCount = graph.TriangleCount * 3;
            draw.DrawCount = indicesCount;

            VertexPositionNormalColor[] vertices = graph.GetVertexArray();
            int[] indices = graph.GetIndexArray();

            Buffer vertexBuffer = Buffer.Vertex.New(graphicsDevice, vertices, graphicsResourceUsage);
            Buffer indexBuffer = Buffer.Index.New(graphicsDevice, indices, graphicsResourceUsage);

            draw.VertexBuffers = new VertexBufferBinding[] { new VertexBufferBinding(vertexBuffer, VertexPositionNormalColor.Layout, vertexBuffer.ElementCount) };
            draw.IndexBuffer = new IndexBufferBinding(indexBuffer, true, indexBuffer.ElementCount);

            return mesch;
        }
    }

}
