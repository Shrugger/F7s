using Stride.Engine;
using Stride.Extensions;
using Stride.Graphics.GeometricPrimitives;
using Stride.Physics;
using Stride.Rendering;

namespace F7s {

    public class MainSync : SyncScript {




        public override void Start () {


            Entity terrainEntity = new Entity();
            ModelComponent terrainModelComponent = terrainEntity.GetOrCreate<ModelComponent>();
            Model terrainModel = new Model();
            terrainModelComponent.Model = terrainModel;

            Stride.Rendering.MeshDraw terrainMeshDraw = GeometricPrimitive.Cylinder.New(GraphicsDevice).ToMeshDraw();
            Mesh terrainMesh = new Mesh { Draw = terrainMeshDraw };
            terrainModel.Meshes.Add(terrainMesh);

            Simulation simulation = this.GetSimulation();
            simulation.PreTick += PrePhysicsUpdate;
            simulation.PostTick += PostPhysicsUpdate;
        }

        private void PrePhysicsUpdate (Simulation sender, float tick) {

        }

        private void PostPhysicsUpdate (Simulation sender, float tick) {

        }

        public override void Update () {

        }
    }
}
