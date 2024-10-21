using F7s.Modell.Abstract;
using F7s.Modell.Physical.Localities;
using Stride.Core.Mathematics; using F7s.Utility.Geometry.Double;

namespace F7s.Modell.Handling.PhysicalData {
    public class PresumedPhysicalData : PhysicalRepresentationData {

        private readonly GameEntity entity;

        public float boundingRadius;
        public Vector3 scale;

        public PresumedPhysicalData (GameEntity entity) {
            this.entity = entity;
        }

        public override float BoundingRadius () {
            return boundingRadius;
        }

        public override Locality Locality () {
            return entity.GetLocality();
        }

        public override Vector3 Scale () {
            return scale;
        }
    }
}
