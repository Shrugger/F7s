using F7s.Utility.Geometry.Double;
using Stride.Core.Mathematics;
using System;

namespace F7s.Geometry {

    public struct AxesLengthsOrdered {


        public readonly double longestAxisLength;
        public readonly double intermediateAxisLength;
        public readonly double shortestAxisLength;

        public AxesLengthsOrdered (Vector3d axes)
            : this(x: axes.X, y: axes.Y, z: axes.Z) { }

        public AxesLengthsOrdered (double x, double y, double z) {
            if (x >= y && x >= z) {
                this.longestAxisLength = x;
                if (y >= z) {
                    this.intermediateAxisLength = y;
                    this.shortestAxisLength = z;
                } else if (z >= y) {
                    this.intermediateAxisLength = z;
                    this.shortestAxisLength = y;
                } else {
                    throw new Exception(x + " " + y + " " + z);
                }
            } else if (y >= x && y >= z) {
                this.longestAxisLength = y;
                if (x >= z) {
                    this.intermediateAxisLength = x;
                    this.shortestAxisLength = z;
                } else if (z >= x) {
                    this.intermediateAxisLength = z;
                    this.shortestAxisLength = x;
                } else {
                    throw new Exception(x + " " + y + " " + z);
                }
            } else if (z >= x && z >= y) {
                this.longestAxisLength = z;
                if (y >= x) {
                    this.intermediateAxisLength = y;
                    this.shortestAxisLength = x;
                } else if (x >= y) {
                    this.intermediateAxisLength = x;
                    this.shortestAxisLength = y;
                } else {
                    throw new Exception(x + " " + y + " " + z);
                }
            } else {
                throw new Exception(x + " " + y + " " + z);
            }
        }

        public static implicit operator AxesLengthsOrdered (Vector3d vector) {
            return new AxesLengthsOrdered(vector);
        }
        public static implicit operator AxesLengthsOrdered (Vector3 vector) {
            return new AxesLengthsOrdered(vector);
        }

        public static bool CanFitInside (AxesLengthsOrdered container, AxesLengthsOrdered content) {

            bool longestAxesFit = container.longestAxisLength >= content.longestAxisLength;
            bool intermediateAxesFit = container.intermediateAxisLength >= content.intermediateAxisLength;
            bool shortestAxesFit = container.shortestAxisLength >= content.shortestAxisLength;

            bool result = longestAxesFit && intermediateAxesFit && shortestAxesFit;
            return result;
        }

    }

}