using F7s.Modell.Physical.Localities;

namespace F7s.Modell.Conceptual.Agents.GroupDistributions {
    public class LocalizedGroupDistribution : GeographicDistribution {
        private Locality locality;

        public LocalizedGroupDistribution (Locality locality) {
            this.locality = locality;
        }

        public override Locality RepresentativeLocality () {
            return this.locality;
        }
    }
}
