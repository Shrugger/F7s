using Stride.Core.Mathematics; using System;

namespace F7s.Utility.Shapes {
    
    public class HollowBox : Box {

        private readonly float wallThickness;

        public HollowBox(float width, float height, float length, float wallThickness) : this(new Vector3(width, height, length), wallThickness) { }
        public HollowBox(Vector3 externalFullExtents, float wallThickness) : base(externalFullExtents) {
            this.wallThickness = wallThickness;
        }

        public override Shape3Dim GetShapePlusSize(float addition) {
            return new HollowBox(new Vector3(this.fullExtents.X + addition, this.fullExtents.Y + addition, this.fullExtents.Z + addition), this.wallThickness);
        }

        public override float SubstantialVolume() {
            return base.SubstantialVolume() - this.InternalNegativeBox().SubstantialVolume();
        }
        public Box InternalNegativeBox() {
            return new Box(this.fullExtents.X - this.wallThickness, this.fullExtents.Y - this.wallThickness, this.fullExtents.Z - this.wallThickness);
        }

        public override Shape3Dim GetInternalNegativeSpace() {
            return this.InternalNegativeBox();
        }

        public override bool Equals(object obj) {
            return obj is HollowBox duct &&
                   base.Equals(obj) &&
                   this.wallThickness == duct.wallThickness;
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