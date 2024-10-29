using F7s.Modell.Abstract;
using F7s.Modell.Physical.Localities;
using System; using F7s.Utility.Geometry.Double; using Stride.Core.Mathematics;

namespace F7s.Modell.Conceptual.Agents.GroupDistributions {
    public class GroupComposition : AbstractGameValue {
        // Comprises various statistics determining what the group can do, and the attributes of individual members.
        // Folding members into the group should alter the group composition.

        public bool humans { get; private set; } = true;
        private GeographicDistribution geographicDistribution;
        private Demographics demographics;
        public GameLong abstractMembersCount { get; private set; } = 0;

        public GroupComposition (long members, GeographicDistribution geographicDistribution, Demographics demographics = null) {
            this.geographicDistribution = geographicDistribution;
            this.demographics = demographics ?? Demographics.Default();
            this.abstractMembersCount = members;
        }

        public void SetMemberCount (long newCount) {
            this.abstractMembersCount.Set(newCount);
        }

        public override string ToString () {
            return this.abstractMembersCount.ToString() + " anonymous members";
        }

        public Locality GetRepresentativeLocality () {
            return this.geographicDistribution.RepresentativeLocality();
        }
        public Locality GenerateIndividualMemberLocality () {
            return new Fixed(null, this.GetRepresentativeLocality(), null);
        }

        public GroupComposition Copy () {
            throw new NotImplementedException();
        }
    }
}
