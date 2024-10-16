using System;
using Stride.Core.Mathematics; using System;

namespace F7s.Utility.Shapes {
    [Serializable]
    public class ComplexHollowBox : CompoundShape {
        public ComplexHollowBox(float wallThickness, Vector3 internalExtents, bool left, bool right, bool top, bool bottom, bool front, bool back) : this(internalExtents.X + (wallThickness * 2), internalExtents.Y + (wallThickness * 2), internalExtents.Z + (wallThickness * 2), wallThickness, left, right, top, bottom, front, back) { }
        public ComplexHollowBox(Vector3 fullExtents, float wallThickness, bool left, bool right, bool top, bool bottom, bool front, bool back) : this(fullExtents.X, fullExtents.Y, fullExtents.Z, wallThickness, left, right, top, bottom, front, back) { }
        private readonly float wallThickness;
        private readonly bool left;
        private readonly bool right;
        private readonly bool top;
        private readonly bool bottom;
        private readonly bool front;
        private readonly bool back;
        public ComplexHollowBox(float width, float height, float length, float wallThickness, bool left, bool right, bool top, bool bottom, bool front, bool back) : base(new Vector3(width, height, length)) {
            this.wallThickness = wallThickness;
            this.left = left;
            this.right = right;
            this.top = top;
            this.bottom = bottom;
            this.front = front;
            this.back = back;
            float halfThickness = wallThickness / 2.0f;
            float halfWidth = width / 2.0f;
            float halfHeight = height / 2.0f;
            float halfLength = length / 2.0f;

            int walls = (right ? 1 : 0) + (left ? 1 : 0) + (top ? 1 : 0) + (bottom ? 1 : 0) + (front ? 1 : 0) + (back ? 1 : 0);

            if (walls <= 1) {
                throw new ArgumentException("This should be a primitive.");
            }

            if (top) {
                this.AddConstituent(new Constituent(new Box(width, wallThickness, length), new Vector3(0, halfHeight - halfThickness, 0), Vector3.Zero, "Top"));
            }

            if (bottom) {
                this.AddConstituent(new Constituent(new Box(width, wallThickness, length), new Vector3(0, -halfHeight + halfThickness, 0), Vector3.Zero, "Bottom"));
            }

            if (left) {
                this.AddConstituent(new Constituent(new Box(wallThickness, height, length), new Vector3(-halfWidth + halfThickness, 0, 0), Vector3.Zero, "Left"));
            }

            if (right) {
                this.AddConstituent(new Constituent(new Box(wallThickness, height, length), new Vector3(halfWidth - halfThickness, 0, 0), Vector3.Zero, "Right"));
            }

            if (front) {
                this.AddConstituent(new Constituent(new Box(width, height, wallThickness), new Vector3(0, 0, halfLength - halfThickness), Vector3.Zero, "Front"));
            }

            if (back) {
                this.AddConstituent(new Constituent(new Box(width, height, wallThickness), new Vector3(0, 0, -halfLength + halfThickness), Vector3.Zero, "Back"));
            }

        }
        public Box InternalNegativeBox() {
            return new Box(this.fullExtents.Value.X - this.wallThickness, this.fullExtents.Value.Y - this.wallThickness, this.fullExtents.Value.Z - this.wallThickness);
        }

        public override Shape3Dim GetInternalNegativeSpace() {
            return this.InternalNegativeBox();
        }

        public override ShapeType ShapeType() {
            return F7s.Utility.Shapes.ShapeType.Box;
        }

        public static explicit operator Box(ComplexHollowBox complexHollowBox) {
            return new Box(complexHollowBox.fullExtents.Value);
        }
        public override Shape3Dim GetShapePlusSize(float addition) {
            return new HollowBox(new Vector3(this.fullExtents.Value.X + addition, this.fullExtents.Value.Y + addition, this.fullExtents.Value.Z + addition), this.wallThickness);
        }
    }

}