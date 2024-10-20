using F7s.Utility.Geometry;
using F7s.Utility.Mescherei;

namespace F7s.Modell.Terrains {
    public class Mountains : SetLinearElevation {


        public Mountains(PolarCoordinatesD coordinates, float radius, float peakElevation) : base(coordinates, radius, peakElevation) {

        }

        protected override void Apply(Vertex vertex) {
            base.Apply(vertex);
            vertex.SetColor(Terrain.Colors.Mountain);
        }
    }

}
