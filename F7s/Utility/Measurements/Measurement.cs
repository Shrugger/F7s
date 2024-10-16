using Assets.Utility.Measurements;
using F7s.Utility.Mathematics;
using F7s.Utility.Measurements.Units;
using System;
using System.Collections.Generic;
using System.Linq;

namespace F7s.Utility.Measurements {

    public static class Measurement {

        private const Padding DefaultPadding = Padding.None;
        private const Commonness DefaultCommonness = Commonness.Standardised;
        private const int DefaultFinalStringLength = 6;

        /// <summary>
        ///     1 keeps things compact, but means that a decimal point and a single decimal are used - space-inefficient.
        ///     2 is more space-efficient for cases in which decimals are used, but they are also used more freely.
        ///     Larger values are not suited as defaults.
        /// </summary>
        private const int DefaultMaximumDecimals = 1;

        private const string DefaultSeparator = "";

        #region initialization

        static Measurement () {
            InitializeIfApplicable();
        }

        private static bool systemInitialized;

        internal static void InitializeIfApplicable () {
            if (systemInitialized == false) {
                Quality.InitializeQualities();
                Unit.InitializeUnits();
                systemInitialized = true;
            }
        }

        #endregion

        #region quality-specific factories

        public static string MakeMeasurement (
            double value,
            string unitAbbreviation,
            int finalStringLength = DefaultFinalStringLength,
            int maximumDecimals = DefaultMaximumDecimals,
            bool? permitGranularMagnitudes = null,
            string separator = DefaultSeparator,
            Padding padding = DefaultPadding,
            Magnitude? preferredMagnitude = null
        ) {

            Quality quality = new Quality(name: unitAbbreviation, dimensionality: Dimensions.Linear);

            Unit unit = new Unit(
                                       quality: quality,
                                       abbreviatedName: unitAbbreviation,
                                       commonness: Commonness.Special
                                      );

            return MakeMeasurement(
                                               quality: quality,
                                               quantityInBaseUnits: value,
                                               finalStringLength: finalStringLength,
                                               maximumDecimals: maximumDecimals,
                                               commonness: Commonness.Special,
                                               permitGranularMagnitudes: permitGranularMagnitudes,
                                               separator: separator,
                                               padding: padding,
                                               preferredUnit: unit,
                                               preferredMagnitude: preferredMagnitude
                                              );
        }


        public static string MeasureVelocity (
            float velocityInMetersPerSecond,
            int finalStringLength = DefaultFinalStringLength,
            int maximumDecimals = DefaultMaximumDecimals,
            Commonness commonness = DefaultCommonness,
            bool? permitGranularMagnitudes = null,
            string separator = DefaultSeparator,
            Padding padding = DefaultPadding,
            Unit preferredUnit = null,
            Magnitude? preferredMagnitude = null
            ) {
            return MakeMeasurement(
                                               quality: Quality.Speed,
                                               quantityInBaseUnits: velocityInMetersPerSecond,
                                               finalStringLength: finalStringLength,
                                               maximumDecimals: maximumDecimals,
                                               commonness: commonness,
                                               permitGranularMagnitudes: permitGranularMagnitudes,
                                               separator: separator,
                                               padding: padding,
                                               preferredUnit: preferredUnit,
                                               preferredMagnitude: preferredMagnitude
                                              );
        }

        public static string MeasureLength (
            double lengthInMeters,
            int finalStringLength = DefaultFinalStringLength,
            int maximumDecimals = DefaultMaximumDecimals,
            Commonness commonness = DefaultCommonness,
            bool? permitGranularMagnitudes = null,
            string separator = DefaultSeparator,
            Padding padding = DefaultPadding,
            Unit preferredUnit = null,
            Magnitude? preferredMagnitude = null
            ) {
            return MakeMeasurement(
                                               quality: Quality.Length,
                                               quantityInBaseUnits: lengthInMeters,
                                               finalStringLength: finalStringLength,
                                               maximumDecimals: maximumDecimals,
                                               commonness: commonness,
                                               permitGranularMagnitudes: permitGranularMagnitudes,
                                               separator: separator,
                                               padding: padding,
                                               preferredUnit: preferredUnit,
                                               preferredMagnitude: preferredMagnitude
                                              );
        }


        public static string MeasureMass (
            double massInKilograms,
            int finalStringLength = DefaultFinalStringLength,
            int maximumDecimals = DefaultMaximumDecimals,
            Commonness commonness = DefaultCommonness,
            bool? permitGranularMagnitudes = null,
            string separator = DefaultSeparator,
            Padding padding = DefaultPadding,
            Unit preferredUnit = null,
            Magnitude? preferredMagnitude = null
        ) {
            return Measurement.MakeMeasurement(
                                               quality: Quality.Mass,
                                               quantityInBaseUnits:
                                               massInKilograms
                                             * 1000.0 /* Multiply by 1000 because the Mass struct defaults to KG whereas the Mass quality assumes grams.*/,
                                               finalStringLength: finalStringLength,
                                               maximumDecimals: maximumDecimals,
                                               commonness: commonness,
                                               permitGranularMagnitudes: permitGranularMagnitudes,
                                               separator: separator,
                                               padding: padding,
                                               preferredUnit: preferredUnit,
                                               preferredMagnitude: preferredMagnitude
                                              );
        }

        public static string MeasureVolume (
            double volume,
            int finalStringLength = DefaultFinalStringLength,
            int maximumDecimals = DefaultMaximumDecimals,
            Commonness commonness = DefaultCommonness,
            bool? permitGranularMagnitudes = null,
            string separator = DefaultSeparator,
            Padding padding = DefaultPadding,
            Unit preferredUnit = null,
            Magnitude? preferredMagnitude = null
        ) {
            return Measurement.MakeMeasurement(
                                               quality: Quality.Volume,
                                               quantityInBaseUnits: volume,
                                               finalStringLength: finalStringLength,
                                               maximumDecimals: maximumDecimals,
                                               commonness: commonness,
                                               permitGranularMagnitudes: permitGranularMagnitudes,
                                               separator: separator,
                                               padding: padding,
                                               preferredUnit: preferredUnit,
                                               preferredMagnitude: preferredMagnitude
                                              );
        }

        public static string MeasureTime (
            double timeInSeconds,
            int finalStringLength = DefaultFinalStringLength,
            int maximumDecimals = DefaultMaximumDecimals,
            Commonness commonness = DefaultCommonness,
            bool? permitGranularMagnitudes = null,
            string separator = DefaultSeparator,
            Padding padding = DefaultPadding,
            Unit preferredUnit = null,
            Magnitude? preferredMagnitude = null
        ) {
            return MakeMeasurement(
                                               quality: Quality.Time,
                                               quantityInBaseUnits: timeInSeconds,
                                               finalStringLength: finalStringLength,
                                               maximumDecimals: maximumDecimals,
                                               commonness: commonness,
                                               permitGranularMagnitudes: permitGranularMagnitudes,
                                               separator: separator,
                                               padding: padding,
                                               preferredUnit: preferredUnit,
                                               preferredMagnitude: preferredMagnitude
                                              );

        }

        #endregion

        #region general factory

        public static string MakeMeasurement (
            Quality quality,
            double quantityInBaseUnits,
            int finalStringLength = DefaultFinalStringLength,
            int maximumDecimals = DefaultMaximumDecimals,
            Commonness commonness = DefaultCommonness,
            bool? permitGranularMagnitudes = null,
            string separator = DefaultSeparator,
            Padding padding = DefaultPadding,
            Unit preferredUnit = null,
            Magnitude? preferredMagnitude = null
        ) {

            if (quality == null) {
                throw new NullReferenceException(message: "No (" + quality + ") quality provided. Other arguments: " + quantityInBaseUnits + ", " + finalStringLength + ", " + maximumDecimals + ", " + commonness + ", " + permitGranularMagnitudes + ", " + separator + ", " + padding + ", " + preferredUnit + ", " + preferredMagnitude + ".");
            }

            if (quantityInBaseUnits == 0) {
                return PadAppellation(
                                                  appellation: new Appellation(
                                                                               numericalValue: 0,
                                                                               unitAndPrefix: "",
                                                                               completeAppellation: "0"
                                                                              ),
                                                  separator: separator,
                                                  padding: padding,
                                                  totalLength: finalStringLength
                                                 );
            }

            permitGranularMagnitudes = permitGranularMagnitudes ?? quality.Dimensionality >= Dimensions.Quadratic;

            Appellation appellation = PrepareAppellation(
                                                                     quality: quality,
                                                                     quantityInBaseUnits: quantityInBaseUnits,
                                                                     finalStringLength: finalStringLength,
                                                                     maximumDecimals: maximumDecimals,
                                                                     commonness: commonness,
                                                                     permitGranularMagnitudes:
                                                                     (bool) permitGranularMagnitudes,
                                                                     separator: separator,
                                                                     preferredUnit: preferredUnit,
                                                                     preferredMagnitude: preferredMagnitude
                                                                    );

            string appellationString = PadAppellation(
                                                                  appellation: appellation,
                                                                  separator: separator,
                                                                  padding: padding,
                                                                  totalLength: finalStringLength
                                                                 );

            return appellationString;
        }

        private static string PadAppellation (
            Appellation appellation,
            string separator,
            Padding padding,
            int totalLength
        ) {

            string appellationString;

            if (padding == Padding.None) {
                appellationString = appellation.completeAppellation;
            } else if (padding == Padding.Left) {
                appellationString = appellation.completeAppellation.PadLeft(totalWidth: totalLength);
            } else if (padding == Padding.Right) {
                appellationString = appellation.completeAppellation.PadRight(totalWidth: totalLength);
            } else if (padding == Padding.TableCentered) {
                string numericalString = appellation.numericalValue.ToString();

                double paddingLength = totalLength
                                     - (numericalString.Length + appellation.unitAndPrefix.Length + separator.Length);
                int paddingLeft = Mathematik.FloorToInt(paddingLength / 2.0);
                int paddingRight = Mathematik.CeilToInt(paddingLength / 2.0);

                appellationString = numericalString.PadLeft(totalWidth: paddingLeft)
                                  + separator
                                  + appellation.unitAndPrefix.PadRight(totalWidth: paddingRight);
            } else if (padding == Padding.TableBlocky) {
                string numericalString = appellation.numericalValue.ToString();
                int paddingLength = totalLength - (numericalString.Length + appellation.unitAndPrefix.Length);
                appellationString = numericalString.PadRight(totalWidth: paddingLength) + appellation.unitAndPrefix;
            } else {
                throw new NotImplementedException();
            }

            return appellationString;
        }

        private static Appellation PrepareAppellation (
            Quality quality,
            double quantityInBaseUnits,
            int finalStringLength,
            int maximumDecimals,
            Commonness commonness,
            bool permitGranularMagnitudes,
            string separator,
            Unit preferredUnit,
            Magnitude? preferredMagnitude
        ) {

            #region find out which unit of measurement we use

            Unit unit = preferredUnit
                     ?? quality.GetBestUnitForQuantity(valueInBaseUnits: quantityInBaseUnits, commonness: commonness);

            double valueAdjustedForUnit = unit.ConvertFromBaseUnitToThisUnit(baseValue: quantityInBaseUnits);

            #endregion

            #region try to conform to the preferences provided

            if (preferredMagnitude != null) {
                Magnitude roundedMagnitude = (Magnitude) preferredMagnitude;

                double valueAdjustedToMagnitude = ScaleValueToMagnitude(
                                                                                    rawValue: valueAdjustedForUnit,
                                                                                    quality: quality,
                                                                                    magnitude: roundedMagnitude
                                                                                   );

                try {
                    return ConstructAppellationCandidate(
                                                                     unit: unit,
                                                                     finalRoundedMagnitude: roundedMagnitude,
                                                                     quantityInScaledUnits: valueAdjustedToMagnitude,
                                                                     separator: separator,
                                                                     finalStringLength: finalStringLength,
                                                                     maximumDecimals: maximumDecimals,
                                                                     commonness: commonness
                                                                    );
                } catch (InsufficientDigitsException) {
                    // Do nothing, fall through to standard (non-preferential) procedure.
                }
            }

            #endregion

            #region construct two likely adequate appellations and choose the better one

            double rawMagnitudeAdjustedForUnit = quality.GetOrderOfMagnitude(quantityInBaseUnits: valueAdjustedForUnit);

            List<Appellation> candidates = new List<Appellation>();

            foreach (Magnitude roundedMagnitude in PotentialMagnitudes(
                                                                                   rawMagnitude:
                                                                                   rawMagnitudeAdjustedForUnit,
                                                                                   permitGranularMagnitudes:
                                                                                   permitGranularMagnitudes
                                                                                  )) {

                double valueAdjustedToMagnitude = ScaleValueToMagnitude(
                                                                                    rawValue: valueAdjustedForUnit,
                                                                                    quality: quality,
                                                                                    magnitude: roundedMagnitude
                                                                                   );

                Appellation candidate = ConstructAppellationCandidate(
                                                                                  unit: unit,
                                                                                  finalRoundedMagnitude:
                                                                                  roundedMagnitude,
                                                                                  quantityInScaledUnits:
                                                                                  valueAdjustedToMagnitude,
                                                                                  separator: separator,
                                                                                  finalStringLength: finalStringLength,
                                                                                  maximumDecimals: maximumDecimals,
                                                                                  commonness: commonness
                                                                                 );

                candidates.Add(item: candidate);

            }

            return ChoosePreferableAppellation(candidates: candidates);

            #endregion

        }

        private static Appellation ChoosePreferableAppellation (List<Appellation> candidates) {
            Appellation champion;

            if (candidates.Count == 1) {
                champion = candidates[index: 0];
            } else if (candidates.Count == 0) {
                throw new Exception(message: "No candidates provided.");
            } else {
                champion = candidates.OrderBy(keySelector: c => c.isZero)
                                     .ThenBy(keySelector: c => c.overallLength)
                                     .ThenByDescending(keySelector: c => c.digitToLengthRatio)
                                     .ToList()
                                     .First();
            }

            /*
			GD.Print(candidates.Report(c => c + Chars.tab + Rounding.RoundToFirstInterestingDigit(c.digitToLengthRatio, 1) + (c.isZero ? Chars.tab + " zero " : "") + (Equals(c, champion) ? Chars.tab + " chosen" : "") + "\n") + "\n");
			*/

            return champion;

        }

        #endregion

        #region string formatting

        private static Appellation ConstructAppellationCandidate (
            Unit unit,
            Magnitude finalRoundedMagnitude,
            double quantityInScaledUnits,
            string separator,
            int finalStringLength,
            int maximumDecimals,
            Commonness commonness
        ) {

            string unitString = unit.AttachPrefix(magnitude: finalRoundedMagnitude, requestedCommonness: commonness);

            int desiredDigits = finalStringLength - unitString.Length;

            double quantityInScaledUnitsRounded =
                RoundQuantityInScaledUnitsToDesiredDigits(
                                                                      quantityInScaledUnits: quantityInScaledUnits,
                                                                      desiredDigits: desiredDigits,
                                                                      maximumDecimals: maximumDecimals
                                                                     );

            string appellation = quantityInScaledUnitsRounded + separator + unitString;

            return new Appellation(
                                   numericalValue: quantityInScaledUnitsRounded,
                                   unitAndPrefix: unitString,
                                   completeAppellation: appellation
                                  );
        }

        private class InsufficientDigitsException : Exception {

            public InsufficientDigitsException (string s)
                : base(message: s) { }

        }

        private static double RoundQuantityInScaledUnitsToDesiredDigits (
            double quantityInScaledUnits,
            int desiredDigits,
            int maximumDecimals
        ) {

            #region catch edge cases

            if (quantityInScaledUnits == 0) {
                // Is zero, has no decimals, cannot round.
                return quantityInScaledUnits;
            }

            if (quantityInScaledUnits % 1 == 0) {
                // Is round, has no decimals, cannot round.
                return quantityInScaledUnits;
            }

            if (InterestingDigitsBeforeDecimal(value: quantityInScaledUnits) >= maximumDecimals) {
                // CONVENTION: If there are more interesting digits before the decimal point than we would put behind it, round completely.
                return Rounding.Round(quantityInScaledUnits);
            }

            #endregion

            double absoluteValue = Math.Abs(quantityInScaledUnits);

            if (absoluteValue < 0) { }

            int availableDigits = desiredDigits;

            // Needs a point
            availableDigits -= 1;

            if (quantityInScaledUnits < 0) {
                // Needs a signum
                availableDigits -= 1;
            }

            int digitsBeforeDecimal = InterestingDigitsBeforeDecimal(value: quantityInScaledUnits);
            int availaleDecimals = availableDigits - digitsBeforeDecimal;

            int minimumNecessaryDecimals;

            if (absoluteValue < 1) {
                minimumNecessaryDecimals =
                    Strings.ZerosAfterDecimalBeforeInterestingDigitsStart(value: absoluteValue) + 1;
            } else {
                minimumNecessaryDecimals = 0;
            }

            // GD.Print("MND " + absoluteValue + " -> " + minimumNecessaryDecimals + "\n");

            availaleDecimals = Math.Clamp(
                                           value: Math.Max(
                                                            minimumNecessaryDecimals,
                                                            Math.Min(availaleDecimals, maximumDecimals)
                                                           ),
                                           min: 0,
                                           max: 64
                                          );

            double roundedValue = Rounding.Round(value: quantityInScaledUnits, decimals: availaleDecimals);

            return roundedValue;
        }

        #endregion

        #region finding closest magnitudes

        private static List<Magnitude> PotentialMagnitudes (double rawMagnitude, bool permitGranularMagnitudes) {

            int ceiled = CeilMagnitude(
                                                   rawMagnitude: rawMagnitude,
                                                   permitGranularMagnitudes: permitGranularMagnitudes
                                                  );

            int floored = FloorMagnitude(
                                                     rawMagnitude: rawMagnitude,
                                                     permitGranularMagnitudes: permitGranularMagnitudes
                                                    );

            return new List<Magnitude> { (Magnitude) ceiled, (Magnitude) floored };
        }

        private static int RoundMagnitude (double rawMagnitude, bool permitGranularMagnitudes) {
            if (IsWithinGranularMagnitudeRange(
                                                           rawMagnitude: rawMagnitude,
                                                           permitGranularMagnitudes: permitGranularMagnitudes
                                                          )) {
                return Mathematik.RoundToInt(rawMagnitude);
            }

            return RoundToMultiplesOfThree(value: rawMagnitude);
        }

        private static int FloorMagnitude (double rawMagnitude, bool permitGranularMagnitudes) {
            if (IsWithinGranularMagnitudeRange(
                                                           rawMagnitude: rawMagnitude,
                                                           permitGranularMagnitudes: permitGranularMagnitudes
                                                          )) {
                return Mathematik.FloorToInt(rawMagnitude);
            }

            return FloorToMultiplesOfThree(value: rawMagnitude);
        }

        private static int CeilMagnitude (double rawMagnitude, bool permitGranularMagnitudes) {
            if (IsWithinGranularMagnitudeRange(
                                                           rawMagnitude: rawMagnitude,
                                                           permitGranularMagnitudes: permitGranularMagnitudes
                                                          )) {
                return Mathematik.CeilToInt(rawMagnitude);
            }

            return CeilToMultiplesOfThree(value: rawMagnitude);
        }

        private static bool IsWithinGranularMagnitudeRange (double rawMagnitude, bool permitGranularMagnitudes) {
            if (permitGranularMagnitudes == false) {
                return false;
            }

            return Math.Abs(rawMagnitude) <= 3;
        }

        private static int RoundToMultiplesOfThree (double value) {
            return Mathematik.RoundToInt(value / 3d) * 3;
        }

        private static int FloorToMultiplesOfThree (double value) {
            return Mathematik.FloorToInt(value / 3d) * 3;
        }

        private static int CeilToMultiplesOfThree (double value) {
            return Mathematik.CeilToInt(value / 3d) * 3;
        }

        #endregion

        #region utility

        private static double ScaleValueToMagnitude (double rawValue, Quality quality, Magnitude magnitude) {
            return rawValue / MagnitudeToScale(quality: quality, magnitude: magnitude);
        }

        private static double MagnitudeToScale (Quality quality, Magnitude magnitude) {
            return Math.Pow(quality.SingleMagnitudeScale(), (int) magnitude);
        }

        private static int InterestingDigitsBeforeDecimal (double value) {
            double absoluteValue = Math.Abs(value);

            if (absoluteValue < 1) {
                return 0;
            }

            return Mathematik.FloorToInt(absoluteValue).ToString().Length;
        }

        #endregion

    }

}