using F7s.Utility.Geometry.Double;

namespace F7s.Modell.Physical.Localities {
    public class Attached : Fixed {

        private PhysicalEntity anchorEntity;

        public Attached (PhysicalEntity entity, PhysicalEntity anchor, MatrixD offset) : base(entity, offset, anchor) {
            anchorEntity = anchor;
        }

        public override Locality HierarchySuperior () {
            return anchorEntity;
        }
        protected override void ReplaceSuperior (Locality replacement) {
            anchorEntity = replacement.physicalEntity;
            base.ReplaceSuperior(replacement);
        }


    }
}
