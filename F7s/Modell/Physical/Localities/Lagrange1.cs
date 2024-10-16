using F7s.Utility.Geometry;
using Stride.Core.Mathematics;
using System;

namespace F7s.Modell.Physical.Localities {
    public class Lagrange1 : Locality {
        public readonly PhysicalEntity primary;
        public readonly PhysicalEntity secondary;
        private Quaternion rotation = Quaternion.Identity;

        public Lagrange1 (PhysicalEntity entity, PhysicalEntity primary, PhysicalEntity secondary) : base(entity, primary) {
            this.primary = primary;
            this.secondary = secondary;
        }

        public override Locality HierarchySuperior () {
            return this.primary;
        }

        public override Transform3D GetLocalTransform () {
            float factor = 0.5f; // TODO: L1 Formula.
            Vector3d origin = this.secondary.Locality.GetRelativeTransform(this.primary).Origin * factor;
            return new Transform3D(new Basis(this.rotation), origin);
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
