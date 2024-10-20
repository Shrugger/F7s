using F7s.Utility.Geometry;
using F7s.Utility.Mescherei;

namespace F7s.Modell.Terrains {
    public class Mesas : SetFixedElevation {
        public Mesas(PolarCoordinatesD coordinates, float radius, float elevation) : base(coordinates, radius, elevation) {
        }

        protected override void Apply(Vertex vertex) {
            base.Apply(vertex);
            vertex.SetColor(Terrain.Colors.Mesa);
        }
    }

}
