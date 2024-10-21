using System; using F7s.Utility.Geometry.Double; using Stride.Core.Mathematics;

namespace F7s.Modell.Physical {
    public static class Constants {

        public const float blackbodyColorCutoff = 500;
        public const float redHotPeak = 1000;
        public const float whiteHotPeak = 6500;
        public const float blueHotPeak = 12500;

        public const double CelsiusOffsetToKelvin = 273.15;

        public const double Mole = 6.02214179e23;

        /// <summary> In meters. </summary>
        public const double AstronomicUnit = 149597870700;

        /// <summary> In meters per second. </summary>
        public const double SpeedOfLight = 299792458;

        /// <summary> In meters square times kilograms per second. </summary>
        public const double PlankConstant = 6.626068e-34;

        /// <summary>
        ///     In number per Mole.
        /// </summary>
        public const double AvogadroConstant = 6.02214076e23;

        /// <summary>
        ///     In Joules per Kelvin.
        /// </summary>
        public const double BoltzmannConstant = 1.3806485279e-23;

        /// <summary>
        ///     In Joules per (Mole * Kelvin).
        /// </summary>
        public const double GasConstant = 8.314459848;

        /// <summary>
        ///     In meters per second square.
        /// </summary>
        public const double StandardEarthGravity = 9.80665;

        /// <summary>
        ///     In meters cubed.
        /// </summary>
        public static readonly double VolumeOfEarth = 1.083207 * Math.Pow(10, 21);


        public static readonly float RadiusOfTheSun = 696340000;

        public static readonly double SolarLuminosityInWatts = 3.828d * Math.Pow(10, 26);
        public static readonly float AngularVelocityOfTheSunInDegreesPerSecond = 360f / SecondsInADay;

        #region units of time

        public const int DaysInAStandardYear = 365;
        public const int MonthsInAStandardYear = 12;
        public const int HoursInADay = 24;
        public const int MinutesInAnHour = 60;
        public const int SecondsInAMinute = 60;

        public static int SecondsInADay => Constants.MinutesInADay * Constants.SecondsInAMinute;

        public static int MinutesInADay => Constants.MinutesInAnHour * Constants.HoursInADay;

        /// <summary> m^3 * kg^-1 * s^-2</summary>
        public static readonly double GravitationalConstant = 6.674 * Math.Pow(10, -11);


        public static readonly double MassOfEarth = 5.972 * Math.Pow(10, 24);

        #endregion

    }

}