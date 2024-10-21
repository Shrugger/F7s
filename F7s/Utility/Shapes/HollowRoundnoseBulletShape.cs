using F7s.Utility.Shapes.Shapes2D;
using Stride.Core.Mathematics; using F7s.Utility.Geometry.Double; using System; using F7s.Utility.Geometry.Double;

namespace F7s.Utility.Shapes {
    
    public class HollowRoundnoseBulletShape : CompoundShape {
        public HollowRoundnoseBulletShape(float diameter, float length, float wallThickness) : base(new Vector3(diameter, diameter, length)) {
            this.AddConstituent(new HollowCylinder(diameter, length / 2.0f, diameter - wallThickness * 2, length / 2.0f - wallThickness * 2), new Vector3(0, 0, -length / 4.0f), Vector3.Zero, "Base");
            this.AddConstituent(new HollowCapsule(diameter, length, diameter - wallThickness * 2, length - wallThickness * 2), new Vector3(0, 0, 0), Vector3.Zero, "Nose");
        }

        public override Circle GetCircularCrossSection() {
            return new Circle(this.FullExtents().X / 2.0f);
        }

        public override ShapeType ShapeType() {
            return F7s.Utility.Shapes.ShapeType.Cylinder;
        }
    }

}