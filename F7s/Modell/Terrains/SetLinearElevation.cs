using F7s.Utility.Geometry;
using F7s.Utility.Mescherei;

namespace F7s.Modell.Terrains
{
    public class SetLinearElevation : TerrainFeature {
        private PolarCoordinates coordinates;
        private float featureRadius;
        private float peakElevation;
        public SetLinearElevation(PolarCoordinates coordinates, float radius, float peakElevation) : base() {
            this.coordinates = coordinates;
            this.featureRadius = radius;
            this.peakElevation = peakElevation;
        }
        public override bool Reaches(Vertex vertex) {
            bool reaches = this.RadiusReaches(this.featureRadius, this.coordinates, this.coordinates);
            return reaches;
        }

        protected override void Apply(Vertex vertex) {

            double distance = this.Distance(this.coordinates, vertex.Coordinates);
            double relativeDistance = (distance / this.featureRadius);
            double factor = 1 + relativeDistance;
            float effectiveElevation = (float)(this.peakElevation / factor);

            vertex.SetRadius(this.Terrain.BaseRadius + effectiveElevation);
        }
    }

}
