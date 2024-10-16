using F7s.Utility.Geometry;
using System;

namespace F7s.Modell.Physical.Localities {
    public class Rigidbody : Locality {
        public Rigidbody (PhysicalEntity entity, Locality anchor) : base(entity, anchor) {
            throw new NotImplementedException();
        }

        public override Transform3D GetLocalTransform () {
            throw new NotImplementedException();
        }

        public override Locality HierarchySuperior () {
            throw new NotImplementedException();
        }
    }
}
