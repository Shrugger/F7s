using F7s.Utility;
using F7s.Utility.Mescherei;
using F7s.Utility.Geometry;

namespace F7s.Modell.Terrains {
    public class ImpactCrater : TerrainFeature {

        private readonly PolarCoordinates center;
        private readonly float radius;
        private readonly float depth;

        public ImpactCrater (PolarCoordinates center, float radius, float depth) {
            this.center = center;
            this.radius = radius;
            this.depth = depth;
        }

        public override bool Reaches (Vertex vertex) {
            return base.RadiusReaches(this.radius, this.center, vertex.Coordinates);
        }

        protected override void Apply (Vertex vertex) {
            float proximityFactor = this.ProximityFactor(this.center, vertex.Coordinates, this.radius);
            if (proximityFactor > 0.1f) {
                vertex.ShiftRadius(2f * this.depth);
                vertex.SetColor(Farbe.darkGrau);
            } else {
                vertex.ShiftRadius(-this.depth * proximityFactor);
                vertex.SetColor(Farbe.Lerp(vertex.Color, Farbe.black, 0.5f));
            }
        }
    }
}
