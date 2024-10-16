using System.Linq;

namespace F7s.Utility.Measurements {

    public struct Appellation {

        public readonly string unitAndPrefix;
        public readonly double numericalValue;
        public readonly string completeAppellation;
        public readonly double digitToLengthRatio;
        public readonly int overallLength;
        public readonly bool isZero;

        public Appellation(double numericalValue, string unitAndPrefix, string completeAppellation) {
            this.unitAndPrefix = unitAndPrefix;
            this.numericalValue = numericalValue;
            this.completeAppellation = completeAppellation;

            this.overallLength = completeAppellation.Length;
            int digits = completeAppellation.Count(predicate: char.IsDigit);

            digits -= Strings.CountUninformativeZeroes(value: numericalValue);

            this.isZero = numericalValue == 0d;

            this.digitToLengthRatio = digits / (double)this.overallLength;
        }

        public override string ToString() {
            return this.completeAppellation;
        }

    }

}