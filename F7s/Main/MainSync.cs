using Stride.Engine;
using Stride.Physics;

namespace F7s.Main {

    public class MainSync : SyncScript {

        private readonly bool simulationStarted = false;


        public override void Start () {

            //    Terrain terrain = new Terrain("Tiny Planet", 1, 2, this.Entity, new PlanetologyData(1, 1, 1, true, true, 5)); // TODO: Reactivate after child's play.

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
        }
    }
}
