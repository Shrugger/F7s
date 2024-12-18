﻿using F7s.Utility.Geometry.Double;
using F7s.Utility.Shapes;
using Stride.Core.Mathematics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace F7s.Utility {

    /// <summary>
    /// Mathematik.
    /// </summary>
    public static class MM {

        private const float DefaultValidationTolerance = 0.0001f;

        public static Double3 ForwardD => -Double3.UnitZ;
        public static Double3 BackwardD => Double3.UnitZ;
        public static Double3 LeftD => -Double3.UnitX;
        public static Double3 RightD => Double3.UnitX;
        public static Double3 DownD => -Double3.UnitY;
        public static Double3 UpD => Double3.UnitY;

        public static Vector3 Forward => -Vector3.UnitZ;
        public static Vector3 Backward => Vector3.UnitZ;
        public static Vector3 Left => -Vector3.UnitX;
        public static Vector3 Right => Vector3.UnitX;
        public static Vector3 Down => -Vector3.UnitY;
        public static Vector3 Up => Vector3.UnitY;


        public static float ApproachLogarithmically (float value, float limit = 1, float floor = 0, float speed = 1, float log = 2) {
            Debug.Assert(log > 1);
            float logValue = (float) Math.Log(value == 0 ? 1 : value, log);
            Debug.Assert(!float.IsNaN(logValue));
            return Approach(logValue, limit, floor, speed);
        }
        public static double ApproachExponentially (float value, float limit = 1, float floor = 0, float speed = 1, float inversePower = 1) {
            return Approach((float) Math.Pow(value, 1f / inversePower), limit, floor, speed);
        }

        public static float Approach (float value, float limit = 1, float floor = 0, float speed = 1) {
            Debug.Assert(!float.IsNaN(value));
            Debug.Assert(!float.IsNaN(speed));
            Debug.Assert(!float.IsNaN(limit));
            Debug.Assert(!float.IsNaN(floor));
            if (limit == floor) {
                return limit;
            }
            if (limit < floor) {
                return floor - Approach(value, floor, limit, speed);
            }
            float range = limit - floor;
            float polation = range / ((value * speed) + 1);
            float result = limit - polation;

            if (float.IsNaN(result)) {
                throw new Exception();
            }
            if (value > 0 && result < floor) {
                throw new Exception();
            }
            if (result > limit) {
                throw new Exception();
            }

            return result;

        }


        public static double InverseLerpClamped (double from, double to, double value) {
            if (from == to) {
                throw new ArgumentException(from + " == " + to);
            }

            if (from < to) {
                if (value < from) {
                    return 0d;
                }

                if (value > to) {
                    return 1d;
                }

                value -= from;
                value /= to - from;

                return value;
            }

            if (from <= to) {
                return 0d;
            }

            if (value < to) {
                return 1d;
            }

            if (value > from) {
                return 0d;
            }

            return 1.0d - ((value - to) / (from - to));
        }
        public static double InverseLerp (double from, double to, double value) {
            if (from == to) {
                throw new ArgumentException(from + " == " + to);
            }

            if (from < to) {
                value -= from;
                value /= to - from;

                return value;
            }

            return 1.0d - ((value - to) / (from - to));
        }
        public static double LerpClamped (double from, double to, double t) {
            return from + ((to - from) * Clamp01(value: t));
        }


        public static double Clamp01 (double value) {
            if (value < 0.0) {
                return 0.0d;
            }

            if (value > 1.0) {
                return 1d;
            }

            return value;
        }

        public static float DegToRad (float degrees) {
            const float factor = MathF.PI / 180f;
            return degrees * factor;
        }

        public static float RadToDeg (float radians) {
            const float factor = 180f / MathF.PI;
            return radians * factor;
        }
        public static double DegToRad (double degrees) {
            const double factor = Math.PI / 180.0;
            return degrees * factor;
        }

        public static double RadToDeg (double radians) {
            const double factor = 180.0 / Math.PI;
            return radians * factor;
        }

        public static bool IsEqualApprox (double a, double b, double delta = 0.00000001) {
            return Math.Abs(a - b) <= delta;
        }

        public static int Clamp (int value, int min, int max) {
            if (value < min) {
                return min;
            } else if (value > max) {
                return max;
            } else {
                return value;
            }
        }
        public static double Clamp (double value, double min, double max) {
            if (value < min) {
                return min;
            } else if (value > max) {
                return max;
            } else {
                return value;
            }
        }
        public static float Clamp (float value, float min, float max) {
            if (value < min) {
                return min;
            } else if (value > max) {
                return max;
            } else {
                return value;
            }
        }

        public static float Lerp (float a, float b, float v) {
            return a + ((b - a) * v);
        }

        public static double Lerp (double a, double b, double interpolationFactor) {
            return a + ((b - a) * interpolationFactor);
        }

        public static int FloorToInt (double v) {
            return (int) Math.Floor(v);
        }

        public static int CeilToInt (double v) {
            return (int) Math.Ceiling(v);
        }


        public static float Wrap (float rotation, float v1, float v2) {
            throw new NotImplementedException();
        }

        public static double RoundReasonably (double value, int overallDigits) {
            if (value < 1f) {
                overallDigits -= 2;

                if (Math.Log10(value) * -1f > overallDigits) {
                    overallDigits = RoundToInt(Math.Log10(value) * -1);
                }
            } else {
                if (Nachkommastellen(value: Round(value: value, decimals: overallDigits))) {
                    overallDigits -= 1;
                }

                overallDigits = RoundToInt(overallDigits - Math.Log10(value));
            }

            double rounded = Round(value: value, decimals: overallDigits);

            return rounded;
        }

        public static bool Nachkommastellen (double value) {
            return value % 1 == 0;
        }

        public static int RoundToInt (float value) {
            return RoundToInt(value);
        }
        public static int RoundToInt (double value) {
            return RoundToInt(value);
        }

        public static float Round (float value, int decimals) {
            return (float) Round(value: (double) value, decimals: decimals);
        }

        public static double Round (double value) {
            return Round(value: value, decimals: 0);
        }

        public static double Round (double value, int decimals) {
            return Math.Round(value: value, digits: Clamp(value: decimals, min: 0, max: 15));
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

        public static char Sum (IEnumerable<char> collection) {
            char sum = (char) 0;

            foreach (char item in collection) {
                sum += item;
            }

            return sum;
        }

        public static int Sum (IEnumerable<int> collection) {
            int sum = 0;

            foreach (int item in collection) {
                sum += item;
            }

            return sum;
        }

        public static float Sum (IEnumerable<float> collection) {
            float sum = 0;

            foreach (float item in collection) {
                sum += item;
            }

            return sum;
        }

        public static double Sum (IEnumerable<double> collection) {
            double sum = 0;

            foreach (double item in collection) {
                sum += item;
            }

            return sum;
        }

        public static double Sum<SourceType> (IEnumerable<SourceType> collection, Func<SourceType, double> function) {
            double sum = 0;

            foreach (SourceType item in collection) {
                sum += function.Invoke(item);
            }

            return sum;
        }

        public static float Sum<SourceType> (IEnumerable<SourceType> collection, Func<SourceType, float> function) {
            float sum = 0;

            foreach (SourceType item in collection) {
                sum += function.Invoke(item);
            }

            return sum;
        }
        public const double Deg2RadDouble = 0.01745329;
        public const double Rad2DegDouble = 57.29578;

        public static float AngularSizeInPixels (double realDiameter, double distance, float screenSize, float fieldOfView) {
            float realAngularSize = (float) (realDiameter / distance) * Rad2Deg;
            return realAngularSize * AngularSizeOfPixel(screenSize, fieldOfView);
        }

        public static double Logistic (double midpoint, double supremum, double steepness, double x) {
            return supremum / (1 + Math.Pow(Math.E, -steepness * (x - midpoint)));
        }

        public static double GreatCircleDistance (double lat1, double lon1, double lat2, double lon2, double radius = 1.0) {
            lat1 = Deg2RadDouble * lat1;
            lon1 = Deg2RadDouble * lon1;
            lat2 = Deg2RadDouble * lat2;
            lon2 = Deg2RadDouble * lon2;
            double d_lat = lat2 - lat1;
            double d_lon = lon2 - lon1;
            double h = (Math.Sin(d_lat / 2) * Math.Sin(d_lat / 2)) +
                (Math.Cos(lat1) * Math.Cos(lat2) *
                Math.Sin(d_lon / 2) * Math.Sin(d_lon / 2));
            return 2 * radius * Math.Asin(Math.Sqrt(h));
        }
        public static double FrustrumVolume (double minimumAltitude, double maximumAltitude, double crossSectionAreAtMinimumAltitude, double crossSectionAreAtMaximumAltitude) {
            double frustrumHeight = maximumAltitude - minimumAltitude;
            double lowerSurfaceArea = crossSectionAreAtMinimumAltitude;
            double upperSurfaceArea = crossSectionAreAtMaximumAltitude;
            double volume = frustrumHeight / 3.0f * (lowerSurfaceArea + upperSurfaceArea + Math.Sqrt(lowerSurfaceArea * upperSurfaceArea));
            return volume;
        }

        public static double TriangleSurfaceArea (Double3 v1, Double3 v2, Double3 v3) {

            double side1 = Double3.Distance(v1, v2);
            double side2 = Double3.Distance(v2, v3);
            double side3 = Double3.Distance(v3, v1);
            double s = (side1 + side2 + side3) / 2.0f;
            double preArea = s * (s - side1) * (s - side2) * (s - side3);
            double area = Math.Sqrt(preArea);

            return area;
        }

        public static double AngularRadiusInDegreesFromDistanceAndRadius (double distance, double radius) {
            return AngularRadiusInDegreesFromDistanceAndDiameter(distance, radius * 2.0f);
        }

        public static double AngularRadiusInDegreesFromDistanceAndDiameter (double distance, double diameter) {
            return AngularDiameterInDegreesFromDistanceAndDiameter(distance, diameter) / 2.0f;
        }

        public static double AngularDiameterInDegreesFromDistanceAndRadius (double distance, double radius) {
            return AngularDiameterInDegreesFromDistanceAndDiameter(distance: distance, diameter: radius * 2.0f);
        }

        public static double AngularDiameterInRadiansFromDistanceAndDiameter (double distance, double diameter) {
            // Source: https://rechneronline.de/sehwinkel/angular-diameter.php
            // α = 2*arctan(g/(2r)), where g = real size, r = distance, α = angular diameter or apparent size, parallax

            double alpha = 2 * Math.Atan(diameter / (2 * distance));

            return alpha;
        }


        public static double AngularDiameterInDegreesFromDistanceAndDiameter (double distance, double diameter) {
            double result = MM.RadToDeg(AngularDiameterInRadiansFromDistanceAndDiameter(distance, diameter));
            return result;
        }

        public static bool PointIsOnLine (Double3 point, Double3 lineStart, Double3 lineEnd) {
            double pointDistance = Double3.Distance(lineStart, point) + Double3.Distance(lineEnd, point);
            double lineLength = Double3.Distance(lineStart, lineEnd);
            if (MM.IsEqualApprox(lineLength, pointDistance)) {
                return true; // C is on the line.
            }
            return false;    // C is not on the line.
        }

        public static double CylinderSurfaceArea (double radius, double height) {
            double flatSurface = CircleArea(radius);
            double sideSurface = CylinderSideSurfaceArea(radius, height);
            return sideSurface + (flatSurface * 2.0f);
        }
        public static double CylinderSideSurfaceArea (double radius, double height) {
            double sideSurface = 2.0f * Math.PI * radius * height;
            return sideSurface;
        }

        public static double DistanceFromAngularDiameterAndSize (double angularDiameter, double realDiameter) {
            return realDiameter / (2f * Math.Tan(angularDiameter / 2f));
        }

        public static double DiameterFromAngularSizeAndDistance (double angularDiameter, double distance) {
            return 2f * Math.Tan(angularDiameter / 2f);
        }

        public static double CircleDiameterFromSurfaceArea (double surfaceArea) {
            return Math.Sqrt(surfaceArea / Math.PI) * 2;
        }

        public static double SphereDiameterFromVolume (double volume) {
            return Math.Pow(volume / (Math.PI * (4f / 3f)), 1f / 3f) * 2;
        }

        public static double SphereVolumeFromDiameter (double diameter) {
            return Math.Pow(diameter * 0.5f, 3) * Math.PI * (4f / 3f);
        }

        public static double SphereSurfaceArea (double diameter) {
            return Math.Pow(diameter * 0.5f, 2) * Math.PI * 4f;
        }

        public static double CubeVolumeFromDimensions (Double3 dimensions) {
            return dimensions.X * dimensions.Y * dimensions.Z;
        }

        public static double CylinderVolumeFromDimensions (Double3 dimensions) {
            return dimensions.Y * Math.PI * Math.Pow(0.5f * dimensions.X, 2);
        }

        public static Double3 DegreesToFractions (Double3 degrees) {
            return degrees / 360f;
        }

        public static Double3 FractionsToDegrees (Double3 fractions) {
            return fractions * 360f;
        }


        public static QuaternionD FractionsToQuaternion (Double3 fractions) {
            return DegreesToQuaternion(degrees: FractionsToDegrees(fractions: fractions));
        }

        public static Double3 QuaternionToDegreesDouble (QuaternionD quaternion) {
            return QuaternionToDegrees(quaternion);
        }

        public static Double3 QuaternionToFractionsDouble (QuaternionD quaternion) {
            return DegreesToFractions(degrees: QuaternionToDegreesDouble(quaternion));
        }

        public static bool PointIsInTriangle (Vector2 point, Vector2 corner1, Vector2 corner2, Vector2 corner3) {
            // Calculated using barycentric coordinates
            double s = (corner1.Y * corner3.X)
                     - (corner1.X * corner3.Y)
                     + ((corner3.Y - corner1.Y) * point.X)
                     + ((corner1.X - corner3.X) * point.Y);

            double t = (corner1.X * corner2.Y)
                     - (corner1.Y * corner2.X)
                     + ((corner1.Y - corner2.Y) * point.X)
                     + ((corner2.X - corner1.X) * point.Y);

            if ((s < 0) != (t < 0)) {
                return false;
            }

            double a = (-corner2.Y * corner3.X)
                     + (corner1.Y * (corner3.X - corner2.X))
                     + (corner1.X * (corner2.Y - corner3.Y))
                     + (corner2.X * corner3.Y);

            if (a < 0.0f) {
                s = -s;
                t = -t;
                a = -a;
            }

            return s > 0 && t > 0 && s + t < a;
        }

        public static Double3 ScaleBoxToVolume (Double3 boxExtents, double volume) {
            double originalVolume = Box.Volume(fullExtents: (Vector3) boxExtents);
            double scaleFactor = volume / originalVolume;
            double factorRoot = Math.Pow(scaleFactor, 1.0 / 3.0);
            Double3 scaledExtents = boxExtents * factorRoot;

            return scaledExtents;
        }
        public static double SphereRadiusFromVolume (double volume) {
            return Math.Pow(volume * 3.0f / (4.0f * Math.PI), 1.0f / 3.0f);
        }

        public static double VolumeFromSphereRadius (double radius) {
            return 4.0f / 3.0f * Math.PI * radius * radius * radius;
        }

        public static double CircumferenceFromRadius (double radius) {
            return Math.PI * radius * 2.0f;
        }

        public static float Angle (Vector3 a, Vector3 b) {
            if (a == b) {
                throw new Exception();
            }
            float aLength = a.Length();
            if (aLength == 0) {
                throw new Exception();
            }
            float bLength = b.Length();
            if (bLength == 0) {
                throw new Exception();
            }
            float dot = Vector3.Dot(a, b);
            float lengthProduct = aLength * bLength;
            float ratio = dot / lengthProduct;
            float clampedRatio = Clamp(ratio, -1f, 1f);
            float result = MathF.Acos(clampedRatio);
            return result;
        }
        public static double Angle (Double3 a, Double3 b) {
            return Math.Acos(Double3.Dot(a, b) / (a.Length() * b.Length()));
        }

        public static bool FacesTowards (Double3 position, Double3 facing, Double3 observer) {
            return Angle(facing, observer - position) < 90;
        }

        public static double CircleArea (double radius) {
            return radius * radius * Math.PI;
        }

        public static double CylinderVolume (double radius, double height) {
            return CircleArea(radius: radius) * height;
        }

        public static double CylinderHeight (double radius, double volume) {
            return volume / (Math.PI * radius * radius);
        }

        public static bool LinesOverlap (Vector2 start1, Vector2 end1, Vector2 start2, Vector2 end2) {
            double startX = 0;
            double endX = 0;
            double firstY1 = 0;
            double firstY2 = 0;
            double lastY1 = 0;
            double lastY2 = 0;
            double inclination1 = (end1.Y - start1.Y) / (end1.X - start1.X);
            double inclination2 = (end2.Y - start2.Y) / (end2.X - start2.X);
            double firstYDifference = 0;
            double lastYDifference = 0;

            if (start1.X > start2.X) {
                startX = start1.X;
                firstY1 = start1.Y;
                firstY2 = startX * inclination2 * (start2.Y - (inclination2 * start2.X));
            } else {
                startX = start2.X;
                firstY2 = start2.Y;
                firstY1 = startX * inclination1 * (start1.Y - (inclination1 * start1.X));
            }

            if (end1.X > end2.X) {
                endX = end1.X;
                lastY1 = end1.Y;
                lastY2 = endX * inclination2 * (end2.Y - (inclination2 * end2.X));
            } else {
                endX = end2.X;
                lastY2 = end2.Y;
                lastY1 = startX * inclination1 * (end1.Y - (inclination1 * end1.X));
            }

            firstYDifference = firstY2 - firstY1;
            lastYDifference = lastY2 - lastY1;

            if ((firstYDifference < 0) != (lastYDifference < 0)) {
                return true;
            }

            return false;
        }
        public static bool OneCardinalDirectionOnly (Double3 vector) {
            return (vector.X != 0 ? 1 : 0) + (vector.Y != 0 ? 1 : 0) + (vector.Z != 0 ? 1 : 0) == 1;
        }

        public static double DistanceOfPointToRay (Double3 point, Double3 rayOrigin, Double3 rayDirection) {
            return Double3.Cross(rayDirection, point - rayOrigin).Length();
        }

        public static Double3 InvertRotation (Double3 rotationInDegrees) {
            return QuaternionToDegreesDouble(Inverse(DegreesToQuaternion(rotationInDegrees)));
        }


        public static bool ApproximatelyEquals (Quaternion q1, Quaternion q2, double delta = 0.0000004f) {

            if (q1 == default) {
                throw new ArgumentException();
            }
            if (q2 == default) {
                throw new ArgumentException();
            }
            double angle = Math.Abs(Quaternion.AngleBetween(q1, q2));
            return angle <= delta;
        }

        public const float Deg2Rad = 0.01745329f;
        public const float Rad2Deg = 57.29578f;

        public static float AngularSizeOfPixel (float screenSize, float fieldOfView) {
            return screenSize / fieldOfView;
        }

        public static float AngularSizeInPixels (float realDiameter, float distance, float screenSize, float fieldOfView) {
            float realAngularSize = realDiameter / distance * Rad2Deg;
            return realAngularSize * AngularSizeOfPixel(screenSize, fieldOfView);
        }

        public static float Logistic (float midpoint, float supremum, float steepness, float x) {
            return supremum / (1 + MathF.Pow(MathF.E, -steepness * (x - midpoint)));
        }

        /// <returns>Positive value if separating, negative of approaching, zero if neither.</returns>
        public static float ApproachOrSeparate (Vector2 position1, Vector2 velocity1, Vector2 position2, Vector2 velocity2) {
            Vector2 p = position2 - position1;
            Vector2 v = velocity2 - velocity1;
            float dot = Vector2.Dot(v, p);
            return dot;
        }

        public static float ApproachOrSeparate (Vector2 relativePosition, Vector2 relativeVelocity) {
            float dot = Vector2.Dot(relativeVelocity, relativePosition);
            return dot;
        }

        public static float GreatCircleDistance (float lat1, float lon1, float lat2, float lon2, float radius = 1.0f) {
            lat1 = Deg2Rad * lat1;
            lon1 = Deg2Rad * lon1;
            lat2 = Deg2Rad * lat2;
            lon2 = Deg2Rad * lon2;
            float d_lat = lat2 - lat1;
            float d_lon = lon2 - lon1;
            float h = (MathF.Sin(d_lat / 2) * MathF.Sin(d_lat / 2)) +
                (MathF.Cos(lat1) * MathF.Cos(lat2) *
                MathF.Sin(d_lon / 2) * MathF.Sin(d_lon / 2));
            return 2 * radius * MathF.Asin(MathF.Sqrt(h));
        }
        public static float FrustrumVolume (float minimumAltitude, float maximumAltitude, float crossSectionAreAtMinimumAltitude, float crossSectionAreAtMaximumAltitude) {
            float frustrumHeight = maximumAltitude - minimumAltitude;
            float lowerSurfaceArea = crossSectionAreAtMinimumAltitude;
            float upperSurfaceArea = crossSectionAreAtMaximumAltitude;
            float volume = frustrumHeight / 3.0f * (lowerSurfaceArea + upperSurfaceArea + MathF.Sqrt(lowerSurfaceArea * upperSurfaceArea));
            return volume;
        }

        public static float TriangleSurfaceArea (Vector3 v1, Vector3 v2, Vector3 v3) {

            float side1 = Vector3.Distance(v1, v2);
            float side2 = Vector3.Distance(v2, v3);
            float side3 = Vector3.Distance(v3, v1);
            float s = (side1 + side2 + side3) / 2.0f;
            float preArea = s * (s - side1) * (s - side2) * (s - side3);
            float area = MathF.Sqrt(preArea);

            return area;
        }

        public static float AngularRadiusInDegreesFromDistanceAndRadius (float distance, float radius) {
            return AngularRadiusInDegreesFromDistanceAndDiameter(distance, radius * 2.0f);
        }

        public static float AngularRadiusInDegreesFromDistanceAndDiameter (float distance, float diameter) {
            return AngularDiameterInDegreesFromDistanceAndDiameter(distance, diameter) / 2.0f;
        }

        public static float AngularDiameterInDegreesFromDistanceAndRadius (float distance, float radius) {
            return AngularDiameterInDegreesFromDistanceAndDiameter(distance: distance, diameter: radius * 2.0f);
        }

        public static float AngularDiameterInRadiansFromDistanceAndDiameter (float distance, float diameter) {
            // Source: https://rechneronline.de/sehwinkel/angular-diameter.php
            // α = 2*arctan(g/(2r)), where g = real size, r = distance, α = angular diameter or apparent size, parallax

            float alpha = 2 * MathF.Atan(diameter / (2 * distance));

            return alpha;
        }


        public static float AngularDiameterInDegreesFromDistanceAndDiameter (float distance, float diameter) {
            return AngularDiameterInRadiansFromDistanceAndDiameter(distance, diameter) * Rad2Deg;
        }

        public static bool PointIsOnLine (Vector3 point, Vector3 lineStart, Vector3 lineEnd) {
            float pointDistance = Vector3.Distance(lineStart, point) + Vector3.Distance(lineEnd, point);
            float lineLength = Vector3.Distance(lineStart, lineEnd);
            if (MM.IsEqualApprox(lineLength, pointDistance)) {
                return true; // C is on the line.
            }
            return false;    // C is not on the line.
        }

        public static float CylinderSurfaceArea (float radius, float height) {
            float flatSurface = CircleArea(radius);
            float sideSurface = CylinderSideSurfaceArea(radius, height);
            return sideSurface + (flatSurface * 2.0f);
        }
        public static float CylinderSideSurfaceArea (float radius, float height) {
            float sideSurface = 2.0f * MathF.PI * radius * height;
            return sideSurface;
        }

        public static float DistanceFromAngularDiameterAndSize (float angularDiameter, float realDiameter) {
            return realDiameter / (2f * MathF.Tan(angularDiameter / 2f));
        }

        public static float DiameterFromAngularSizeAndDistance (float angularDiameter, float distance) {
            return 2f * MathF.Tan(angularDiameter / 2f);
        }

        public static float CircleDiameterFromSurfaceArea (float surfaceArea) {
            return MathF.Sqrt(surfaceArea / MathF.PI) * 2;
        }

        public static float SphereDiameterFromVolume (float volume) {
            return MathF.Pow(volume / (MathF.PI * (4f / 3f)), 1f / 3f) * 2;
        }

        public static float SphereVolumeFromDiameter (float diameter) {
            return MathF.Pow(diameter * 0.5f, 3) * MathF.PI * (4f / 3f);
        }

        public static float SphereSurfaceArea (float diameter) {
            return MathF.Pow(diameter * 0.5f, 2) * MathF.PI * 4f;
        }

        public static float CubeVolumeFromDimensions (Vector3 dimensions) {
            return dimensions.X * dimensions.Y * dimensions.Z;
        }

        public static float CylinderVolumeFromDimensions (Vector3 dimensions) {
            return dimensions.Y * MathF.PI * MathF.Pow(0.5f * dimensions.X, 2);
        }

        public static Vector3 DegreesToFractions (Vector3 degrees) {
            return degrees / 360f;
        }

        public static Vector3 FractionsToDegrees (Vector3 fractions) {
            return fractions * 360f;
        }

        public static Quaternion DegreesToQuaternion (Vector3 degrees) {
            if (degrees == Vector3.Zero) {
                return Quaternion.Identity;
            } else {
                throw new NotImplementedException();
            }
        }
        public static QuaternionD DegreesToQuaternion (Double3 degrees) {
            if (degrees == Double3.Zero) {
                return QuaternionD.Identity;
            } else {
                throw new NotImplementedException();
            }
        }

        public static Quaternion FractionsToQuaternion (Vector3 fractions) {
            return DegreesToQuaternion(degrees: FractionsToDegrees(fractions: fractions));
        }

        public static Vector3 QuaternionToDegrees (Quaternion quaternion) {
            throw new NotImplementedException();
        }

        public static Vector3 QuaternionToFractions (Quaternion quaternion) {
            return DegreesToFractions(degrees: QuaternionToDegrees(quaternion));
        }
        public static Double3 QuaternionToDegrees (QuaternionD quaternion) {
            throw new NotImplementedException();
        }

        public static Double3 QuaternionToFractions (QuaternionD quaternion) {
            return DegreesToFractions(degrees: QuaternionToDegrees(quaternion));
        }

        public static Vector3 ScaleBoxToVolume (Vector3 boxExtents, float volume) {
            float originalVolume = Box.Volume(fullExtents: boxExtents);
            float scaleFactor = volume / originalVolume;
            float factorRoot = MathF.Pow(scaleFactor, 1f / 3f);
            Vector3 scaledExtents = boxExtents * factorRoot;

            return scaledExtents;
        }

        public static Vector2 Random2DPointInTriangle (Vector2 c1, Vector2 c2, Vector2 c3) {
            //P = aA + bB + cC
            //@see htt//www.cgafaq.info/wiki/Random_Point_In_Triangle
            float a = Alea.Float();
            float b = Alea.Float();

            if (a + b > 1) {
                a = 1 - a;
                b = 1 - b;
            }

            float c = 1 - a - b;
            float rndX = (a * c1.X) + (b * c2.X) + (c * c3.X);
            float rndY = (a * c1.Y) + (b * c2.Y) + (c * c3.Y);

            return new Vector2(x: rndX, y: rndY);
        }

        public static float SphereRadiusFromVolume (float volume) {
            return MathF.Pow(volume * 3.0f / (4.0f * MathF.PI), 1.0f / 3.0f);
        }

        public static float VolumeFromSphereRadius (float radius) {
            return 4.0f / 3.0f * MathF.PI * radius * radius * radius;
        }

        public static float CircumferenceFromRadius (float radius) {
            return MathF.PI * radius * 2.0f;
        }

        public static bool FacesTowards (Vector3 position, Vector3 facing, Vector3 observer) {
            return Angle(facing, observer - position) < 90;
        }

        public static float CircleArea (float radius) {
            return radius * radius * MathF.PI;
        }

        public static float CylinderVolume (float radius, float height) {
            return CircleArea(radius: radius) * height;
        }

        public static float CylinderHeight (float radius, float volume) {
            return volume / (MathF.PI * radius * radius);
        }

        public static bool OneCardinalDirectionOnly (Vector3 vector) {
            return (vector.X != 0 ? 1 : 0) + (vector.Y != 0 ? 1 : 0) + (vector.Z != 0 ? 1 : 0) == 1;
        }

        public static float DistanceOfPointToRay (Vector3 point, Vector3 rayOrigin, Vector3 rayDirection) {
            return Vector3.Cross(rayDirection, point - rayOrigin).Length();
        }

        public static Vector3 InvertRotation (Vector3 rotationInDegrees) {
            return QuaternionToDegrees(Inverse(DegreesToQuaternion(rotationInDegrees)));
        }

        private static Quaternion Inverse (Quaternion quaternion) {
            throw new NotImplementedException();
        }
        private static QuaternionD Inverse (QuaternionD quaternion) {
            throw new NotImplementedException();
        }

        public static bool ApproximatelyEquals (Quaternion q1, Quaternion q2, float delta = 0.0000004f) {

            if (q1 == default) {
                throw new ArgumentException();
            }
            if (q2 == default) {
                throw new ArgumentException();
            }
            float angle = MathF.Abs(Quaternion.AngleBetween(q1, q2));
            return angle <= delta;
        }

        public static Vector3 Normalize (Vector3 v) {
            return Vector3.Normalize(v);
        }
        public static Double3 Normalize (Double3 v) {
            return Double3.Normalize(v);
        }

        public static Vector3 Cross (Vector3 a, Vector3 b) {
            return Vector3.Cross(a, b);
        }

        public static Vector3 Slerp (Vector3 start, Vector3 end, float weight) {
            // Source: https://keithmaggio.wordpress.com/2011/02/15/math-magician-lerp-slerp-and-nlerp/

            if (MM.ApproximatelyEqual(start, end)) {
                throw new Exception(start + " == " + end + ".");
            }

            Quaternion startQ = Quaternion.LookRotation(start, Vector3.UnitY);
            Quaternion endQ = Quaternion.LookRotation(end, Vector3.UnitY);
            Quaternion resultQ = Quaternion.Slerp(startQ, endQ, weight);
            Vector3 result = resultQ * Vector3.UnitZ;

            if (!MM.Valid(result) || (weight != 0 && MM.ApproximatelyEqual(result, start)) || (weight != 1 && MM.ApproximatelyEqual(result, end))) {
                throw new Exception("Spherical interpolation failed from " + start + " to " + end + " by " + weight + " with " + result + ".");
            }
            return result;
        }

        public static bool ApproximatelyEqual (float a, float b, float delta) {
            return MathF.Abs(a - b) < delta;
        }

        public static bool ApproximatelyEqual (double a, double b, double delta) {
            return Math.Abs(a - b) < delta;
        }

        public static Vector2 Normalize (Vector2 v) {
            return Vector2.Normalize(v);
        }

        public static bool Valid (Vector3 v) {

            float x = v.X;
            float y = v.Y;
            float z = v.Z;

            if (!float.IsFinite(x) || !float.IsFinite(y) || !float.IsFinite(z)) {
                return false;
            }

            if (float.IsNaN(x) || float.IsNaN(y) || float.IsNaN(z)) {
                return false;
            }

            return true;
        }
        public static bool Valid (Double3 v) {

            double x = v.X;
            double y = v.Y;
            double z = v.Z;

            if (!double.IsFinite(x) || !double.IsFinite(y) || !double.IsFinite(z)) {
                return false;
            }

            if (double.IsNaN(x) || double.IsNaN(y) || double.IsNaN(z)) {
                return false;
            }

            return true;
        }

        public static bool Valid (Quaternion q) {
            return float.IsFinite(q.X) && float.IsFinite(q.Y) && float.IsFinite(q.Z) && float.IsFinite(q.W);
        }
        public static bool Valid (QuaternionD q) {
            return double.IsFinite(q.X) && double.IsFinite(q.Y) && double.IsFinite(q.Z) && double.IsFinite(q.W);
        }

        public static bool Invalid (Vector3 v) {
            return !Valid(v);
        }
        public static bool Invalid (Double3 v) {
            return !Valid(v);
        }


        public static bool ApproximatelyEqual (Vector3 expected, Vector3 actual, float tolerance = 0.0001f) {
            bool ComponentWise (float expected, float actual) {
                return Math.Abs(expected - actual) < tolerance;
            }
            return ComponentWise(expected.X, actual.X) && ComponentWise(expected.Y, actual.Y) && ComponentWise(expected.Z, actual.Z);
        }

        public static Vector3 Sum (IEnumerable<Vector3> vectors) {
            Vector3 sum = Vector3.Zero;
            foreach (Vector3 vector in vectors) {
                sum += vector;
            }
            return sum;
        }

        public static Vector3 Average (IEnumerable<Vector3> vectors) {
            Vector3 sum = Sum(vectors);
            Vector3 average = sum / vectors.Count();
            return average;
        }

        public static bool Positive (Vector3 scale) {
            return scale.X > 0 && scale.Y > 0 && scale.Z > 0;
        }

        public static bool AllPositiveOrAllNegative (Vector3 scale) {
            return (scale.X > 0 && scale.Y > 0 && scale.Z > 0) || (scale.X < 0 && scale.Y < 0 && scale.Z < 0);
        }
        public static bool AllPositivellOrNegativeOne (Vector3 scale, float tolerance) {
            return ApproximatelyEqual(Vector3.One, scale, tolerance) || ApproximatelyEqual(-Vector3.One, scale, tolerance);
        }

        public static bool ApproximatelyEqual (Double3 origin1, Double3 origin2, float delta) {
            throw new NotImplementedException();
        }


        public static bool ApproximatelyEqual (MatrixD a, MatrixD b, double delta = 0.001) {
            return
                ApproximatelyEqual(a.M11, b.M11, delta) &&
                ApproximatelyEqual(a.M12, b.M12, delta) &&
                ApproximatelyEqual(a.M13, b.M13, delta) &&
                ApproximatelyEqual(a.M14, b.M14, delta) &&
                ApproximatelyEqual(a.M21, b.M21, delta) &&
                ApproximatelyEqual(a.M22, b.M22, delta) &&
                ApproximatelyEqual(a.M23, b.M23, delta) &&
                ApproximatelyEqual(a.M24, b.M24, delta) &&
                ApproximatelyEqual(a.M31, b.M31, delta) &&
                ApproximatelyEqual(a.M32, b.M32, delta) &&
                ApproximatelyEqual(a.M33, b.M33, delta) &&
                ApproximatelyEqual(a.M34, b.M34, delta) &&
                ApproximatelyEqual(a.M41, b.M41, delta) &&
                ApproximatelyEqual(a.M42, b.M42, delta) &&
                ApproximatelyEqual(a.M43, b.M43, delta) &&
                ApproximatelyEqual(a.M44, b.M44, delta);
        }


        public static bool ApproximatelyEqual (Matrix a, Matrix b, float delta = 0.001f) {
            return
                ApproximatelyEqual(a.M11, b.M11, delta) &&
                ApproximatelyEqual(a.M12, b.M12, delta) &&
                ApproximatelyEqual(a.M13, b.M13, delta) &&
                ApproximatelyEqual(a.M14, b.M14, delta) &&
                ApproximatelyEqual(a.M21, b.M21, delta) &&
                ApproximatelyEqual(a.M22, b.M22, delta) &&
                ApproximatelyEqual(a.M23, b.M23, delta) &&
                ApproximatelyEqual(a.M24, b.M24, delta) &&
                ApproximatelyEqual(a.M31, b.M31, delta) &&
                ApproximatelyEqual(a.M32, b.M32, delta) &&
                ApproximatelyEqual(a.M33, b.M33, delta) &&
                ApproximatelyEqual(a.M34, b.M34, delta) &&
                ApproximatelyEqual(a.M41, b.M41, delta) &&
                ApproximatelyEqual(a.M42, b.M42, delta) &&
                ApproximatelyEqual(a.M43, b.M43, delta) &&
                ApproximatelyEqual(a.M44, b.M44, delta);
        }

        public static void AssertEqual (MatrixD a, MatrixD b, float delta = 0.001f) {
            Debug.Assert(ApproximatelyEqual(a, b, delta));
        }

        public static void ValidatePositional (MatrixD transform, float tolerance = DefaultValidationTolerance) {
            if (InvalidPositional(transform, tolerance)) {
                throw new Exception(transform.ToString());
            }
        }

        public static bool InvalidPositional (MatrixD transform, float tolerance = DefaultValidationTolerance) {
            return !ValidPositional(transform, tolerance);
        }
        public static bool InvalidScaled (MatrixD transform) {
            return !ValidScaled(transform);
        }

        public static bool ValidPositional (MatrixD transform, float tolerance = DefaultValidationTolerance) {
            return Valid(transform, true, tolerance);
        }

        public static bool ValidScaled (MatrixD transform) {
            return Valid(transform, false, 0);
        }

        public static bool Valid (Matrix m, bool expectUniformScale, float tolerance = DefaultValidationTolerance) {
            if (Matrix.Zero == m) {
                return false;
            }

            bool ValidCell (float cell) {
                return !double.IsNaN(cell) && double.IsFinite(cell);
            }

            if (!AllCells(m, ValidCell)) {
                return false;
            }

            return true;
        }
        public static bool Valid (MatrixD m, bool expectUniformScale, double tolerance = DefaultValidationTolerance) {
            if (MatrixD.Zero == m) {
                return false;
            }

            bool ValidCell (double cell) {
                return !double.IsNaN(cell) && double.IsFinite(cell);
            }

            if (!AllCells(m, ValidCell)) {
                return false;
            }

            return true;
        }

        public static bool AllCells (Matrix m, Func<float, bool> f) {
            return
                f.Invoke(m.M11) &&
                f.Invoke(m.M12) &&
                f.Invoke(m.M13) &&
                f.Invoke(m.M14) &&
                f.Invoke(m.M21) &&
                f.Invoke(m.M22) &&
                f.Invoke(m.M23) &&
                f.Invoke(m.M24) &&
                f.Invoke(m.M31) &&
                f.Invoke(m.M32) &&
                f.Invoke(m.M33) &&
                f.Invoke(m.M34) &&
                f.Invoke(m.M41) &&
                f.Invoke(m.M42) &&
                f.Invoke(m.M43) &&
                f.Invoke(m.M44);
        }
        public static bool AllCells (MatrixD m, Func<double, bool> f) {
            return
                f.Invoke(m.M11) &&
                f.Invoke(m.M12) &&
                f.Invoke(m.M13) &&
                f.Invoke(m.M14) &&
                f.Invoke(m.M21) &&
                f.Invoke(m.M22) &&
                f.Invoke(m.M23) &&
                f.Invoke(m.M24) &&
                f.Invoke(m.M31) &&
                f.Invoke(m.M32) &&
                f.Invoke(m.M33) &&
                f.Invoke(m.M34) &&
                f.Invoke(m.M41) &&
                f.Invoke(m.M42) &&
                f.Invoke(m.M43) &&
                f.Invoke(m.M44);
        }

        public static bool ValidCellValue (double value) {
            return double.IsFinite(value) && !double.IsNaN(value);
        }

        //
        // Summary:
        //     Returns the value wrapped between 0 and the length. If the limit is reached,
        //     the next value the function returned is decreased to the 0 side or increased
        //     to the length side (like a triangle wave). If length is less than zero, it becomes
        //     positive.
        //
        // Parameters:
        //   value:
        //     The value to pingpong.
        //
        //   length:
        //     The maximum value of the function.
        //
        // Returns:
        //     The ping-ponged value.
        public static float PingPong (float value, float length) {
            if (length == 0f) {
                return 0f;
            }

            return Math.Abs((Fract((value - length) / (length * 2f)) * length * 2f) - length);
            static float Fract (float value) {
                return value - MathF.Floor(value);
            }
        }

        //
        // Summary:
        //     Returns the value wrapped between 0 and the length. If the limit is reached,
        //     the next value the function returned is decreased to the 0 side or increased
        //     to the length side (like a triangle wave). If length is less than zero, it becomes
        //     positive.
        //
        // Parameters:
        //   value:
        //     The value to pingpong.
        //
        //   length:
        //     The maximum value of the function.
        //
        // Returns:
        //     The ping-ponged value.
        public static double PingPong (double value, double length) {
            if (length == 0.0) {
                return 0.0;
            }

            return Math.Abs((Fract((value - length) / (length * 2.0)) * length * 2.0) - length);
            static double Fract (double value) {
                return value - Math.Floor(value);
            }
        }

        public static QuaternionD DegreesToQuaternionD (Double3 vector3) {
            throw new NotImplementedException();
        }

        public static QuaternionD ExtractRotation (MatrixD m) {
            QuaternionD rotation;
            m.Decompose(out _, out rotation, out _);
            return rotation;
        }

        public static Quaternion ExtractRotation (Matrix m) {
            Quaternion rotation;
            m.Decompose(out _, out rotation, out _);
            return rotation;
        }

        public static MatrixD Inverse (MatrixD m) {
            MatrixD result;
            MatrixD.Invert(ref m, out result);
            return result;
        }

        public static Matrix Transformation (Vector3 translation, Quaternion rotation) {
            Matrix result;
            Vector3 scale = Vector3.One;
            Matrix.Transformation(ref scale, ref rotation, ref translation, out result);
            return result;
        }
        public static MatrixD Transformation (Double3 translation, QuaternionD rotation) {
            MatrixD result;
            Double3 scale = Double3.One;
            MatrixD.Transformation(ref scale, ref rotation, ref translation, out result);
            return result;
        }

        public static Matrix Downscale (MatrixD m) {
            return new Matrix(
                (float) m.M11, (float) m.M12, (float) m.M13, (float) m.M14,
                (float) m.M21, (float) m.M22, (float) m.M23, (float) m.M24,
                (float) m.M31, (float) m.M32, (float) m.M33, (float) m.M34,
                (float) m.M41, (float) m.M42, (float) m.M43, (float) m.M44
                );
        }
    }
}
