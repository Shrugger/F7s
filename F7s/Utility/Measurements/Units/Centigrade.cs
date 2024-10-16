using F7s.Modell.Physical;

namespace F7s.Utility.Measurements.Units {

    public class Centigrade : Unit {

        public Centigrade ()
            : base(
                   quality: Quality.Temperature,
                   abbreviatedName: Chars.degree + "C",
                   offsetToBaseUnit: Constants.CelsiusOffsetToKelvin,
                   commonness: Commonness.Common
                  ) { }

        public override Unit ConditionalReplacement (
            double absoluteQuantityInBaseUnits,
            Commonness requestedCommonness
        ) {
            /* Likely unnecessary; reactivate if needed.
				if (commonness < Commonness.Standardised) {
					return Unit.Kelvin;
				}
				*/

            if (absoluteQuantityInBaseUnits >= this.offsetToBaseUnit + 1000) {
                return Kelvin;
            }

            return this;
        }

    }

}