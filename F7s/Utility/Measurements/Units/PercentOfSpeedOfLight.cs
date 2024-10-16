using F7s.Modell.Physical;

namespace F7s.Utility.Measurements.Units {

    public class PercentOfSpeedOfLight : Unit {

        public PercentOfSpeedOfLight()
            : base(
                   quality: Quality.Speed,
                   abbreviatedName: "%c",
                   multiplesOfBaseUnit: Constants.SpeedOfLight / 100.0,
                   commonness: Commonness.Scientific
                  ) { }

        public override Unit ConditionalReplacement(
            double absoluteQuantityInBaseUnits,
            Commonness requestedCommonness
        ) {
            if (absoluteQuantityInBaseUnits >= SpeedOfLight.multiplesOfBaseUnit) {
                return SpeedOfLight;
            }

            if (absoluteQuantityInBaseUnits < this.multiplesOfBaseUnit) {
                return MetersPerSecond;
            }

            return this;
        }

    }

}