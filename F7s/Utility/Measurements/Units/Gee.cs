using F7s.Modell.Physical;
using System; using F7s.Utility.Geometry.Double;

namespace F7s.Utility.Measurements.Units {

    public class Gee : Unit {

        public Gee()
            : base(
                   quality: Quality.Acceleration,
                   abbreviatedName: "G",
                   multiplesOfBaseUnit: Constants.StandardEarthGravity,
                   commonness: Commonness.Special
                  ) { }

        public override Unit ConditionalReplacement(
            double absoluteQuantityInBaseUnits,
            Commonness requestedCommonness
        ) {
            if (requestedCommonness == Commonness.Basic) {
                throw new Exception(
                                                       message:
                                                       "This shouldn't happen because Quality.SelectBestUnit ought to avoid picking this for basic commonness."
                                                      );
            }

            if (absoluteQuantityInBaseUnits < 1) {
                return MetersPerSecondSquared;
            }

            if (absoluteQuantityInBaseUnits > this.multiplesOfBaseUnit * 10) {
                return MetersPerSecondSquared;
            }

            return base.ConditionalReplacement(
                                               absoluteQuantityInBaseUnits: absoluteQuantityInBaseUnits,
                                               requestedCommonness: requestedCommonness
                                              );
        }

    }

}