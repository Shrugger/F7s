using F7s.Modell.Physical.Localities;
using System;

namespace F7s.Modell.Conceptual.Agents.GroupDistributions {
    public abstract class GeographicDistribution {
        public virtual Locality RepresentativeLocality () {
            throw new NotImplementedException();
        }

        public static implicit operator GeographicDistribution (Locality locality) {
            return new LocalizedGroupDistribution(locality);
        }
    }
}
