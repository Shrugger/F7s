using F7s.Utility.Geometry;
using Stride.Core.Mathematics;
using System;
using System.Diagnostics;

namespace F7s.Utility {
    public static class Transforms {
        public static bool ApproximatelyEqual (Transform3D a, Transform3D b, float delta = 0.001f) {
            bool origin = Vectors.ApproximatelyEqual(a.Origin, b.Origin, delta);
            bool basis0 = Vectors.ApproximatelyEqual(a.Basis.Column0, b.Basis.Column0, delta);
            bool basis1 = Vectors.ApproximatelyEqual(a.Basis.Column1, b.Basis.Column1, delta);
            bool basis2 = Vectors.ApproximatelyEqual(a.Basis.Column2, b.Basis.Column2, delta);
            return origin && basis0 && basis1 && basis2;
        }

        public static void AssertEqual (Transform3D a, Transform3D b, float delta = 0.001f) {
            Debug.Assert(
                ApproximatelyEqual(a, b, delta),
                "\n" +
                a + "\n" +
                "!= " + "\n" +
                b + "\n" +
                "Differences " + "\n" +
                "Col0" + (a.Basis.Column0 - b.Basis.Column0) + "\n" +
                "Col1" + (a.Basis.Column1 - b.Basis.Column1) + "\n" +
                "Col2" + (a.Basis.Column2 - b.Basis.Column2) + "\n" +
                "Orig" + (a.Origin - b.Origin) + "\n" +
                "Delta " + delta,
                delta);
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
            if (Vector3.Zero == transform.Basis.Column0) {
                return false;
            }
            if (Vector3.Zero == transform.Basis.Column1) {
                return false;
            }
            if (Vector3.Zero == transform.Basis.Column2) {
                return false;
            }
            if (Vectors.Invalid(transform.Basis.Column0)) {
                return false;
            }
            if (Vectors.Invalid(transform.Basis.Column1)) {
                return false;
            }
            if (Vectors.Invalid(transform.Basis.Column2)) {
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
    }

}