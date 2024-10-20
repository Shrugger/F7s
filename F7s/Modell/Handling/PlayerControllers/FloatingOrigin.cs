using F7s.Engine;
using F7s.Modell.Physical.Localities;
using System;

namespace F7s.Modell.Handling.PlayerControllers {

    public class FloatingOrigin : Locality {
        public Locality FloatingAnchor { get; private set; }
        public const float DefaultOriginSnapDistanceLimit = 500;
        private MatrixD transform;

        public float MaximumDistance { get; private set; } = DefaultOriginSnapDistanceLimit;
        public void SetMaximumDistance (float value) {
            if (value == MaximumDistance) {
                return;
            }
            MaximumDistance = value;
            Console.WriteLine("Setting Origin Snap Distance of " + this + " from " + MaximumDistance + " to " + value + ".");
        }

        public FloatingOrigin (Locality floatingAnchor, float maxDistance = DefaultOriginSnapDistanceLimit) : base(null, floatingAnchor) {
            FloatingAnchor = floatingAnchor;
            SetMaximumDistance(maxDistance);
            Snap(0);
        }

        public override bool InheritsRotation () {
            return false; // As far as I can see, it makes no difference. TODO: Confirm.
        }

        public double DistanceToReferenceOrigin () {
            FloatingAnchor.Validate();
            return DistanceTo(FloatingAnchor);
        }

        public override void SetTransform (MatrixD value) {
            transform = value;
        }

        public override MatrixD GetLocalTransform () {
            MatrixD local = transform;
            return local;
        }

        public override void Update (double deltaTime) {
            base.Update(deltaTime);
            SnapIfNecessary();
        }

        private void SnapIfNecessary () {
            double distance = DistanceToReferenceOrigin();
            if (distance > MaximumDistance) {
                Snap(distance);
            }
        }

        public static void ForceSnap () {
            (Origin.GetOriginLocality() as FloatingOrigin).Snap();
        }

        private void Snap (double? distance = null) {
            bool logOriginSnaps = false;
            if (logOriginSnaps) {
                Console.WriteLine(Zeit.Stempel + this + ": Origin snap over distance " + distance + " / " + MaximumDistance + " to " + FloatingAnchor + ".");
            }
            SetTransform(CalculateNewTransform());
        }

        private MatrixD CalculateNewTransform () {
            return MatrixD.Transformation(FloatingAnchor.GetLocalTransform().Origin, Matrix3x3d.Identity);
        }

        public override Locality HierarchySuperior () {
            Locality superior = FloatingAnchor.HierarchySuperior();
            superior.Validate();
            return superior;
        }

        protected override void ReplaceSuperior (Locality replacement) {
            replacement.Validate();
            FloatingAnchor = replacement;
            base.ReplaceSuperior(replacement);
        }

    }
}
