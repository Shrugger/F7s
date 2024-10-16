using F7s.Modell.Physical;

namespace F7s.Utility.Measurements.Units {

    public class Minute : Unit {

        public Minute()
            : base(quality: Quality.Time, abbreviatedName: "m", multiplesOfBaseUnit: Constants.SecondsInAMinute) { }

        public override Unit ConditionalReplacement(
            double absoluteQuantityInBaseUnits,
            Commonness requestedCommonness
        ) {

            if (absoluteQuantityInBaseUnits < 1000) {
                return Second;
            }

            if (absoluteQuantityInBaseUnits > 6000) {
                return Hour;
            }

            return this;
        }

    }

}