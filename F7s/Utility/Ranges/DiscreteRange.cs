namespace F7s.Utility.Ranges {

    public struct DiscreteRange {

        ///<summary> The inclusive minimum </summary>
        public readonly long Min;

        ///<summary> The inclusive maximum </summary>
        public readonly long Max;

        public readonly Distribution Distribution;

        public DiscreteRange(long min, long max)
            : this(min: min, max: max, distribution: new Uniform(min, max)) { }

        public DiscreteRange(long min, long max, Distribution distribution) {
            this.Min = min;
            this.Max = max;
            this.Distribution = distribution;
        }

        public long GetValue() {
            return this.Distribution.GetDiscreteValue();
        }

    }

}