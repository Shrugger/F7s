namespace F7s.Utility.Measurements.Units {

    public class Liter : Unit {

        public Liter()
            : base(
                   quality: Quality.Volume,
                   abbreviatedName: "l",
                   multiplesOfBaseUnit: 0.001,
                   commonness: Commonness.Common
                  ) { }

        public override Unit ConditionalReplacement(
            double absoluteQuantityInBaseUnits,
            Commonness requestedCommonness
        ) {
            if (absoluteQuantityInBaseUnits > 1000) {
                return MeterCubed;
            }

            return this;
        }

    }

}