using F7s.Geometry;

namespace F7s.Modell.Physical.Localities {
    public class Attached : Fixed {

        private PhysicalEntity anchorEntity;

        public Attached (PhysicalEntity entity, PhysicalEntity anchor, Transform3D offset) : base(entity, offset, anchor) {
            this.anchorEntity = anchor;
        }

        public override Locality HierarchySuperior () {
            return this.anchorEntity;
        }
        protected override void ReplaceSuperior (Locality replacement) {
            this.anchorEntity = replacement.physicalEntity;
            base.ReplaceSuperior(replacement);
        }


    }
}
