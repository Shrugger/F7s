using F7s.Utility.Mescherei;

namespace F7s.Modell.Terrains
{
    public class Oceans : TerrainFeature {
        private float seaLevel;

        public Oceans(float seaLevel) {
            this.seaLevel = seaLevel;
        }

        public override bool Reaches(Vertex vertex) {
            return this.Terrain.CalculateSurfaceElevation(vertex) < this.seaLevel;
        }

        protected override void Apply(Vertex vertex) {
            vertex.SetRadius(this.Terrain.BaseRadius);
            vertex.SetColor(Terrain.Colors.Ocean);
        }
    }
}
