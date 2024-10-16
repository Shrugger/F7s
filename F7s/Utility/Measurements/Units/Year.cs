using F7s.Modell.Physical;

namespace F7s.Utility.Measurements.Units {

    public class Year : Unit {

        public Year()
            : base(
                   quality: Quality.Time,
                   abbreviatedName: "y",
                   multiplesOfBaseUnit: Constants.DaysInAStandardYear
                                      * Constants.HoursInADay
                                      * Constants.MinutesInAnHour
                                      * Constants.SecondsInAMinute
                  ) { }

        public override Unit ConditionalReplacement(
            double absoluteQuantityInBaseUnits,
            Commonness requestedCommonness
        ) {

            if (absoluteQuantityInBaseUnits < Day.multiplesOfBaseUnit * 1000) {
                return Day;
            }

            return this;
        }

    }

}