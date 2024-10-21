namespace Tests;

public class MeschereiTests {
    [SetUp]
    public void Setup () {
    }

    [Test]
    public void TestIcosahedra () {

        Graph graph = new Graph();

        graph.SetFacing(new Facing(Facing.Direction.Outward, Vector3.Zero), false);

        float unit = 1.0f;
        float t = (1.0f + MathF.Sqrt(5.0f)) / 2.0f;

        Vertex GenerateVertex (float x, float y, float z) {
            return graph.AddVertex(x, y, z);
        }

        Vertex v0 = GenerateVertex(x: -unit, y: t, z: 0);
        Vertex v1 = GenerateVertex(x: unit, y: t, z: 0);

        Debug.Assert(!Mathematik.ApproximatelyEqual(v0.Position, v1.Position, 0.0001f));

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

        Triangle AddIndex (Vertex a, Vertex b, Vertex c) {
            return graph.AddTriangle(a, b, c);
        }

        // 5 faces around point 0
        AddIndex(a: v0, b: v11, c: v5);
        AddIndex(a: v0, b: v5, c: v1);
        AddIndex(a: v0, b: v1, c: v7);
        AddIndex(a: v0, b: v7, c: v10);
        AddIndex(a: v0, b: v10, c: v11);

        // 5 adjacent faces
        AddIndex(a: v1, b: v5, c: v9);
        AddIndex(a: v5, b: v11, c: v4);
        AddIndex(a: v11, b: v10, c: v2);
        AddIndex(a: v10, b: v7, c: v6);
        AddIndex(a: v7, b: v1, c: v8);

        // 5 faces around point 3
        AddIndex(a: v3, b: v9, c: v4);
        AddIndex(a: v3, b: v4, c: v2);
        AddIndex(a: v3, b: v2, c: v6);
        AddIndex(a: v3, b: v6, c: v8);
        AddIndex(a: v3, b: v8, c: v9);

        // 5 adjacent faces
        AddIndex(a: v4, b: v9, c: v5);
        AddIndex(a: v2, b: v4, c: v11);
        AddIndex(a: v6, b: v2, c: v10);
        AddIndex(a: v8, b: v6, c: v7);
        AddIndex(a: v9, b: v8, c: v1);

        Assert.Pass();
    }
}