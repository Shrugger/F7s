using F7s.Modell.Physical;
using F7s.Modell.Physical.Localities;
using Stride.Core.Mathematics;

namespace F7s.Modell.Handling.PhysicalData {
    public class DirectPhysicalData : PhysicalRepresentationData {
        private readonly PhysicalEntity entity;

        public DirectPhysicalData (PhysicalEntity entity) {
            this.entity = entity;
        }

        public override float BoundingRadius () {
            return entity.BoundingRadius();
        }

        public override Locality Locality () {
            return entity.GetLocality();
        }

        public override Vector3 Scale () {
            return entity.Scale();
        }
    }
}
