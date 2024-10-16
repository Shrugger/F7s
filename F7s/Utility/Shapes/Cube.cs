using Stride.Core.Mathematics; using System;

namespace F7s.Utility.Shapes {

    [System.Serializable]
    public class Cube : Box {

        public Cube(float edgeLength)
            : base(fullExtents: new Vector3(edgeLength, edgeLength, edgeLength)) { }

        public static implicit operator ShapeType(Cube shape) {
            return F7s.Utility.Shapes.ShapeType.Box;
        }
        public override string ToString() {
            return this.GetType().Name + "(" + Rounding.RoundToFirstInterestingDigit(this.fullExtents.X) + ")";
        }
    }

}