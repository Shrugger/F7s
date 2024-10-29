using F7s.Utility;
using F7s.Utility.Geometry.Double;
using Stride.Core.Mathematics;
using System;
using System.Diagnostics;

namespace F7s.Modell.Physical.Localities {

    public class Fixed : Locality {

        private Locality anchor;
        public MatrixD Transform { get; private set; }

        public Fixed (PhysicalEntity entity, Locality anchor, MatrixD? transform = null, Double3? velocity = null) : base(entity, anchor) {
            Transform = transform ?? MatrixD.Identity;
            this.anchor = anchor;
        }

        public override Locality Sibling (PhysicalEntity entity) {
            return new Fixed(entity, anchor, Transform);
        }

        public static Fixed FixedSibling (PhysicalEntity entity, Locality sibling) {
            return new Fixed(entity, sibling.HierarchySuperior(), sibling.GetLocalTransform(), null);
        }

        public enum ReanchorMethodology { MaintainAbsoluteTransform, MaintainLocalTransform, UseNewTransform }
        public Fixed Reanchored (Locality newAnchor, ReanchorMethodology methodology = ReanchorMethodology.MaintainAbsoluteTransform, MatrixD? newTransform = null) {

            Debug.Assert(newAnchor != null);

            // TODO: Transform velocity to new anchor's coordinate system.

            Fixed newLocality;
            switch (methodology) {
                case ReanchorMethodology.MaintainAbsoluteTransform:
                    Debug.Assert(null == newTransform);
                    MatrixD newLocalTransform = CalculateRelativeTransform(newAnchor);
                    newLocality = new Fixed(physicalEntity, newAnchor, newLocalTransform);
                    AssertEquivalence(this, newLocality);
                    break;
                case ReanchorMethodology.MaintainLocalTransform:
                    Debug.Assert(null == newTransform);
                    newLocality = new Fixed(physicalEntity, newAnchor, Transform);
                    break;
                case ReanchorMethodology.UseNewTransform:
                    Debug.Assert(newTransform != null);
                    Debug.Assert(Mathematik.ValidPositional(newTransform.Value));
                    newLocality = new Fixed(physicalEntity, newAnchor, newTransform);
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

        public override Double3 GetLocalVelocity () {
            return Double3.Zero;
        }

        public override Locality HierarchySuperior () {
            return anchor;
        }

        protected override void ReplaceSuperior (Locality replacement) {
            anchor = replacement;
            base.ReplaceSuperior(replacement);
        }

        public override MatrixD GetLocalTransform () {
            return Transform;
        }

        public override string ToString () {
            if (Name != null) {
                return Name;
            } else {
                return base.ToString() + (anchor != null ? "@[" + (anchor.physicalEntity?.ToString() ?? anchor.ToString()) + "]" : "");
            }
        }

        public override void SetTransform (MatrixD value) {
            if (Mathematik.InvalidPositional(value)) {
                throw new Exception();
            }
            Transform = value;
        }

        public override void Translate (Double3 relativeOffset) {
            if (relativeOffset == Double3.Zero) {
                return;
            }
            if (Mathematik.Invalid(relativeOffset)) {
                throw new Exception(relativeOffset.ToString());
            }
            Transform = MatrixD.Translation(relativeOffset) * Transform;
            Double3 newTranslation = Transform.TranslationVector;

            if (Mathematik.Invalid(Transform.TranslationVector)) {
                throw new Exception(Transform.TranslationVector.ToString());
            }
        }

        public override void Rotate (double yaw, double pitch, double roll = 0) {
            MatrixD entityTransform = GetLocalTransform();

            throw new NotImplementedException(); // TODO: Redo for Stride.
            // entityTransform.Basis = entityTransform.Basis.Rotated(-Double3.UnitY, yaw).Rotated(-Double3.UnitX, pitch).Rotated(Double3.UnitZ, roll);
            SetTransform(entityTransform);
        }

        public override void RotateEcliptic (double yaw, double pitch, double roll = 0) {
            MatrixD entityTransform = GetLocalTransform();

            throw new NotImplementedException(); // TODO: Redo for Stride.
            /*
            float oldPitch = -Mathematik.RadToDeg(entityTransform.Basis.GetEuler().X);
            float permittedPitch = LimitPitch(oldPitch, pitch);

            bool useBuiltinOperations = false;
            if (useBuiltinOperations) {
                entityTransform = entityTransform.Rotated(Double3.UnitY, MathF.DegToRad(yaw));
                entityTransform = entityTransform.RotatedLocal(-Double3.UnitX, MathF.DegToRad(permittedPitch));
                entityTransform = entityTransform.RotatedLocal(Double3.UnitZ, MathF.DegToRad(roll));
            } else {
                Basis oldBasis = entityTransform.Basis;
                Basis yawBasis = new Basis(Double3.UnitY, MathF.DegToRad(yaw));
                Basis pitchBasis = new Basis(-Double3.UnitX, MathF.DegToRad(permittedPitch));
                Basis rollBasis = new Basis(Double3.UnitZ, MathF.DegToRad(roll));
                Basis newBasis = yawBasis * oldBasis * pitchBasis * rollBasis;
                entityTransform = MatrixD.Transformation(newBasis, entityTransform.TranslationVector);
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
                System.Diagnostics.Debug.WriteLine(
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

        public void LookAt (Double3 relativePosition, Double3 up) {

            Debug.Assert(Double3.Zero != relativePosition);
            Debug.Assert(Double3.Zero != up);
            Debug.Assert(Double3.Zero != Double3.Cross(up, relativePosition));

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
