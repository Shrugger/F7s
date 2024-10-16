using F7s.Utility.Geometry;
using Stride.Core.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace F7s.Utility {

    public static class Vectors {
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
        public static bool Valid (Vector3d v) {

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

        public static bool Invalid (Vector3 v) {
            return !Valid(v);
        }
        public static bool Invalid (Vector3d v) {
            return !Valid(v);
        }


        public static bool ApproximatelyEqual (Vector3 expected, Vector3 actual, float tolerance) {
            return Math.Abs(expected.X) - Math.Abs(actual.X) < tolerance &&
                    Math.Abs(expected.Y) - Math.Abs(actual.Y) < tolerance &&
                    Math.Abs(expected.Z) - Math.Abs(actual.Z) < tolerance;
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
            return scale.X > 0 && scale.Y > 0 && scale.Z > 0 || scale.X < 0 && scale.Y < 0 && scale.Z < 0;
        }
        public static bool AllPositivellOrNegativeOne (Vector3 scale, float tolerance) {
            return ApproximatelyEqual(Vector3.One, scale, tolerance) || ApproximatelyEqual(-Vector3.One, scale, tolerance);
        }
    }

}