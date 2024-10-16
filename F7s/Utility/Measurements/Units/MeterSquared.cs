using Assets.Utility.Measurements;

namespace F7s.Utility.Measurements.Units {

    public class MeterSquared : Unit {

        public MeterSquared()
            : base(quality: Quality.Area, abbreviatedName: "m" + Chars.squared, commonness: Commonness.Basic) { }

        public override string AttachPrefix(Magnitude magnitude, Commonness requestedCommonness) {
            if (requestedCommonness == Commonness.Common) {
                if (magnitude == Magnitude.deca) {
                    return "ar";
                }

                if (magnitude == Magnitude.hecto) {
                    return "ha";
                }
            }

            return base.AttachPrefix(magnitude: magnitude, requestedCommonness: requestedCommonness);

        }

    }

}