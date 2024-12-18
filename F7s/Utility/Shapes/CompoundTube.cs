﻿using F7s.Utility.Geometry;
using Stride.Core.Mathematics;
using System;
namespace F7s.Utility.Shapes {

    public class CompoundTube : CompoundShape {
        public readonly Tube tubeShape;

        public override string ToString () {
            return GetType().Name + " " + tubeShape.ToString();
        }

        public CompoundTube (Tube shape, int sides) {
            if (sides < 3) {
                throw new ArgumentException("Cannot create a compound tube out of merely " + sides + " sides.");
            }

            tubeShape = shape;

            float thickness = (shape.diameter - shape.internalDiameter) / 2.0f;
            float width = (MathF.Sqrt(2) - 1.0f) * shape.diameter;
            float length = shape.length;

            Box GenerateWall () {
                if (width <= 0) {
                    throw new ArgumentException("Width " + width + " from arguments diameter " + shape.diameter + " length " + length + " internalDiameter " + shape.internalDiameter + ".");
                }
                if (thickness <= 0) {
                    throw new ArgumentException("Thickness " + thickness + " from arguments diameter " + shape.diameter + " length " + length + " internalDiameter " + shape.internalDiameter + ".");
                }
                if (length <= 0) {
                    throw new ArgumentException("Length " + length + " from arguments diameter " + shape.diameter + " length " + length + " internalDiameter " + shape.internalDiameter + ".");
                }
                return new Box(width, thickness, length);
            }

            void Constitute (float rotation) {
                float longitude = rotation < 180 ? 90 : 270;
                float latitude = MM.Wrap(rotation, -90f, 90f);
                Vector3 position = new PolarCoordinatesD(longitude, latitude, (shape.diameter / 2.0f) - (thickness / 2.0f)).ToVector3();

                AddConstituent(new Constituent(GenerateWall(), position, new Vector3(0, 0, rotation), "Partial " + GetType().Name + " " + rotation + Chars.degree));
            }

            float degreesPerSide = 360f / sides;
            for (int i = 0; i < sides; i++) {
                Constitute(i * degreesPerSide);
            }
        }


        public override Shape3Dim GetInternalNegativeSpace () {
            return tubeShape.InternalNegativeCylinder();
        }
        public float WallThickness () {
            return (tubeShape.diameter - tubeShape.internalDiameter) / 2.0f;
        }

        public override Shape3Dim GetShapePlusSize (float addition) {
            return new Tube(tubeShape.diameter + addition, tubeShape.length + addition, tubeShape.internalDiameter);
        }

        public override float SubstantialVolume () {
            return tubeShape.SubstantialVolume();
        }

        public override bool Equals (object obj) {
            return obj is Tube tube &&
                   base.Equals(obj) &&
                   tubeShape.internalDiameter == tube.internalDiameter;
        }

        public override int GetHashCode () {
            var hashCode = -469255269;
            hashCode = (hashCode * -1521134295) + base.GetHashCode();
            hashCode = (hashCode * -1521134295) + tubeShape.internalDiameter.GetHashCode();
            return hashCode;
        }
    }

}