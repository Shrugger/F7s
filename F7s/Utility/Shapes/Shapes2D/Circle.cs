using Stride.Core.Mathematics; using F7s.Utility.Geometry.Double; using Stride.Core.Mathematics; using System; using F7s.Utility.Geometry.Double; using Stride.Core.Mathematics;

namespace F7s.Utility.Shapes.Shapes2D {

    public class Circle : Shape2Dim {

        public readonly float radius;

        public Circle(float radius) {
            this.radius = radius;
        }

        public float Diameter() {
            return this.radius * 2.0f;
        }

        public override Shape3Dim Extrude(float depth) {
            return new Cylinder(this.radius * 2, depth);
        }

        public override float SurfaceArea() {
            return this.radius * MathF.PI * this.radius;
        }
    }

}