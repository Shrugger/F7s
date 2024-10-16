using F7s.Utility;
using F7s.Utility.Mescherei;

namespace F7s.Modell.Terrains {
    public class BaseTerrain : TerrainFeature {

        private readonly Farbe? color;

        public BaseTerrain (Farbe? color) {
            this.Date = 0;
            this.color = color;
        }

        public override bool Reaches (Vertex vertex) {
            return true;
        }

        protected override void Apply (Vertex vertex) {
            vertex.SetRadius(this.Terrain.BaseRadius);
            if (this.color.HasValue) {
                vertex.SetColor(this.color.Value);
            }
        }
    }
}
