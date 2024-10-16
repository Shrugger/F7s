using F7s.Modell.Physical;

namespace F7s.Utility.Measurements.Units {

    public class Day : Unit {

        public Day ()
            : base(
                   quality: Quality.Time,
                   abbreviatedName: "d",
                   multiplesOfBaseUnit: Constants.HoursInADay * Constants.MinutesInAnHour * Constants.SecondsInAMinute
                  ) { }

        public override Unit ConditionalReplacement (
            double absoluteQuantityInBaseUnits,
            Commonness requestedCommonness
        ) {

            if (absoluteQuantityInBaseUnits < this.multiplesOfBaseUnit) {
                return Hour;
            }

            if (absoluteQuantityInBaseUnits > this.multiplesOfBaseUnit * 1000) {
                return Year;
            }

            return this;
        }

    }

}