using F7s.Utility.Mathematics;
using F7s.Utility.Shapes;
using Stride.Core.Mathematics;
using System;

namespace F7s.Utility.Geometry {

    public static class GeometryD {

        public const double Deg2Rad = 0.01745329f;
        public const double Rad2Deg = 57.29578f;

        public static double AngularSizeOfPixel (double screenSize, double fieldOfView) {
            return screenSize / fieldOfView;
        }

        public static double AngularSizeInPixels (double realDiameter, double distance, double screenSize, double fieldOfView) {
            double realAngularSize = realDiameter / distance * GeometryD.Rad2Deg;
            return realAngularSize * AngularSizeOfPixel(screenSize, fieldOfView);
        }

        public static double Logistic (double midpoint, double supremum, double steepness, double x) {
            return supremum / (1 + Math.Pow(Math.E, -steepness * (x - midpoint)));
        }

        public static double GreatCircleDistance (double lat1, double lon1, double lat2, double lon2, double radius = 1.0) {
            lat1 = GeometryD.Deg2Rad * lat1;
            lon1 = GeometryD.Deg2Rad * lon1;
            lat2 = GeometryD.Deg2Rad * lat2;
            lon2 = GeometryD.Deg2Rad * lon2;
            double d_lat = lat2 - lat1;
            double d_lon = lon2 - lon1;
            double h = Math.Sin(d_lat / 2) * Math.Sin(d_lat / 2) +
                Math.Cos(lat1) * Math.Cos(lat2) *
                Math.Sin(d_lon / 2) * Math.Sin(d_lon / 2);
            return 2 * radius * Math.Asin(Math.Sqrt(h));
        }
        public static double FrustrumVolume (double minimumAltitude, double maximumAltitude, double crossSectionAreAtMinimumAltitude, double crossSectionAreAtMaximumAltitude) {
            double frustrumHeight = maximumAltitude - minimumAltitude;
            double lowerSurfaceArea = crossSectionAreAtMinimumAltitude;
            double upperSurfaceArea = crossSectionAreAtMaximumAltitude;
            double volume = frustrumHeight / 3.0f * (lowerSurfaceArea + upperSurfaceArea + Math.Sqrt(lowerSurfaceArea * upperSurfaceArea));
            return volume;
        }

        public static double TriangleSurfaceArea (Vector3d v1, Vector3d v2, Vector3d v3) {

            double side1 = Vector3d.Distance(v1, v2);
            double side2 = Vector3d.Distance(v2, v3);
            double side3 = Vector3d.Distance(v3, v1);
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
            double result = Mathematik.RadToDeg(AngularDiameterInRadiansFromDistanceAndDiameter(distance, diameter));
            return result;
        }

        public static bool PointIsOnLine (Vector3d point, Vector3d lineStart, Vector3d lineEnd) {
            double pointDistance = Vector3d.Distance(lineStart, point) + Vector3d.Distance(lineEnd, point);
            double lineLength = Vector3d.Distance(lineStart, lineEnd);
            if (Mathematik.IsEqualApprox(lineLength, pointDistance)) {
                return true; // C is on the line.
            }
            return false;    // C is not on the line.
        }

        public static double CylinderSurfaceArea (double radius, double height) {
            double flatSurface = CircleArea(radius);
            double sideSurface = CylinderSideSurfaceArea(radius, height);
            return sideSurface + flatSurface * 2.0f;
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

        public static double CubeVolumeFromDimensions (Vector3d dimensions) {
            return dimensions.X * dimensions.Y * dimensions.Z;
        }

        public static double CylinderVolumeFromDimensions (Vector3d dimensions) {
            return dimensions.Y * Math.PI * Math.Pow(0.5f * dimensions.X, 2);
        }

        public static Vector3d DegreesToFractions (Vector3d degrees) {
            return degrees / 360f;
        }

        public static Vector3d FractionsToDegrees (Vector3d fractions) {
            return fractions * 360f;
        }

        public static Quaternion DegreesToQuaternion (Vector3d degrees) {
            if (degrees == Vector3d.Zero) {
                return Quaternion.Identity;
            } else {
                if (degrees.ContainsNaN()) {
                    throw new Exception("Degrees euler vector contains NaN: " + degrees);
                }
                return GeometryF.DegreesToQuaternion(degrees.ToVector3());
            }
        }

        public static Quaternion FractionsToQuaternion (Vector3d fractions) {
            return DegreesToQuaternion(degrees: FractionsToDegrees(fractions: fractions));
        }

        public static Vector3d QuaternionToDegrees (Quaternion quaternion) {
            return GeometryF.QuaternionToDegrees(quaternion);
        }

        public static Vector3d QuaternionToFractions (Quaternion quaternion) {
            return GeometryD.DegreesToFractions(degrees: QuaternionToDegrees(quaternion));
        }

        public static bool PointIsInTriangle (Vector2 point, Vector2 corner1, Vector2 corner2, Vector2 corner3) {
            // Calculated using barycentric coordinates
            double s = corner1.Y * corner3.X
                     - corner1.X * corner3.Y
                     + (corner3.Y - corner1.Y) * point.X
                     + (corner1.X - corner3.X) * point.Y;

            double t = corner1.X * corner2.Y
                     - corner1.Y * corner2.X
                     + (corner1.Y - corner2.Y) * point.X
                     + (corner2.X - corner1.X) * point.Y;

            if (s < 0 != t < 0) {
                return false;
            }

            double a = -corner2.Y * corner3.X
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

        public static Vector3d ScaleBoxToVolume (Vector3d boxExtents, double volume) {
            double originalVolume = Box.Volume(fullExtents: boxExtents.ToVector3());
            double scaleFactor = volume / originalVolume;
            double factorRoot = Math.Pow(scaleFactor, 1.0 / 3.0);
            Vector3d scaledExtents = boxExtents * factorRoot;

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

        public static bool FacesTowards (Vector3d position, Vector3d facing, Vector3d observer) {
            return Vector3d.Angle(facing, observer - position) < 90;
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
        public static bool OneCardinalDirectionOnly (Vector3d vector) {
            return (vector.X != 0 ? 1 : 0) + (vector.Y != 0 ? 1 : 0) + (vector.Z != 0 ? 1 : 0) == 1;
        }

        public static double DistanceOfPointToRay (Vector3d point, Vector3d rayOrigin, Vector3d rayDirection) {
            return Vector3d.Cross(rayDirection, point - rayOrigin).Length();
        }

        public static Vector3d InvertRotation (Vector3d rotationInDegrees) {
            return QuaternionToDegrees(Inverse(DegreesToQuaternion(rotationInDegrees.ToVector3())));
        }

        private static Quaternion Inverse (Quaternion quaternion) {
            throw new NotImplementedException();
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
    }

}