using F7s.Modell.Physical;

namespace F7s.Utility.Measurements.Units {

    public class Hour : Unit {

        public Hour()
            : base(
                   quality: Quality.Time,
                   abbreviatedName: "h",
                   multiplesOfBaseUnit: Constants.MinutesInAnHour * Constants.SecondsInAMinute
                  ) { }

        public override Unit ConditionalReplacement(
            double absoluteQuantityInBaseUnits,
            Commonness requestedCommonness
        ) {

            if (absoluteQuantityInBaseUnits < Minute.multiplesOfBaseUnit * 100) {
                return Minute;
            }

            if (absoluteQuantityInBaseUnits > this.multiplesOfBaseUnit * 100) {
                return Day;
            }

            return this;
        }

    }

}