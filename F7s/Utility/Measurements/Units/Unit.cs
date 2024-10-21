using Assets.Utility.Measurements;
using F7s.Modell.Physical;
using F7s.Utility.Lazies;
using System; using F7s.Utility.Geometry.Double; using Stride.Core.Mathematics;

namespace F7s.Utility.Measurements.Units {

    public class Unit {

        public readonly string abbreviatedName;

        public readonly Commonness commonness;
        public readonly double multiplesOfBaseUnit;
        public readonly double offsetToBaseUnit;

        public readonly Quality quality;
        public readonly double unitMagnitude;

        public Unit(
            Quality quality,
            string abbreviatedName,
            double multiplesOfBaseUnit = 1,
            double offsetToBaseUnit = 0.0,
            Commonness commonness = Commonness.Standardised
        ) {

            if (multiplesOfBaseUnit != 1 && offsetToBaseUnit != 0) {
                throw new NotImplementedException(
                                                  message:
                                                  "Handling both a multiplier and an offset at once is currently undefined."
                                                 );
            }

            this.quality = quality;
            this.abbreviatedName = abbreviatedName;
            this.multiplesOfBaseUnit = multiplesOfBaseUnit;
            this.offsetToBaseUnit = offsetToBaseUnit;
            this.unitMagnitude = quality.GetOrderOfMagnitude(quantityInBaseUnits: multiplesOfBaseUnit);
            this.commonness = commonness;

            if (quality != null) {
                this.quality.AddUnit(unit: this);
            }
        }

        public Dimensions Dimensionality => this.quality.Dimensionality;

        public double ConvertFromBaseUnitToThisUnit(double baseValue) {
            if (this.multiplesOfBaseUnit != 1) {
                return baseValue / this.multiplesOfBaseUnit;
            }

            if (this.offsetToBaseUnit != 0) {
                return baseValue - this.offsetToBaseUnit;
            }

            return baseValue;
        }

        public bool AvailableInCommonness(Commonness commonness) {
            return this.commonness <= commonness;
        }

        protected bool Equals(Unit other) {
            return object.Equals(objA: this.quality, objB: other.quality)
                && string.Equals(a: this.abbreviatedName, b: other.abbreviatedName)
                && this.multiplesOfBaseUnit.Equals(obj: other.multiplesOfBaseUnit)
                && this.offsetToBaseUnit.Equals(obj: other.offsetToBaseUnit);
        }

        public override bool Equals(object obj) {
            if (obj is null) {
                return false;
            }

            if (ReferenceEquals(objA: this, objB: obj)) {
                return true;
            }

            if (obj.GetType() != this.GetType()) {
                return false;
            }

            return this.Equals(other: (Unit)obj);
        }

        public override int GetHashCode() {
            unchecked {
                int hashCode = this.quality != null ? this.quality.GetHashCode() : 0;
                hashCode = (hashCode * 397) ^ (this.abbreviatedName != null ? this.abbreviatedName.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ this.multiplesOfBaseUnit.GetHashCode();
                hashCode = (hashCode * 397) ^ this.offsetToBaseUnit.GetHashCode();

                return hashCode;
            }
        }

        public static bool operator ==(Unit left, Unit right) {
            return object.Equals(objA: left, objB: right);
        }

        public static bool operator !=(Unit left, Unit right) {
            return !object.Equals(objA: left, objB: right);
        }

        public override string ToString() {
            return this.abbreviatedName;
        }

        public virtual Unit ConditionalReplacement(
            double absoluteQuantityInBaseUnits,
            Commonness requestedCommonness
        ) {
            return this;
        }

        public virtual string AttachPrefix(Magnitude magnitude, Commonness requestedCommonness) {
            string unitAndPrefix = "";

            if (magnitude == Magnitude.none) {
                unitAndPrefix = this.abbreviatedName;
            } else if (magnitude == Magnitude.peu) {
                return this.quality.ToString().ToLower();
            } else if (magnitude == Magnitude.lots) {
                return this.quality.ToString().ToUpper();
            } else {
                unitAndPrefix = Unit.AbbreviatedPrefix(magnitude: magnitude) + this.abbreviatedName;
            }

            return unitAndPrefix;
        }

        private static string Prefix(Magnitude magnitude) {
            return magnitude.ToString();
        }

        private static LazyMapper<Magnitude, string> MetricPrefixes = new LazyMapper<Magnitude, string>(m => m.ToString().Substring(startIndex: 0, length: 1));

        protected static string AbbreviatedPrefix(Magnitude magnitude) {
            switch (magnitude) {
                case Magnitude.peu: throw new ExceedinglySmallMeasure();
                case Magnitude.lots: throw new ExceedinglyLargeMeasure();
                case Magnitude.none: return "";
                case Magnitude.deca: return "da";
                default:
                    string prefix = MetricPrefixes.Get(magnitude);

                    if ((int)magnitude > 3) {
                        prefix = prefix.ToUpper();
                    }

                    return prefix;
            }
        }

        #region Unit presets

        public static Unit Gram { get; private set; }

        public static Unit Ton { get; private set; }

        public static Unit Second { get; private set; }

        public static Minute Minute { get; private set; }

        public static Hour Hour { get; private set; }

        public static Day Day { get; private set; }

        public static Year Year { get; private set; }

        public static Unit MetersPerSecond { get; private set; }

        public static PercentOfSpeedOfLight PercentOfSpeedOfLight { get; private set; }

        public static Unit SpeedOfLight { get; private set; }

        public static Unit MetersPerSecondSquared { get; private set; }

        public static Gee Gee { get; private set; }

        public static Unit Meter { get; private set; }

        public static Unit AstronomicalUnit { get; private set; }

        public static MeterSquared MeterSquared { get; private set; }

        public static Unit MeterCubed { get; private set; }

        public static Liter Liter { get; private set; }

        public static Unit Kelvin { get; private set; }

        public static Unit Centigrade { get; private set; }

        public static Unit Ampere { get; private set; }

        public static Unit Candela { get; private set; }

        public static Unit Mole { get; private set; }

        public static Unit Pascal { get; private set; }

        public static void InitializeUnits() {

            Unit.Gram = new Unit(quality: Quality.Mass, abbreviatedName: "g", commonness: Commonness.Basic);

            Unit.Ton = new Ton();

            Unit.Second = new Unit(quality: Quality.Time, abbreviatedName: "s");

            Unit.Minute = new Minute();

            Unit.Hour = new Hour();

            Unit.Day = new Day();

            Unit.Year = new Year();

            Unit.MetersPerSecond = new Unit(
                                            quality: Quality.Speed,
                                            abbreviatedName: "m/s",
                                            commonness: Commonness.Basic
                                           );

            Unit.PercentOfSpeedOfLight = new PercentOfSpeedOfLight();

            Unit.SpeedOfLight = new Unit(
                                         quality: Quality.Speed,
                                         abbreviatedName: "c",
                                         multiplesOfBaseUnit: Constants.SpeedOfLight,
                                         commonness: Commonness.Scientific
                                        );

            Unit.MetersPerSecondSquared = new Unit(
                                                   quality: Quality.Acceleration,
                                                   abbreviatedName: "m/s" + Chars.squared,
                                                   commonness: Commonness.Basic
                                                  );

            Unit.Gee = new Gee();

            Unit.Meter = new Unit(quality: Quality.Length, abbreviatedName: "m", commonness: Commonness.Basic);

            Unit.AstronomicalUnit = new Unit(
                                             quality: Quality.Length,
                                             abbreviatedName: "AU",
                                             multiplesOfBaseUnit: Constants.AstronomicUnit,
                                             commonness: Commonness.Scientific
                                            );

            Unit.MeterSquared = new MeterSquared();

            Unit.MeterCubed = new Unit(
                                       quality: Quality.Volume,
                                       abbreviatedName: "m" + Chars.cubic,
                                       commonness: Commonness.Basic
                                      );

            Unit.Liter = new Liter();

            Unit.Kelvin = new Unit(quality: Quality.Temperature, abbreviatedName: "K", commonness: Commonness.Basic);

            Unit.Centigrade = new Centigrade();

            Unit.Ampere = new Unit(quality: Quality.Current, abbreviatedName: "A", commonness: Commonness.Basic);

            Unit.Candela = new Unit(quality: Quality.Luminosity, abbreviatedName: "cd", commonness: Commonness.Basic);

            Unit.Mole = new Unit(quality: Quality.Substance, abbreviatedName: "mol", commonness: Commonness.Basic);

            Unit.Pascal = new Unit(
                                   quality: Quality.Pressure,
                                   abbreviatedName: "Pa",
                                   commonness: Commonness.Standardised
                                  );

        }

        #endregion

    }

}