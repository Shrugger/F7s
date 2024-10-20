using F7s.Engine;
using F7s.Engine.InputHandling;
using F7s.Modell.Abstract;
using F7s.Modell.Handling.PlayerControllers;
using F7s.Modell.Populators;
using F7s.Modell.Terrains;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Stride.Engine;
using Stride.Physics;

namespace F7s.Main {

    public class MainSync : SyncScript {

        private readonly bool simulationStarted = false;

        private Populator populator;

        public override void Start () {

            Terrain terrain = new Terrain("Tiny Planet", 1, 2, Entity, new PlanetologyData(1, 1, 1, true, true, 5)); // TODO: Reactivate after child's play.
            Engine.Mesch terrainMesch = terrain.Render(GraphicsDevice, Stride.Graphics.GraphicsResourceUsage.Default);
            Assert.IsNotNull(terrainMesch);

            populator = new PerduePopulator();
        }

        private void PrePhysicsUpdate (Simulation sender, float tick) {

        }

        private void PostPhysicsUpdate (Simulation sender, float tick) {

        }

        private void InitializeSimulationUpdateListeners () {
            if (simulationStarted) {
                return;
            }
            Simulation simulation = this.GetSimulation();
            if (simulation != null) {
                simulation.PreTick += PrePhysicsUpdate;
                simulation.PostTick += PostPhysicsUpdate;
            }
        }

        public override void Update () {
            InitializeSimulationUpdateListeners();

            InputHandler.Update();

            double deltaTime = Zeit.DeltaTimeSeconds();
            Origin.Update(deltaTime);

            Player.Update(deltaTime);
            Kamera.Update(deltaTime);

            if (!Zeit.Paused) {
                GameEntity.OnEngineUpdate(1.0, false);
                populator?.Update(deltaTime);
            }
        }
    }
}
