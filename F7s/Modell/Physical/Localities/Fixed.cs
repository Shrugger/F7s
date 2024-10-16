using F7s.Utility;
using F7s.Utility.Geometry;
using System;
using System.Diagnostics;
using Stride.Core.Mathematics;

namespace F7s.Modell.Physical.Localities {

    public class Fixed : Locality {

        private Locality anchor;
        public Transform3D Transform { get; private set; }

        public Fixed (PhysicalEntity entity, Transform3D transform = null, Locality anchor = null, Vector3? velocity = null) : base(entity, anchor) {
            this.Transform = transform ?? Transform3D.Identity;
            this.anchor = anchor;
        }

        public override Locality Sibling (PhysicalEntity entity) {
            return new Fixed(entity, this.Transform, this.anchor);
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
                    Transform3D newLocalTransform = this.CalculateRelativeTransform(newAnchor);
                    newLocality = new Fixed(this.physicalEntity, newLocalTransform, newAnchor);
                    AssertEquivalence(this, newLocality);
                    break;
                case ReanchorMethodology.MaintainLocalTransform:
                    Debug.Assert(null == newTransform);
                    newLocality = new Fixed(this.physicalEntity, this.Transform, newAnchor);
                    break;
                case ReanchorMethodology.UseNewTransform:
                    Debug.Assert(newTransform != null);
                    Debug.Assert(Transforms.ValidPositional(newTransform));
                    newLocality = new Fixed(this.physicalEntity, newTransform, newAnchor);
                    break;
                default:
                    throw new NotImplementedException(methodology.ToString());
            }

            newLocality.Name = this.Name;
            this.Replace(newLocality);
            return newLocality;
        }

        public override Visualizabilities Visualizability () {
            return Visualizabilities.Mobile;
        }

        public override Vector3 GetLocalVelocity () {
            return Vector3.Zero;
        }

        public override Locality HierarchySuperior () {
            return this.anchor;
        }

        protected override void ReplaceSuperior (Locality replacement) {
            this.anchor = replacement;
            base.ReplaceSuperior(replacement);
        }

        public override Transform3D GetLocalTransform () {
            return this.Transform;
        }

        public override string ToString () {
            if (this.Name != null) {
                return this.Name;
            } else {
                return base.ToString() + (this.anchor != null ? "@[" + (this.anchor.physicalEntity?.ToString() ?? this.anchor.ToString()) + "]" : "");
            }
        }

        public override void SetTransform (Transform3D value) {
            if (Transforms.InvalidPositional(value)) {
                throw new Exception();
            }
            this.Transform = value;
        }

        public override void Translate (Vector3 relativeOffset) {
            if (Vectors.Invalid(relativeOffset)) {
                throw new Exception(relativeOffset.ToString());
            }
            throw new NotImplementedException(); // TODO: Redo for Stride.
            //this.Transform = this.Transform.TranslatedLocal(relativeOffset);
            if (Vectors.Invalid(this.Transform.Origin)) {
                throw new Exception(this.Transform.Origin.ToString());
            }
        }

        public override void Rotate (float yaw, float pitch, float roll = 0) {
            Transform3D entityTransform = this.GetLocalTransform();

            throw new NotImplementedException(); // TODO: Redo for Stride.
            // entityTransform.Basis = entityTransform.Basis.Rotated(-Vector3.UnitY, yaw).Rotated(-Vector3.UnitX, pitch).Rotated(Vector3.UnitZ, roll);
            this.SetTransform(entityTransform);
        }

        public override void RotateEcliptic (float yaw, float pitch, float roll = 0) {
            Transform3D entityTransform = this.GetLocalTransform();

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
                    Rounding.RoundToFirstInterestingDigit(currentPitch, 3) +
                    " + " + (Math.Abs(permittedPitch - additionalPitch) > 0.0001f ?
                        Rounding.RoundToFirstInterestingDigit(permittedPitch, 3) + " LIMITED" :
                        Rounding.RoundToFirstInterestingDigit(additionalPitch, 3)
                    ) +
                    " = " + Rounding.RoundToFirstInterestingDigit(expectedLegalPitch, 3)
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

            if (Transforms.InvalidPositional(this.Transform)) {
                throw new Exception();
            }
        }
    }
}
