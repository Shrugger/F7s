using System;
using System.Diagnostics;

namespace F7s.Utility.Mathematics {
    public static class Mathematik {

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
            float polation = range / (value * speed + 1);
            float result = limit - polation;

            if (float.IsNaN(result)) {
                throw new Exception();
            }
            if ((value > 0 && result < floor)) {
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

            return 1.0d - (value - to) / (from - to);
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

            return 1.0d - (value - to) / (from - to);
        }
        public static double LerpClamped (double from, double to, double t) {
            return from + (to - from) * Clamp01(value: t);
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

        public static float DegToRad (float v) {
            throw new NotImplementedException();
        }

        public static int RadToDeg (object x) {
            throw new NotImplementedException();
        }

        public static bool IsEqualApprox (double a, double b, double delta = 0.00000001) {
            throw new NotImplementedException();
        }

        public static int RoundToInt (double v) {
            throw new NotImplementedException();
        }
        public static int RoundToInt (float v) {
            throw new NotImplementedException();
        }

        public static int Clamp (int value, int min, int max) {
            throw new NotImplementedException();
        }
        public static double Clamp (double value, double min, double max) {
            throw new NotImplementedException();
        }
        public static float Clamp (float value, float min, float max) {
            throw new NotImplementedException();
        }

        public static float Lerp (float minAltitudeOverBounds, float maxAltitudeOverBounds, float v) {
            throw new NotImplementedException();
        }

        public static double DegToRad (double v) {
            throw new NotImplementedException();
        }

        public static double Lerp (double a, double b, double interpolationFactor) {
            throw new NotImplementedException();
        }
    }
}
