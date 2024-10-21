using Stride.Core.Mathematics; using F7s.Utility.Geometry.Double; using System; using F7s.Utility.Geometry.Double;

namespace F7s.Utility.Shapes {
    
    public class DoubleEdgedBladeShape : CompoundShape {

        public DoubleEdgedBladeShape(Vector3 size)
            : this(size.Z, size.Y, size.X) { }

        public DoubleEdgedBladeShape(float overallLength, float width, float thickness) : base(new Vector3(thickness, width, overallLength)) {

            float edgeThickness = thickness / MathF.Sqrt(2);
            Shape3Dim bladeShape = new Box(thickness, width, overallLength);
            Shape3Dim edgeShape = new CantedBox(edgeThickness, overallLength);
            Vector3 bladePosition = new Vector3(0, 0, 0);

            this.AddConstituent(bladeShape, bladePosition, Vector3.Zero, "Blade");
            this.AddConstituent(edgeShape, bladePosition + Vector3.UnitY * (width / 2.0f), Vector3.Zero, "False Edge");
            this.AddConstituent(edgeShape, bladePosition + -Vector3.UnitY * (width / 2.0f), Vector3.Zero, "True Edge");

        }
    }

}