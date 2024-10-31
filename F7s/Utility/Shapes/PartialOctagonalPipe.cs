using F7s.Utility.Geometry;
using Stride.Core.Mathematics;
using System;
namespace F7s.Utility.Shapes {


    public class PartialOctagonalPipe : CompoundShape {
        public PartialOctagonalPipe (float outerDiameter, float length, float internalDiameter, bool top, bool upperRight, bool right, bool lowerRight, bool bottom, bool lowerLeft, bool left, bool upperLeft) : base(new Vector3(outerDiameter, outerDiameter, length)) {

            float thickness = (outerDiameter - internalDiameter) / 2.0f;
            float width = (MathF.Sqrt(2) - 1.0f) * outerDiameter;

            Box GenerateWall () {
                if (width <= 0) {
                    throw new ArgumentException("Width " + width + " from arguments diameter " + outerDiameter + " length " + length + " internalDiameter " + internalDiameter + ".");
                }
                if (thickness <= 0) {
                    throw new ArgumentException("Thickness " + thickness + " from arguments diameter " + outerDiameter + " length " + length + " internalDiameter " + internalDiameter + ".");
                }
                if (length <= 0) {
                    throw new ArgumentException("Length " + length + " from arguments diameter " + outerDiameter + " length " + length + " internalDiameter " + internalDiameter + ".");
                }
                return new Box(width, thickness, length);
            }

            void Constitute (float rotation) {
                float longitude = rotation < 180 ? 90 : 270;
                float latitude = MM.PingPong(rotation, 180.0f) - 90.0f;
                Vector3 position = new PolarCoordinatesD(longitude, latitude, (outerDiameter / 2.0f) - (thickness / 2.0f)).ToVector3();

                AddConstituent(new Constituent(GenerateWall(), position, new Vector3(0, 0, rotation), "Partial " + GetType().Name + " " + rotation + Chars.degree));
            }

            if (bottom) {
                Constitute(0);
            }
            if (lowerLeft) {
                Constitute(45);
            }
            if (left) {
                Constitute(90);
            }
            if (upperLeft) {
                Constitute(135);
            }
            if (top) {
                Constitute(180);
            }
            if (upperRight) {
                Constitute(225);
            }
            if (right) {
                Constitute(270);
            }
            if (lowerRight) {
                Constitute(315);
            }
        }
    }

}