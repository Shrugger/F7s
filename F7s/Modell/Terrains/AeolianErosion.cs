using F7s.Utility.Mescherei;

namespace F7s.Modell.Terrains {
    public class AeolianErosion : TerrainFeature {
        public override bool Reaches (Vertex vertex) {
            return true;
        }

        protected override void Apply (Vertex vertex) {
            bool erode;
            bool deposit;
            throw new System.NotImplementedException("Check surrounding edges to see how exposed we are. If exposed at all, erode. Then deposit on neighboring vertices of lower elevation, but only up to this elevation.");
        }
    }
}
