namespace F7s.Utility.Measurements.Units {

    public class Ton : Unit {

        public Ton()
            : base(quality: Quality.Mass, abbreviatedName: "t", multiplesOfBaseUnit: 1000000.0) { }

        public override Unit ConditionalReplacement(
            double absoluteQuantityInBaseUnits,
            Commonness requestedCommonness
        ) {
            /* Likely unnecessary; reactivate if needed.
				if (commonness < Commonness.Standardised) {
					return Unit.Gram;
				}
				*/

            if (absoluteQuantityInBaseUnits < 100000) {
                return Gram;
            }

            return this;
        }

    }

}