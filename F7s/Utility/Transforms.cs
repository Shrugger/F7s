using F7s.Utility.Geometry;
using System;
using System.Diagnostics;

namespace F7s.Utility {
    public static class Transforms {
        public static bool ApproximatelyEqual (Transform3D a, Transform3D b, float delta = 0.001f) {
            bool origin = Vectors.ApproximatelyEqual(a.Origin, b.Origin, delta);
            bool basis = Matrix3x3d.ApproximatelyEqual(a.Basis, b.Basis, delta);
            return origin && basis;
        }

        public static void AssertEqual (Transform3D a, Transform3D b, float delta = 0.001f) {
            Debug.Assert(ApproximatelyEqual(a, b, delta));
        }

        const float DefaultValidationTolerance = 0.0001f;

        public static void ValidatePositional (Transform3D transform, float tolerance = DefaultValidationTolerance) {
            if (InvalidPositional(transform, tolerance)) {
                throw new Exception(transform.ToString());
            }
        }

        public static bool InvalidPositional (Transform3D transform, float tolerance = DefaultValidationTolerance) {
            return !ValidPositional(transform, tolerance);
        }
        public static bool InvalidScaled (Transform3D transform) {
            return !ValidScaled(transform);
        }

        public static bool ValidPositional (Transform3D transform, float tolerance = DefaultValidationTolerance) {
            return Valid(transform, true, tolerance);
        }

        public static bool ValidScaled (Transform3D transform) {
            return Valid(transform, false, 0);
        }

        private static bool Valid (Transform3D transform, bool expectUniformScale, float tolerance = DefaultValidationTolerance) {
            if (Vectors.Invalid(transform.Origin)) {
                return false;
            }
            if (transform.Basis.m00 == 0 && transform.Basis.m10 == 0 && transform.Basis.m20 == 0) {
                return false;
            }
            if (transform.Basis.m00 == 0 && transform.Basis.m10 == 0 && transform.Basis.m20 == 0) {
                return false;
            }
            if (transform.Basis.m00 == 0 && transform.Basis.m10 == 0 && transform.Basis.m20 == 0) {
                return false;
            }
            if (!ValidCellValue(transform.Basis.m00) || !ValidCellValue(transform.Basis.m01) || !ValidCellValue(transform.Basis.m02)) {
                return false;
            }
            if (!ValidCellValue(transform.Basis.m10) || !ValidCellValue(transform.Basis.m11) || !ValidCellValue(transform.Basis.m12)) {
                return false;
            }
            if (!ValidCellValue(transform.Basis.m20) || !ValidCellValue(transform.Basis.m21) || !ValidCellValue(transform.Basis.m22)) {
                return false;
            }
            if (Vectors.Invalid(transform.Basis.Scale)) {
                return false;
            }
            if (expectUniformScale && !Vectors.AllPositivellOrNegativeOne(transform.Basis.Scale, tolerance)) {
                return false;
            }

            return true;
        }

        public static bool ValidCellValue (double value) {
            return double.IsFinite(value) && !double.IsNaN(value);
        }
    }

}