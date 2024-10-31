using System; using F7s.Utility.Geometry.Double; using Stride.Core.Mathematics;
using System.Collections.Generic;
using System.Linq;

namespace F7s.Utility.Mathematics
{

    public class Polator {

        private List<(double, double)> data;

        /// <param name="keyValuePairs">Organized by key, then value.</param>
        public Polator (params (double, double)[] keyValuePairs) {
            Array.Sort(keyValuePairs, (a, b) => a.Item1.CompareTo(b.Item1));
            this.data = keyValuePairs.ToList();

            if (Collections.ContainsDuplicates(this.data, true)) {
                throw new Exception("Duplicates detected.");
            }
        }

        public double Value (double target) {
            if (this.data.Count == 0) {
                throw new Exception();
            } else if (this.data.Count == 1) {
                return this.data.Single().Item2;
            } else {
                List<(double, double)> matches = this.data.FindAll(x => x.Item1 == target);
                if (matches.Count > 0) {
                    if (matches.Count == 1) {
                        return matches.Single().Item2;
                    } else {
                        throw new Exception();
                    }
                }
                (double, double) lower = this.data[0];
                (double, double) upper = this.data[1];
                for (int i = 0; i < this.data.Count; i++) {
                    if (lower.Item1 < target && target < upper.Item1) {
                        double interpolationFactor = MM.InverseLerpClamped(lower.Item1, upper.Item1, target);
                        double result = MM.LerpClamped(lower.Item2, upper.Item2, interpolationFactor);
                        return result;
                    } else if (target < lower.Item1 || (target > upper.Item2 && i >= this.data.Count - 1)) {
                        double interpolationFactor = MM.InverseLerp(lower.Item1, upper.Item1, target);
                        double result = MM.Lerp(lower.Item2, upper.Item2, interpolationFactor);
                        return result;
                    } else {
                        lower = this.data[i];
                        upper = this.data[i + 1];
                    }
                }

                throw new Exception();
            }
        }
    }
}
