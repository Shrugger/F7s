using F7s.Utility.Geometry;
using F7s.Utility.Mescherei;

namespace F7s.Modell.Terrains
{
    public class Ocean : SetFixedElevation {

        public Ocean(PolarCoordinates coordinates, float radius) : base(coordinates, radius, 0) {
        }

        protected override void Apply(Vertex vertex) {
            base.Apply(vertex);
            vertex.SetColor(Terrain.Colors.Ocean);
        }
    }

}
