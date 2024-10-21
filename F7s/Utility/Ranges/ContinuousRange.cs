using System; using F7s.Utility.Geometry.Double; using Stride.Core.Mathematics;

namespace F7s.Utility.Ranges
{

    public struct ContinuousRange {

        ///<summary> The exclusive minimum </summary>
        public readonly float Min;

        ///<summary> The exclusive maximum </summary>
        public readonly float Max;

        public readonly Distribution Distribution;

        public ContinuousRange (float min, float max)
            : this(min: min, max: max, distribution: null) { }

        public ContinuousRange (float min, float max, Distribution distribution) {
            this.Min = min;
            this.Max = max;
            this.Distribution = distribution ?? new Uniform(min, max);
        }

        public static ContinuousRange UpTo (float max) {
            return new ContinuousRange(min: float.MinValue, max: max);
        }

        public static ContinuousRange AtLeast (float min) {
            return new ContinuousRange(min: min, max: float.MaxValue);
        }

        internal bool Contains (float distance) {
            return distance >= this.Min && distance <= this.Max;
        }

        public static ContinuousRange Any () {
            return new ContinuousRange(min: float.MinValue, max: float.MaxValue);
        }

        public float GetValue () {
            if (this.Distribution == null) {
                throw new NullReferenceException(message: "No distribution defined for this range.");
            }

            return this.Distribution.GetContinuousValue();
        }

        public float Span () {
            return this.Max - this.Min;
        }

        public float Clamp (float value) {
            return Mathematik.Clamp(value: value, min: this.Min, max: this.Max);
        }

        public override string ToString () {
            return "[" + this.Min + ", " + this.Max + "]";
        }

    }

}