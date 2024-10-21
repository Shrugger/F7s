using Stride.Core.Mathematics; using F7s.Utility.Geometry.Double; using Stride.Core.Mathematics; using System; using F7s.Utility.Geometry.Double; using Stride.Core.Mathematics;
using System; using F7s.Utility.Geometry.Double; using Stride.Core.Mathematics;

namespace F7s.Utility.Shapes {
    
    public class Octagon : CompoundShape {
        public Octagon(float width, float height, float length) : base(new Vector3(width, height, length)) {
            Box GenerateBox() {
                return new Box(width / 2.0f, height, length);
            }

            for (float rotation = 0; rotation < 180; rotation += 45) {
                this.AddConstituent(new Constituent(GenerateBox(), Vector3.Zero, new Vector3(0, 0, rotation), "Partial " + this.GetType().Name + " " + rotation + Chars.degree));
            }
        }
    }

}