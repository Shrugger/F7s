namespace F7s.Utility.Shapes {
    public class HollowCantedBox : CantedBox {
        private readonly float wallThickness;

        public HollowCantedBox(float diameter, float length, float wallThickness) : base(diameter, length) {
            this.wallThickness = wallThickness;
        }


        public override Shape3Dim GetShapePlusSize(float addition) {
            return new HollowCantedBox(this.fullExtents.X + addition, this.fullExtents.Z + addition, this.wallThickness);
        }

        public override float SubstantialVolume() {
            return base.SubstantialVolume() - this.InternalNegativeBox().SubstantialVolume();
        }
        public Box InternalNegativeBox() {
            return new CantedBox(this.fullExtents.X - this.wallThickness, this.fullExtents.Z - this.wallThickness);
        }

        public override Shape3Dim GetInternalNegativeSpace() {
            return this.InternalNegativeBox();
        }

        public override bool Equals(object obj) {
            return obj is HollowCantedBox hollowCantedBox &&
                   base.Equals(obj) &&
                   this.wallThickness == hollowCantedBox.wallThickness;
        }

        public override int GetHashCode() {
            var hashCode = -469255269;
            hashCode = hashCode * -1521134295 + base.GetHashCode();
            hashCode = hashCode * -1521134295 + this.wallThickness.GetHashCode();
            return hashCode;
        }
        public override string ToString() {
            return this.GetType().Name + this.fullExtents.ToString() + "(" + Rounding.RoundToFirstInterestingDigit(this.wallThickness) + ")";
        }
    }
}