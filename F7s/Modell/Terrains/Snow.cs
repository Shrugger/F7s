using F7s.Utility;
using F7s.Utility.Mescherei;

namespace F7s.Modell.Terrains {
    public class Snow : TerrainFeature {
        public override bool Reaches (Vertex vertex) {
            return this.TemperatureBelowFreezing(vertex);
        }

        private bool TemperatureBelowFreezing (Vertex vertex) {
            float temperature = this.Terrain.CalculateSurfaceTemperature(vertex);
            return temperature < 0;
        }

        protected override void Apply (Vertex vertex) {
            vertex.SetColor(Farbe.white);
        }
    }
}
