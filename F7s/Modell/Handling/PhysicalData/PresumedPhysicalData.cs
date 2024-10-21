using F7s.Modell.Abstract;
using F7s.Modell.Physical.Localities;
using Stride.Core.Mathematics;

namespace F7s.Modell.Handling.PhysicalData {
    public class PresumedPhysicalData : PhysicalRepresentationData {

        private readonly GameEntity entity;

        public double boundingRadius;
        public Double3 scale;

        public PresumedPhysicalData (GameEntity entity) {
            this.entity = entity;
        }

        public override double BoundingRadius () {
            return boundingRadius;
        }

        public override Locality Locality () {
            return entity.GetLocality();
        }

        public override Double3 Scale () {
            return scale;
        }
    }
}
