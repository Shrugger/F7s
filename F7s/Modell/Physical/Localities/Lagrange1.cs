using F7s.Utility.Geometry.Double;
using System;
namespace F7s.Modell.Physical.Localities {
    public class Lagrange1 : Locality {
        public readonly PhysicalEntity primary;
        public readonly PhysicalEntity secondary;
        private QuaternionD rotation = QuaternionD.Identity;

        public Lagrange1 (PhysicalEntity entity, PhysicalEntity primary, PhysicalEntity secondary) : base(entity, primary) {
            this.primary = primary;
            this.secondary = secondary;
        }

        public override Locality HierarchySuperior () {
            return primary;
        }

        public override MatrixD GetLocalTransform () {
            float factor = 0.5f; // TODO: L1 Formula.
            Vector3d origin = secondary.Locality.GetRelativeTransform(primary).TranslationVector * factor;
            return MatrixD.Transformation(origin, rotation);
        }

        public override bool InheritsRotation () {
            return false;
        }

        protected override void ReplaceSuperior (Locality replacement) {
            throw new Exception("Does this even make sense?");
        }

        public override Visualizabilities Visualizability () {
            return Visualizabilities.Cyclical;
        }
    }
}
