using Stride.Core.Mathematics;
using System;

namespace F7s.Utility.Geometry {

    public struct AxesLengthsOrdered {


        public readonly double longestAxisLength;
        public readonly double intermediateAxisLength;
        public readonly double shortestAxisLength;

        public AxesLengthsOrdered (Double3 axes)
            : this(x: axes.X, y: axes.Y, z: axes.Z) { }
        public AxesLengthsOrdered (Vector3 axes)
            : this(x: axes.X, y: axes.Y, z: axes.Z) { }

        public AxesLengthsOrdered (double x, double y, double z) {
            if (x >= y && x >= z) {
                longestAxisLength = x;
                if (y >= z) {
                    intermediateAxisLength = y;
                    shortestAxisLength = z;
                } else if (z >= y) {
                    intermediateAxisLength = z;
                    shortestAxisLength = y;
                } else {
                    throw new Exception(x + " " + y + " " + z);
                }
            } else if (y >= x && y >= z) {
                longestAxisLength = y;
                if (x >= z) {
                    intermediateAxisLength = x;
                    shortestAxisLength = z;
                } else if (z >= x) {
                    intermediateAxisLength = z;
                    shortestAxisLength = x;
                } else {
                    throw new Exception(x + " " + y + " " + z);
                }
            } else if (z >= x && z >= y) {
                longestAxisLength = z;
                if (y >= x) {
                    intermediateAxisLength = y;
                    shortestAxisLength = x;
                } else if (x >= y) {
                    intermediateAxisLength = x;
                    shortestAxisLength = y;
                } else {
                    throw new Exception(x + " " + y + " " + z);
                }
            } else {
                throw new Exception(x + " " + y + " " + z);
            }
        }

        public static implicit operator AxesLengthsOrdered (Double3 vector) {
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