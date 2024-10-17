
using System;

namespace F7s.Utility.Shapes {

    
    public class Tube : Cylinder {

        public readonly float internalDiameter;

        public Tube(float externalDiameter, float externalLength, float internalDiameter) : base(externalDiameter, externalLength) {
            this.internalDiameter = internalDiameter;
        }

        public override string ToString() {
            return this.GetType().Name + "(" + Rounding.RoundToFirstInterestingDigit(this.diameter) + "x" + Rounding.RoundToFirstInterestingDigit(this.length) + ")(" + Rounding.RoundToFirstInterestingDigit(this.internalDiameter) + ")";
        }
        public virtual Cylinder InternalNegativeCylinder() {
            return new Cylinder(this.internalDiameter, this.length);
        }

        public override Shape3Dim GetInternalNegativeSpace() {
            return this.InternalNegativeCylinder();
        }
        public float WallThickness() {
            return (this.diameter - this.internalDiameter) / 2.0f;
        }

        public override Shape3Dim GetShapePlusSize(float addition) {
            return new Tube(this.diameter + addition, this.length + addition, this.internalDiameter);
        }

        public override float SubstantialVolume() {
            float substantialVolume = base.SubstantialVolume();
            float negativeVolume = this.InternalNegativeCylinder().SubstantialVolume();
            float result = substantialVolume - negativeVolume;
            if (substantialVolume <= 0) {
                throw new Exception(substantialVolume + " - " + negativeVolume + " = " + result);
            }
            if (negativeVolume < 0) {
                throw new Exception(substantialVolume + " - " + negativeVolume + " = " + result);
            }
            if (result <= 0) {
                throw new Exception(substantialVolume + " - " + negativeVolume + " = " + result);
            }
            return result;
        }

        public override bool Equals(object obj) {
            return obj is Tube tube &&
                   base.Equals(obj) &&
                   this.internalDiameter == tube.internalDiameter;
        }

        public override int GetHashCode() {
            var hashCode = -469255269;
            hashCode = hashCode * -1521134295 + base.GetHashCode();
            hashCode = hashCode * -1521134295 + this.internalDiameter.GetHashCode();
            return hashCode;
        }
    }

}