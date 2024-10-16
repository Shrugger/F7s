using System;

namespace F7s.Utility.Shapes {
    [System.Serializable]
    public class SlopeShape : CompoundShape {
        public readonly float height;
        public readonly float baseLength;
        public readonly float angle;
        public SlopeShape(float baseLength, float angle) : base() {
            this.baseLength = baseLength;
            this.angle = angle;

            /*
             
            a²+b²=c²
             
             */

            /*
             
            TOP RIGHT VERTEX:
Top_Right.X = center.X + ((width / 2) * cos(angle)) - ((height / 2) * sin(angle))
Top_Right.Y = center.Y + ((width / 2) * sin(angle)) + ((height / 2) * cos(angle))



TOP LEFT VERTEX:
Top_Left.X = center.X - ((width / 2) * cos(angle)) - ((height / 2) * sin(angle))
Top_Left.Y = center.Y - ((width / 2) * sin(angle)) + ((height / 2) * cos(angle))



BOTTOM LEFT VERTEX:
Bot_Left.X = center.X - ((width / 2) * cos(angle)) + ((height / 2) * sin(angle))
Bot_Left.Y = center.Y - ((width / 2) * sin(angle)) - ((height / 2) * cos(angle))



BOTTOM RIGHT VERTEX:
Bot_Right.X = center.X + ((width / 2) * cos(angle)) + ((height / 2) * sin(angle))
Bot_Right.Y = center.Y + ((width / 2) * sin(angle)) - ((height / 2) * cos(angle))
             
             */

            // AddConstituent(new Box()
            throw new NotImplementedException("Aaaaaah trigonometry!");
        }
    }

}