using F7s.Utility.Mathematics;
using Stride.Core.Mathematics;
using System;

namespace F7s.Utility {

    public static class Rounding {

        public static double RoundReasonably (double value, int overallDigits) {
            if (value < 1f) {
                overallDigits -= 2;

                if (Math.Log10(value) * -1f > overallDigits) {
                    overallDigits = Mathematik.RoundToInt(Math.Log10(value) * -1);
                }
            } else {
                if (Nachkommastellen(value: Round(value: value, decimals: overallDigits))) {
                    overallDigits -= 1;
                }

                overallDigits = Mathematik.RoundToInt(overallDigits - Math.Log10(value));
            }

            double rounded = Round(value: value, decimals: overallDigits);

            return rounded;
        }

        public static bool Nachkommastellen (double value) {
            return value % 1 == 0;
        }

        public static int RoundToInt (float value) {
            return Mathematik.RoundToInt(value);
        }
        public static int RoundToInt (double value) {
            return Mathematik.RoundToInt(value);
        }

        public static float Round (float value, int decimals) {
            return (float) Round(value: (double) value, decimals: decimals);
        }

        public static double Round (double value) {
            return Round(value: value, decimals: 0);
        }

        public static double Round (double value, int decimals) {
            return Math.Round(value: value, digits: Mathematik.Clamp(value: decimals, min: 0, max: 15));
        }

        public static double RoundToFirstInterestingDigit (double value, int additionalDecimals = 0) {
            double absoluteValue = Math.Abs(value);

            if (absoluteValue < 1) {

                double rawMagnitude = Math.Log10(absoluteValue);
                double absoluteMagnitude = Math.Abs(rawMagnitude);
                int roundedMagnitude = RoundToInt(Math.Ceiling(absoluteMagnitude));

                double roundedValue = Round(
                                                          value: value,
                                                          decimals: roundedMagnitude + additionalDecimals
                                                         );

                return roundedValue;
            }

            if (absoluteValue > 10) {
                return Round(value: value, decimals: 0);
            }

            return Round(value: value, decimals: additionalDecimals);
        }

        public static string Round (Vector3 v) {
            return "(" + RoundToInt(v.X) + ", " + RoundToInt(v.Y) + ", " + RoundToInt(v.Z) + ")";
        }

        public static string RoundToFirstInterestingDigit (Vector3 v, int additionalDecimals = 0) {
            return "(" +
                RoundToFirstInterestingDigit(v.X, additionalDecimals) + ", " +
                RoundToFirstInterestingDigit(v.Y, additionalDecimals) + ", " +
                RoundToFirstInterestingDigit(v.Z, additionalDecimals) +
                ")";
        }
    }

}