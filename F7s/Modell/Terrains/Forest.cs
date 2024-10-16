using F7s.Utility;
using F7s.Utility.Geometry;
using F7s.Utility.Mescherei;

namespace F7s.Modell.Terrains {
    public class Forest : TerrainFeature {
        private PolarCoordinates coordinates;
        private float featureRadius;
        private readonly Farbe color;
        private readonly float height;

        public Forest (PolarCoordinates coordinates, float featureRadius, Farbe color, float height) {
            this.coordinates = coordinates;
            this.featureRadius = featureRadius;
            this.color = color;
            this.height = height;
        }

        public override bool Reaches (Vertex vertex) {
            double elevation = this.coordinates.radialDistance;
            if (elevation <= 0) {
                return false;
            }
            return this.RadiusReaches(this.featureRadius, this.coordinates, this.coordinates);
        }

        protected override void Apply (Vertex vertex) {
            vertex.ShiftRadius(this.height);
            vertex.SetColor(Alea.Color(this.color, 0.01f));
        }
    }

}
