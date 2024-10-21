using Stride.Core.Mathematics; using F7s.Utility.Geometry.Double; using System; using F7s.Utility.Geometry.Double;
using System; using F7s.Utility.Geometry.Double;
using System.Collections.Generic;
using System.Linq;

namespace F7s.Utility.Shapes.Shapes2D {

    public class Square : Rectangle {
        public Square(float sideLength) : base(new Vector2(sideLength, sideLength)) {
        }
    }
    public class Rectangle : Shape2Dim {
        public Rectangle(Vector2 fullExtents) {
            this.fullExtents = fullExtents;
        }

        public readonly Vector2 fullExtents;

        public override float SurfaceArea() {
            return this.fullExtents.X * this.fullExtents.Y;
        }

        public override Shape3Dim Extrude(float depth) {
            return new Box(this.fullExtents.X, this.fullExtents.Y, depth);
        }
    }

    public abstract class CompoundShape2D : Shape2Dim {
        private List<Composant> composants;

        protected CompoundShape2D(params Composant[] composants) {
            this.composants = composants.ToList();
        }

        public override float SurfaceArea() {
            throw new NotImplementedException();
        }

        public class Composant {
            public readonly Shape2Dim shape;
            public readonly Vector2 relativePosition;
            public readonly float relativeClockwiseRotationInDegrees;
        }
        public override Shape3Dim Extrude(float depth) {
            CompoundShape shape = new CompoundShape();
            foreach (Composant composant in this.composants) {
                shape.AddConstituent(composant.shape.Extrude(depth), new Vector3(composant.relativePosition.X, composant.relativePosition.Y, 0), Vector3.UnitZ * composant.relativeClockwiseRotationInDegrees, composant.shape.ToString());
            }
            return shape;
        }
    }
}
