using Assets.Utility.Measurements;
using F7s.Utility.Measurements.Units;
using System;
using System.Collections.Generic;

namespace F7s.Utility.Measurements {

    public class Quality {

        /// <summary>
        ///     This is effectively a constant; change it to change commonness behaviour overall.
        /// </summary>
        private static readonly bool onlyUseExactlyTheRequestedCommonness = false;

        public readonly Dimensions Dimensionality;

        public readonly string Name;
        private readonly List<Unit> Units = new List<Unit>();

        internal Quality(string name, Dimensions dimensionality = Dimensions.Linear) {
            this.Name = name;
            this.Dimensionality = dimensionality;
        }

        internal double GetOrderOfMagnitude(double quantityInBaseUnits) {
            switch (this.Dimensionality) {
                case Dimensions.Linear: return Math.Log10(Math.Abs(quantityInBaseUnits));
                case Dimensions.Quadratic: return Math.Log(Math.Abs(quantityInBaseUnits), 100);
                case Dimensions.Cubic: return Math.Log(Math.Abs(quantityInBaseUnits), 1000);
                default:

                    throw new Exception(message: this.Dimensionality.ToString());
            }
        }

        internal double SingleMagnitudeScale() {
            switch (this.Dimensionality) {
                case Dimensions.Linear: return 10;
                case Dimensions.Quadratic: return 100;
                case Dimensions.Cubic: return 1000;
                default:

                    throw new Exception(message: this.Dimensionality.ToString());
            }
        }

        internal void AddUnit(Unit unit) {
            this.Units.Add(item: unit);
        }

        internal Unit GetBestUnitForQuantity(double valueInBaseUnits, Commonness commonness) {

            double valueMagnitude = this.GetOrderOfMagnitude(quantityInBaseUnits: valueInBaseUnits);

            Unit bestUnit = null;
            double smallestUnitMagnitudeDifference = 0;
            double smallestUnitOffsetDifference = 0;
            double smallestUnitCommonnessDifference = 0;

            foreach (Unit unit in this.Units) {

                if (onlyUseExactlyTheRequestedCommonness == false
                 || unit.AvailableInCommonness(commonness: commonness)) {
                    double unitMagnitude = this.GetOrderOfMagnitude(quantityInBaseUnits: unit.multiplesOfBaseUnit);

                    double unitMagnitudeDifference = Math.Abs(valueMagnitude - unitMagnitude);
                    double unitOffsetDifference = Math.Abs(valueInBaseUnits - unit.offsetToBaseUnit);
                    double unitCommonnessDifference = Math.Abs(commonness - unit.commonness);

                    bool isNewChampion = false;

                    if (bestUnit == null) {
                        isNewChampion = true;
                    } else if (unitCommonnessDifference < smallestUnitCommonnessDifference) {
                        isNewChampion = true;
                    } else if (unitCommonnessDifference == smallestUnitCommonnessDifference) {
                        if (unitMagnitudeDifference < smallestUnitMagnitudeDifference) {
                            isNewChampion = true;
                        } else if (unitMagnitudeDifference == smallestUnitMagnitudeDifference) {
                            if (unitOffsetDifference < smallestUnitOffsetDifference) {
                                isNewChampion = true;
                            }
                        }
                    }

                    if (isNewChampion) {
                        bestUnit = unit;
                        smallestUnitCommonnessDifference = unitCommonnessDifference;
                        smallestUnitMagnitudeDifference = unitMagnitudeDifference;
                        smallestUnitOffsetDifference = unitOffsetDifference;
                    }
                }
            }

            if (bestUnit == null) {
                throw new NoUnitAvailableForCommonnessException(
                                                                message: "No unit found in quality "
                                                                       + this
                                                                       + " for commonness "
                                                                       + commonness
                                                                       + "."
                                                               );
            }

            bestUnit = bestUnit.ConditionalReplacement(
                                                       absoluteQuantityInBaseUnits: Math.Abs(valueInBaseUnits),
                                                       requestedCommonness: commonness
                                                      );

            return bestUnit;
        }

        public override string ToString() {
            return this.Name;
        }

        #region quality presets

        public static Quality Mass { get; private set; }

        public static Quality Time { get; private set; }

        public static Quality Speed { get; private set; }

        public static Quality Acceleration { get; private set; }

        public static Quality Length { get; private set; }

        public static Quality Area { get; private set; }

        public static Quality Volume { get; private set; }

        public static Quality Temperature { get; private set; }

        public static Quality Current { get; private set; }

        public static Quality Substance { get; private set; }

        public static Quality Luminosity { get; private set; }

        public static Quality Pressure { get; private set; }

        public static void InitializeQualities() {

            Mass = new Quality(name: "Mass");
            Time = new Quality(name: "Time");
            Speed = new Quality(name: "Speed");

            Acceleration = new Quality(name: "Acceleration", dimensionality: Dimensions.Linear);

            Length = new Quality(name: "Length");

            Area = new Quality(name: "Area", dimensionality: Dimensions.Quadratic);

            Volume = new Quality(name: "Volume", dimensionality: Dimensions.Cubic);

            Temperature = new Quality(name: "Temperature");
            Current = new Quality(name: "Current");
            Substance = new Quality(name: "Substance");
            Luminosity = new Quality(name: "Luminosity");
            Pressure = new Quality(name: "Pressure");
        }

        #endregion

    }

}