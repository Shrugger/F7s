using Stride.Core.Mathematics; using F7s.Utility.Geometry.Double; using Stride.Core.Mathematics;

namespace F7s.Utility.Shapes {
    
    public class ClosedBottomBox : ComplexHollowBox {
        private readonly Box internalShape;
        private readonly Vector3 internalShapeOffset;

        public ClosedBottomBox (Vector3 fullExtents, float wallThickness) : this(fullExtents.X, fullExtents.Y, fullExtents.Z, wallThickness) {
        }

        public ClosedBottomBox (float width, float height, float length, float wallThickness) : base(width, height, length, wallThickness, true, true, false, true, true, true) {
            this.internalShape = new Box(width - wallThickness * 2.0f, height - wallThickness, length - wallThickness * 2.0f);

            this.internalShapeOffset = Vector3.UnitY * wallThickness;
        }


        public override Shape3Dim GetInternalNegativeSpace () {
            return this.internalShape;
        }
        public override Vector3 InternalNegativeShapeOffset () {
            return this.internalShapeOffset;
        }
    }

}