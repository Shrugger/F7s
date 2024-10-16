using F7s.Modell.Terrains;
using F7s.Utility;

namespace F7s.Modell.Physical.Celestial {
    public class SolidPlanet : CelestialBody {
        private readonly int terrainResolution;
        private readonly PlanetologyData planetologyData;
        private Terrain terrain;

        public SolidPlanet (string name, double radius, Farbe color, int terrainResolution, PlanetologyData planetologyData) : base(name, radius, color) {
            this.terrainResolution = terrainResolution;
            this.planetologyData = planetologyData;
        }

        public override void RenderUpdate (double deltaTime) {
            base.RenderUpdate(deltaTime);
        }


        protected override float FallbackDensity () {
            return 4.5f;
        }
    }
}
