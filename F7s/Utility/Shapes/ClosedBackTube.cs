using Stride.Core.Mathematics; using F7s.Utility.Geometry.Double; using Stride.Core.Mathematics;

namespace F7s.Utility.Shapes {
    
    public class ClosedBackTube : Cylinder {
        private readonly Cylinder internalShape;
        private readonly Vector3 internalShapeOffset;
        public ClosedBackTube (float diameter, float length, float internalDiameter, float internalLength) : base(diameter, length) {
            this.internalShape = new Cylinder(internalDiameter, internalLength);
            float wallThickness = (length - internalLength) / 2.0f;

            this.internalShapeOffset = Vector3.UnitZ * wallThickness;
        }

        public override Shape3Dim GetInternalNegativeSpace () {
            return this.internalShape;
        }

        public override Vector3 InternalNegativeShapeOffset () {
            return this.internalShapeOffset;
        }

        public Cylinder InternalNegativeCylinder () {
            return this.GetInternalNegativeSpace() as Cylinder;
        }
    }

}