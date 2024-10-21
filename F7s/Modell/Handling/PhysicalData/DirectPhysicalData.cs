using F7s.Modell.Physical;
using F7s.Modell.Physical.Localities;
using F7s.Utility.Geometry.Double;

namespace F7s.Modell.Handling.PhysicalData {
    public class DirectPhysicalData : PhysicalRepresentationData {
        private readonly PhysicalEntity entity;

        public DirectPhysicalData (PhysicalEntity entity) {
            this.entity = entity;
        }

        public override double BoundingRadius () {
            return entity.BoundingRadius();
        }

        public override Locality Locality () {
            return entity.GetLocality();
        }

        public override Vector3d Scale () {
            return entity.Scale();
        }
    }
}
