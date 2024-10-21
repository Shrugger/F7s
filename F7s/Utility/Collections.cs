using System; using F7s.Utility.Geometry.Double;
using System.Collections.Generic;
using System.Linq;

namespace F7s.Utility {
    public static class Collections {
        public static T MinItem<T>(IEnumerable<T> list, Func<T, float> comparer) {
            if (list == null) {
                throw new Exception();
            } else if (list.Count() == 0) {
                return default(T);
            }
            return list.OrderBy(comparer).First();
        }
        public static T MaxItem<T>(IEnumerable<T> list, Func<T, float> comparer) {
            if (list == null) {
                throw new Exception();
            } else if (list.Count() == 0) {
                return default(T);
            }
            return list.OrderByDescending(comparer).First();
        }

        public static void SafeForEach<T>(List<T> list, Action<T> action) {
            for (int i = list.Count - 1; i >= 0; i--) {
                action.Invoke(list[i]);
            }
        }

        public static bool ContainsDuplicates<T>(List<T> data, bool throwException = false) {
            return ContainsDuplicates(data, (a, b) => a.Equals(b));
        }
        public static bool ContainsDuplicates<T>(List<T> data, Func<T, T, bool> comparer, bool throwException = false) {
            for (int a = 0; a < data.Count; a++) {
                for (int b = 0; b < data.Count; b++) {
                    if (a != b && comparer.Invoke(data[a], data[b])) {
                        if (throwException) {
                            throw new Exception(data[a] + " == " + data[b]);
                        }
                        return true;
                    }
                }
            }
            return false;
        }
    }
}