using F7s.Utility.Mathematics;
using F7s.Utility.Shapes;
using Stride.Core.Mathematics;
using System;

namespace F7s.Utility.Geometry {

    public static class GeometryF {

        public const float Deg2Rad = 0.01745329f;
        public const float Rad2Deg = 57.29578f;

        public static float AngularSizeOfPixel (float screenSize, float fieldOfView) {
            return screenSize / fieldOfView;
        }

        public static float AngularSizeInPixels (float realDiameter, float distance, float screenSize, float fieldOfView) {
            float realAngularSize = realDiameter / distance * GeometryF.Rad2Deg;
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
            lat1 = GeometryF.Deg2Rad * lat1;
            lon1 = GeometryF.Deg2Rad * lon1;
            lat2 = GeometryF.Deg2Rad * lat2;
            lon2 = GeometryF.Deg2Rad * lon2;
            float d_lat = lat2 - lat1;
            float d_lon = lon2 - lon1;
            float h = MathF.Sin(d_lat / 2) * MathF.Sin(d_lat / 2) +
                MathF.Cos(lat1) * MathF.Cos(lat2) *
                MathF.Sin(d_lon / 2) * MathF.Sin(d_lon / 2);
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
            if (Mathematik.IsEqualApprox(lineLength, pointDistance)) {
                return true; // C is on the line.
            }
            return false;    // C is not on the line.
        }

        public static float CylinderSurfaceArea (float radius, float height) {
            float flatSurface = CircleArea(radius);
            float sideSurface = CylinderSideSurfaceArea(radius, height);
            return sideSurface + flatSurface * 2.0f;
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

        public static Quaternion FractionsToQuaternion (Vector3 fractions) {
            return DegreesToQuaternion(degrees: FractionsToDegrees(fractions: fractions));
        }

        public static Vector3 QuaternionToDegrees (Quaternion quaternion) {
            throw new NotImplementedException();
        }

        public static Vector3 QuaternionToFractions (Quaternion quaternion) {
            return DegreesToFractions(degrees: QuaternionToDegrees(quaternion));
        }

        public static bool PointIsInTriangle (Vector2 point, Vector2 corner1, Vector2 corner2, Vector2 corner3) {
            // Calculated using barycentric coordinates
            float s = corner1.Y * corner3.X
                     - corner1.X * corner3.Y
                     + (corner3.Y - corner1.Y) * point.X
                     + (corner1.X - corner3.X) * point.Y;

            float t = corner1.X * corner2.Y
                     - corner1.Y * corner2.X
                     + (corner1.Y - corner2.Y) * point.X
                     + (corner2.X - corner1.X) * point.Y;

            if (s < 0 != t < 0) {
                return false;
            }

            float a = -corner2.Y * corner3.X
                     + corner1.Y * (corner3.X - corner2.X)
                     + corner1.X * (corner2.Y - corner3.Y)
                     + corner2.X * corner3.Y;

            if (a < 0.0f) {
                s = -s;
                t = -t;
                a = -a;
            }

            return s > 0 && t > 0 && s + t < a;
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
            float rndX = a * c1.X + b * c2.X + c * c3.X;
            float rndY = a * c1.Y + b * c2.Y + c * c3.Y;

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
            return GeometryF.Angle(facing, observer - position) < 90;
        }

        private static int Angle (Vector3 a, Vector3 b) {
            throw new NotImplementedException();
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

        public static bool LinesOverlap (Vector2 start1, Vector2 end1, Vector2 start2, Vector2 end2) {
            float startX = 0;
            float endX = 0;
            float firstY1 = 0;
            float firstY2 = 0;
            float lastY1 = 0;
            float lastY2 = 0;
            float inclination1 = (end1.Y - start1.Y) / (end1.X - start1.X);
            float inclination2 = (end2.Y - start2.Y) / (end2.X - start2.X);
            float firstYDifference = 0;
            float lastYDifference = 0;

            if (start1.X > start2.X) {
                startX = start1.X;
                firstY1 = start1.Y;
                firstY2 = startX * inclination2 * (start2.Y - inclination2 * start2.X);
            } else {
                startX = start2.X;
                firstY2 = start2.Y;
                firstY1 = startX * inclination1 * (start1.Y - inclination1 * start1.X);
            }

            if (end1.X > end2.X) {
                endX = end1.X;
                lastY1 = end1.Y;
                lastY2 = endX * inclination2 * (end2.Y - inclination2 * end2.X);
            } else {
                endX = end2.X;
                lastY2 = end2.Y;
                lastY1 = startX * inclination1 * (end1.Y - inclination1 * end1.X);
            }

            firstYDifference = firstY2 - firstY1;
            lastYDifference = lastY2 - lastY1;

            if (firstYDifference < 0 != lastYDifference < 0) {
                return true;
            }

            return false;
        }
        public static bool OneCardinalDirectionOnly (Vector3 vector) {
            return (vector.X != 0 ? 1 : 0) + (vector.Y != 0 ? 1 : 0) + (vector.Z != 0 ? 1 : 0) == 1;
        }

        public static float DistanceOfPointToRay (Vector3 point, Vector3 rayOrigin, Vector3 rayDirection) {
            return Vector3.Cross(rayDirection, point - rayOrigin).Length();
        }

        public static Vector3 InvertRotation (Vector3 rotationInDegrees) {
            return GeometryF.QuaternionToDegrees(GeometryF.Inverse(GeometryF.DegreesToQuaternion(rotationInDegrees)));
        }

        private static Quaternion Inverse (Quaternion quaternion) {
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

        internal static Vector3 Normalize (Vector3 direction) {
            throw new NotImplementedException();
        }

        internal static Vector3 Cross (Vector3 a, Vector3 b) {
            throw new NotImplementedException();
        }

        internal static Vector3 Slerp (Vector3 position1, Vector3 position2, float weight) {
            throw new NotImplementedException();
        }

        internal static bool ApproximatelyEquals (float a, float b, float delta) {
            throw new NotImplementedException();
        }

        internal static Vector2 Normalize (Vector2 rheologicMovement) {
            throw new NotImplementedException();
        }

        internal static bool ApproximatelyEquals (Vector3 vector31, Vector3 vector32) {
            throw new NotImplementedException();
        }
    }

}