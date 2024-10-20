using F7s.Utility;
using F7s.Utility.Geometry;
using Stride.Core.Mathematics;
using System;
using System.Diagnostics;

namespace F7s.Modell.Physical.Localities {

    public class Fixed : Locality {

        private Locality anchor;
        public Transform3D Transform { get; private set; }

        public Fixed (PhysicalEntity entity, Transform3D transform = null, Locality anchor = null, Vector3? velocity = null) : base(entity, anchor) {
            Transform = transform ?? Transform3D.Identity;
            this.anchor = anchor;
        }

        public override Locality Sibling (PhysicalEntity entity) {
            return new Fixed(entity, Transform, anchor);
        }

        public static Fixed FixedSibling (PhysicalEntity entity, Locality sibling) {
            return new Fixed(entity, sibling.GetLocalTransform(), sibling.HierarchySuperior(), null);
        }

        public enum ReanchorMethodology { MaintainAbsoluteTransform, MaintainLocalTransform, UseNewTransform }
        public Fixed Reanchored (Locality newAnchor, ReanchorMethodology methodology = ReanchorMethodology.MaintainAbsoluteTransform, Transform3D newTransform = null) {

            Debug.Assert(newAnchor != null);

            // TODO: Transform velocity to new anchor's coordinate system.

            Fixed newLocality;
            switch (methodology) {
                case ReanchorMethodology.MaintainAbsoluteTransform:
                    Debug.Assert(null == newTransform);
                    Transform3D newLocalTransform = CalculateRelativeTransform(newAnchor);
                    newLocality = new Fixed(physicalEntity, newLocalTransform, newAnchor);
                    AssertEquivalence(this, newLocality);
                    break;
                case ReanchorMethodology.MaintainLocalTransform:
                    Debug.Assert(null == newTransform);
                    newLocality = new Fixed(physicalEntity, Transform, newAnchor);
                    break;
                case ReanchorMethodology.UseNewTransform:
                    Debug.Assert(newTransform != null);
                    Debug.Assert(Mathematik.ValidPositional(newTransform));
                    newLocality = new Fixed(physicalEntity, newTransform, newAnchor);
                    break;
                default:
                    throw new NotImplementedException(methodology.ToString());
            }

            newLocality.Name = Name;
            Replace(newLocality);
            return newLocality;
        }

        public override Visualizabilities Visualizability () {
            return Visualizabilities.Mobile;
        }

        public override Vector3 GetLocalVelocity () {
            return Vector3.Zero;
        }

        public override Locality HierarchySuperior () {
            return anchor;
        }

        protected override void ReplaceSuperior (Locality replacement) {
            anchor = replacement;
            base.ReplaceSuperior(replacement);
        }

        public override Transform3D GetLocalTransform () {
            return Transform;
        }

        public override string ToString () {
            if (Name != null) {
                return Name;
            } else {
                return base.ToString() + (anchor != null ? "@[" + (anchor.physicalEntity?.ToString() ?? anchor.ToString()) + "]" : "");
            }
        }

        public override void SetTransform (Transform3D value) {
            if (Mathematik.InvalidPositional(value)) {
                throw new Exception();
            }
            Transform = value;
        }

        public override void Translate (Vector3 relativeOffset) {
            if (Mathematik.Invalid(relativeOffset)) {
                throw new Exception(relativeOffset.ToString());
            }
            throw new NotImplementedException(); // TODO: Redo for Stride.
            //this.Transform = this.Transform.TranslatedLocal(relativeOffset);
            if (Mathematik.Invalid(Transform.Origin)) {
                throw new Exception(Transform.Origin.ToString());
            }
        }

        public override void Rotate (float yaw, float pitch, float roll = 0) {
            Transform3D entityTransform = GetLocalTransform();

            throw new NotImplementedException(); // TODO: Redo for Stride.
            // entityTransform.Basis = entityTransform.Basis.Rotated(-Vector3.UnitY, yaw).Rotated(-Vector3.UnitX, pitch).Rotated(Vector3.UnitZ, roll);
            SetTransform(entityTransform);
        }

        public override void RotateEcliptic (float yaw, float pitch, float roll = 0) {
            Transform3D entityTransform = GetLocalTransform();

            throw new NotImplementedException(); // TODO: Redo for Stride.
            /*
            float oldPitch = -Mathematik.RadToDeg(entityTransform.Basis.GetEuler().X);
            float permittedPitch = LimitPitch(oldPitch, pitch);

            bool useBuiltinOperations = false;
            if (useBuiltinOperations) {
                entityTransform = entityTransform.Rotated(Vector3.UnitY, MathF.DegToRad(yaw));
                entityTransform = entityTransform.RotatedLocal(-Vector3.UnitX, MathF.DegToRad(permittedPitch));
                entityTransform = entityTransform.RotatedLocal(Vector3.UnitZ, MathF.DegToRad(roll));
            } else {
                Basis oldBasis = entityTransform.Basis;
                Basis yawBasis = new Basis(Vector3.UnitY, MathF.DegToRad(yaw));
                Basis pitchBasis = new Basis(-Vector3.UnitX, MathF.DegToRad(permittedPitch));
                Basis rollBasis = new Basis(Vector3.UnitZ, MathF.DegToRad(roll));
                Basis newBasis = yawBasis * oldBasis * pitchBasis * rollBasis;
                entityTransform = new Transform3D(newBasis, entityTransform.Origin);
            }

            this.SetTransform(entityTransform);

            float newPitch = -Mathematik.RadToDeg(entityTransform.Basis.GetEuler().X);
            if ((oldPitch < 0 && newPitch > 0 && pitch < 0) || (oldPitch > 0 && newPitch < 0 && pitch > 0)) {
                throw new Exception("Flipped by " + pitch + " from " + oldPitch + " to " + newPitch + "!");
            }
            */
        }

        public static float LimitPitch (float currentPitch, float additionalPitch, float safetyMargin = 5.0f, bool logging = false) {
            Debug.Assert(safetyMargin > 0 && safetyMargin < 90);

            float expectedPitch = currentPitch + additionalPitch;
            float maxPitch = 90 - safetyMargin;
            float minPitch = -maxPitch;
            float permittedPitch = Math.Clamp(expectedPitch, minPitch, maxPitch) - currentPitch;

            float expectedLegalPitch = currentPitch + permittedPitch;

            if (expectedLegalPitch < minPitch || expectedLegalPitch > maxPitch) {
                throw new Exception(minPitch + " <= " + expectedLegalPitch + " <= " + maxPitch);
            }

            if (logging) {
                Console.WriteLine(
                    Mathematik.RoundToFirstInterestingDigit(currentPitch, 3) +
                    " + " + (Math.Abs(permittedPitch - additionalPitch) > 0.0001f ?
                        Mathematik.RoundToFirstInterestingDigit(permittedPitch, 3) + " LIMITED" :
                        Mathematik.RoundToFirstInterestingDigit(additionalPitch, 3)
                    ) +
                    " = " + Mathematik.RoundToFirstInterestingDigit(expectedLegalPitch, 3)
                    );
            }

            return permittedPitch;
        }

        public void LookAt (Vector3 relativePosition, Vector3 up) {

            Debug.Assert(Vector3.Zero != relativePosition);
            Debug.Assert(Vector3.Zero != up);
            Debug.Assert(Vector3.Zero != Vector3.Cross(up, relativePosition));

            throw new NotImplementedException(); // TODO: Redo for Stride.
            // this.SetTransform(this.Transform.LookingAt(relativePosition, up));
        }

        public override void Validate () {
            base.Validate();

            if (Mathematik.InvalidPositional(Transform)) {
                throw new Exception();
            }
        }
    }
}
