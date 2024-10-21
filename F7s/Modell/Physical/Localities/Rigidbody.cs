using F7s.Utility.Geometry;
using System; using F7s.Utility.Geometry.Double; using Stride.Core.Mathematics;

namespace F7s.Modell.Physical.Localities {
    public class Rigidbody : Locality {
        public Rigidbody (PhysicalEntity entity, Locality anchor) : base(entity, anchor) {
            throw new NotImplementedException();
        }

        public override MatrixD GetLocalTransform () {
            throw new NotImplementedException();
        }

        public override Locality HierarchySuperior () {
            throw new NotImplementedException();
        }
    }
}
