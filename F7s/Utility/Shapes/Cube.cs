using Stride.Core.Mathematics; using F7s.Utility.Geometry.Double; using Stride.Core.Mathematics; using System; using F7s.Utility.Geometry.Double; using Stride.Core.Mathematics;

namespace F7s.Utility.Shapes {

    
    public class Cube : Box {

        public Cube(float edgeLength)
            : base(fullExtents: new Vector3(edgeLength, edgeLength, edgeLength)) { }

        public static implicit operator ShapeType(Cube shape) {
            return F7s.Utility.Shapes.ShapeType.Box;
        }
        public override string ToString() {
            return this.GetType().Name + "(" + MM.RoundToFirstInterestingDigit(this.fullExtents.X) + ")";
        }
    }

}