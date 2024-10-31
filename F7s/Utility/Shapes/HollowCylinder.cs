namespace F7s.Utility.Shapes {
    
    public class HollowCylinder : Cylinder {

        private readonly float internalDiameter;
        private readonly float internalLength;

        public HollowCylinder(float externalDiameter, float externalLength, float internalDiameter, float internalLength) : base(externalDiameter, externalLength) {
            this.internalDiameter = internalDiameter;
            this.internalLength = internalLength;
        }

        public override string ToString() {
            return this.GetType().Name + "(" + MM.RoundToFirstInterestingDigit(this.diameter, 2) + "x" + MM.RoundToFirstInterestingDigit(this.length, 2) + ")(" + MM.RoundToFirstInterestingDigit(this.internalDiameter, 2) + "x" + MM.RoundToFirstInterestingDigit(this.internalLength, 2) + ")";
        }
        public Cylinder InternalNegativeCylinder() {
            return new Cylinder(this.internalDiameter, this.internalLength);
        }

        public override Shape3Dim GetInternalNegativeSpace() {
            return this.InternalNegativeCylinder();
        }

        public override Shape3Dim GetShapePlusSize(float addition) {
            return new Tube(this.diameter + addition, this.length + addition, this.internalDiameter);
        }

        public override float SubstantialVolume() {
            return base.SubstantialVolume() - this.InternalNegativeCylinder().SubstantialVolume();
        }

        public override bool Equals(object obj) {
            return obj is HollowCylinder cylinder &&
                   base.Equals(obj) &&
                   this.internalDiameter == cylinder.internalDiameter;
        }

        public override int GetHashCode() {
            var hashCode = -908813899;
            hashCode = hashCode * -1521134295 + base.GetHashCode();
            hashCode = hashCode * -1521134295 + this.internalDiameter.GetHashCode();
            hashCode = hashCode * -1521134295 + this.internalLength.GetHashCode();
            return hashCode;
        }
        public float SideWallThickness() {
            return (this.diameter - this.internalDiameter) / 2.0f;
        }
        public float FlatWallThickness() {
            return (this.length - this.internalLength) / 2.0f;
        }
    }

}