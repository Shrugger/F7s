using F7s.Modell.Abstract;
using F7s.Modell.Physical.Localities;
using F7s.Utility.Geometry.Double;

namespace F7s.Modell.Handling.PhysicalData {
    public class PresumedPhysicalData : PhysicalRepresentationData {

        private readonly GameEntity entity;

        public double boundingRadius;
        public Vector3d scale;

        public PresumedPhysicalData (GameEntity entity) {
            this.entity = entity;
        }

        public override double BoundingRadius () {
            return boundingRadius;
        }

        public override Locality Locality () {
            return entity.GetLocality();
        }

        public override Vector3d Scale () {
            return scale;
        }
    }
}
