using F7s.Utility.Geometry;
using F7s.Utility.Mescherei;

namespace F7s.Modell.Terrains {

    public class SetFixedElevation : TerrainFeature {

        private float elevation;
        private PolarCoordinates coordinates;
        private float featureRadius;

        public SetFixedElevation(PolarCoordinates coordinates, float radius, float elevation) : base() {
            this.elevation = elevation;
            this.coordinates = coordinates;
            this.featureRadius = radius;
        }

        protected override void Apply(Vertex vertex) {
            vertex.SetRadius(this.Terrain.BaseRadius + this.elevation);
        }

        public override bool Reaches(Vertex vertex) {
            return this.RadiusReaches(this.featureRadius, this.coordinates, this.coordinates);
        }
    }

}
