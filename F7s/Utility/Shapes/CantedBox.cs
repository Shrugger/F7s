using Stride.Core.Mathematics; using F7s.Utility.Geometry.Double; using Stride.Core.Mathematics;
using System; using F7s.Utility.Geometry.Double; using Stride.Core.Mathematics;

namespace F7s.Utility.Shapes {

    
    public class CantedBox : Box {

        public CantedBox (float diameter, float length) : base(diameter, diameter, length) {
        }


        public override Box GetBoundingBox () {
            Vector3 baseExtents = base.FullExtents();
            float sqr2 = MathF.Sqrt(2);
            return new Box(baseExtents.X * sqr2, baseExtents.Y * sqr2, baseExtents.Z);
        }

        public Vector3 RightwardRollAddition () {
            return new Vector3(0, 0, 45);
        }

        public override bool ShapeIsRolledFortyFiveDegrees () {
            return true;
        }
    }

}