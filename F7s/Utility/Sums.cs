using System;
using System.Collections.Generic;

namespace F7s.Utility {

    public static class Sums {

        public static char Sum(IEnumerable<char> collection) {
            char sum = (char)0;

            foreach (char item in collection) {
                sum += item;
            }

            return sum;
        }

        public static int Sum(IEnumerable<int> collection) {
            int sum = 0;

            foreach (int item in collection) {
                sum += item;
            }

            return sum;
        }

        public static float Sum(IEnumerable<float> collection) {
            float sum = 0;

            foreach (float item in collection) {
                sum += item;
            }

            return sum;
        }

        public static double Sum(IEnumerable<double> collection) {
            double sum = 0;

            foreach (double item in collection) {
                sum += item;
            }

            return sum;
        }

        public static double Sum<SourceType>(IEnumerable<SourceType> collection, Func<SourceType, double> function) {
            double sum = 0;

            foreach (SourceType item in collection) {
                sum += function.Invoke(item);
            }

            return sum;
        }

        public static float Sum<SourceType>(IEnumerable<SourceType> collection, Func<SourceType, float> function) {
            float sum = 0;

            foreach (SourceType item in collection) {
                sum += function.Invoke(item);
            }

            return sum;
        }

    }

}