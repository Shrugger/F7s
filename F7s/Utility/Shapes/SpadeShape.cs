using Stride.Core.Mathematics; using F7s.Utility.Geometry.Double; using Stride.Core.Mathematics;
using System; using F7s.Utility.Geometry.Double; using Stride.Core.Mathematics;

namespace F7s.Utility.Shapes {


    public class SpadeShape : CompoundShape {

        public SpadeShape (Vector3 size)
            : this(size.Z, size.Y, size.X) { }

        public SpadeShape (float overallLength, float width, float thickness) : base(new Vector3(thickness, width, overallLength)) {
            float pointDiameter = width / MathF.Sqrt(2);

            Shape3Dim bladeShape = new Box(thickness, width, overallLength - (pointDiameter / 2.0));
            Shape3Dim pointShape = new Box(thickness, pointDiameter, pointDiameter);

            this.AddConstituent(new Constituent(bladeShape, new Vector3(0, 0, -(pointDiameter / 4.0f)), Vector3.Zero, "Blade"));
            this.AddConstituent(new Constituent(pointShape, new Vector3(0, 0, (overallLength / 2.0f) - (pointDiameter / 2.0f)), new Vector3(45, 0, 0), "Point"));
        }
    }

}