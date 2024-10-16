using F7s.Utility.Geometry;
using System;

namespace F7s.Modell.Physical.Localities {



    public class Distributed : Locality {
        public Distributed (PhysicalEntity entity) : base(entity, null) {
        }

        public Locality GenerateSubLocality () {
            throw new NotImplementedException();
        }

        public override Locality HierarchySuperior () {
            throw new NotImplementedException();
        }
        protected override void ReplaceSuperior (Locality replacement) {
            throw new NotImplementedException();
            base.ReplaceSuperior(replacement);
        }

        public override Transform3D GetLocalTransform () {
            throw new NotImplementedException();
        }
    }
}
