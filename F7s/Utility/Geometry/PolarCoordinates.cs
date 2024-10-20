using F7s.Geometry;
using F7s.Utility.Geometry.Double;
using F7s.Utility.Lazies;
using Stride.Core.Mathematics;
using System;

namespace F7s.Utility.Geometry
{


    /// <summary>
    ///     Polar Coordinates describe an objects position relative to the center of a sphere. Values in this struct are
    ///     longitude, latitude and radial distance.
    /// </summary>

    public class PolarCoordinates : Coordinates {

        /// <summary>
        ///     Azimuthal Angle Phi
        /// </summary>
        public readonly double longitude;

        /// <summary>
        ///     Polar Angle Theta
        /// </summary>
        public readonly double latitude;

        /// <summary>
        ///     Radial Distance, also known as r or Rho
        /// </summary>
        public readonly double radialDistance;

        private Faul<Vector3> asVector3;
        private Faul<Vector3d> asVector3d;

        /// <param name="longitude"> Longitude in degrees from 0° to 360°. </param>
        /// <param name="latitude"> Latitude in degrees from -90° to 90°. </param>
        /// <param name="radialDistance"> Distance from the center of the sphere. </param>
        public PolarCoordinates (double longitude, double latitude, double radialDistance) {
            if (double.IsNaN(longitude) || double.IsNaN(latitude) || double.IsNaN(radialDistance)) {
                throw new ArgumentException(
                                            message: "Longitude: "
                                                   + longitude
                                                   + ", latitude: "
                                                   + latitude
                                                   + ", radial distance: "
                                                   + radialDistance
                                           );
            }

            // Limit longitude to between 0 and 360 degrees.
            this.longitude = ConvertToLongitude(longitude);
            if (this.longitude > 360 || this.longitude < 0) {
                throw new Exception("Illegal longitude " + longitude + ", converted to " + this.longitude);
            }


            // Limit latitude to between -90 and 90 degrees and invert longitude if absolute latitude exceeds 90 an odd number of times.

            this.latitude = latitude;

            if (latitude < -90 || latitude > 90) {
                if (latitude > 90 && latitude < 90.000002) {
                    latitude = 90;
                } else if (latitude < -90 && latitude > -90.000002) {
                    latitude = -90;
                } else {
                    throw new ArgumentException(message: "Illegal latitude: " + latitude);
                }
            }

            this.radialDistance = radialDistance;

            if (radialDistance < 0) {
                // Radial distance is negative, so assume it's somehow on the wrong side and invert the coordinates.
                this.radialDistance = -radialDistance;
                this.longitude = ConvertToLongitude(this.longitude + 180.0);
                this.latitude = -latitude;
            }

            InitializeCartesianBackingLazies();
        }

        private void InitializeCartesianBackingLazies () {
            PolarCoordinates thisCoordinates = this;
            asVector3 = new Faul<Vector3>(() => thisCoordinates.ConvertPolarCoordinatesToVector3());
            asVector3d = new Faul<Vector3d>(() => thisCoordinates.ConvertPolarCoordinatesToVector3d());
        }

        public static double ConvertToLongitude (double raw) {
            if (raw < 0) {
                raw += 360;
            }
            double longitude = raw % 360.0;

            if (longitude == 360.0) {
                longitude = 0.0;
            }

            return longitude;
        }

        public PolarCoordinates SetRadialDistance (double radialDistance) {
            return new PolarCoordinates(longitude, latitude, radialDistance);
        }
        #region predefined coordinates

        /// <summary>
        ///     0° longitude, 0° latitude, 0 radial distance.
        /// </summary>
        public static PolarCoordinates zero => new PolarCoordinates(longitude: 0d, latitude: 0d, radialDistance: 0d);

        /// <summary>
        ///     0° longitude, 1° latitude, 0 radial distance.
        /// </summary>
        public static PolarCoordinates OneDegreeNorth =>
            new PolarCoordinates(longitude: 0d, latitude: 1d, radialDistance: 0d);

        /// <summary>
        ///     0° longitude, -1° latitude, 0 radial distance.
        /// </summary>
        public static PolarCoordinates OneDegreeSouth =>
            new PolarCoordinates(longitude: 0d, latitude: -1d, radialDistance: 0d);

        /// <summary>
        ///     1° longitude, 0° latitude, 0 radial distance.
        /// </summary>
        public static PolarCoordinates OneDegreeEast =>
            new PolarCoordinates(longitude: 1d, latitude: 0d, radialDistance: 0d);

        /// <summary>
        ///     -1° longitude, 0° latitude, 0 radial distance.
        /// </summary>
        public static PolarCoordinates OneDegreeWest =>
            new PolarCoordinates(longitude: -1d, latitude: 0d, radialDistance: 0d);

        /// <summary>
        ///     0° longitude, 0° latitude, 1 radial distance.
        /// </summary>
        public static PolarCoordinates OneUnitAway =>
            new PolarCoordinates(longitude: 0d, latitude: 0d, radialDistance: 1d);

        /// <summary>
        ///     0° longitude, 0° latitude, -1 radial distance.
        /// </summary>
        public static PolarCoordinates OneUnitTowards =>
            new PolarCoordinates(longitude: 0d, latitude: 0d, radialDistance: -1d);


        #endregion

        #region conversion

        /// <summary>
        ///     Convert polar coordinates to a cartesian position.
        /// </summary>
        /// <param name="polar"> The polar coordinates meant to be converted. </param>
        /// <returns> The converted coordinates, now as a cartesian vector. </returns>
        public static Vector3d ToCartesian (PolarCoordinates polar) {
            double phi = Mathematik.DegToRad((polar.longitude + 0) % 360);
            double theta = Mathematik.DegToRad(polar.latitude + 90.0);
            double rho = polar.radialDistance;
            double sinTheta = Math.Sin(a: theta);
            double z = rho * sinTheta * Math.Cos(phi);
            double y = rho * Math.Cos(theta);
            double x = rho * sinTheta * Math.Sin(a: phi);

            return new Vector3d(x: x, y: -y, z: z); // I don't think y should be negative, actually.
        }

        /// <summary>
        ///     Convert a cartesian position to polar coordinates.
        /// </summary>
        /// <param name="cartesian"> The cartesian vector meant to be converted. </param>
        /// <returns> The polar coordinates representing the given cartesian position. </returns>
        public static PolarCoordinates FromCartesian (Vector3d cartesian) {
            if (cartesian.X == 0
             && cartesian.Y == 0
             && cartesian.Z == 0) {
                return new PolarCoordinates(longitude: 0, latitude: 0, radialDistance: 0);
            }

            double x = cartesian.X;
            double y = -cartesian.Y;
            double z = cartesian.Z;
            double phi = (Mathematik.RadToDeg(Math.Atan2(y: x, x: z)) - 0) % 360;
            double rho = Math.Sqrt(x * x + y * y + z * z);
            double theta = Mathematik.RadToDeg(Math.Acos(y / rho)) - 90;

            return new PolarCoordinates(longitude: phi, latitude: theta, radialDistance: rho);
        }

        #endregion

        #region casts

        private PolarCoordinates ConvertFromCartesianToPolarCoordinates (Vector3d cartesian) {
            return FromCartesian(cartesian);
        }
        private PolarCoordinates ConvertFromCartesianToPolarCoordinates (Vector3 cartesian) {
            return FromCartesian(cartesian);
        }
        private Vector3d ConvertPolarCoordinatesToVector3d () {
            return ToCartesian(this);
        }
        private Vector3 ConvertPolarCoordinatesToVector3 () {
            return ConvertPolarCoordinatesToVector3d().ToVector3();
        }

        /// <summary>
        ///     Convert these polar coordinates to a two-dimensional vector containing longitude and latitude.
        /// </summary>
        /// <returns> A two-dimensional vector containing longitude and latitude of these polar coordinates. </returns>
        public Vector2 LongLatToVector2 () {
            return new Vector2(x: (float) longitude, y: (float) latitude);
        }

        /// <summary>
        ///     Implicitly cast a cartesian position to polar coordinates.
        /// </summary>
        /// <param name="v3d"> The cartesian vector meant to be converted. </param>
        /// <returns> The polar coordinates representing the given cartesian position. </returns>
        public static implicit operator PolarCoordinates (Vector3d v3d) {
            return FromCartesian(cartesian: v3d);
        }

        public static implicit operator PolarCoordinates (Vector3 v3) {
            return FromCartesian(cartesian: v3);
        }

        public static implicit operator Vector3d (PolarCoordinates polar) {
            return polar.asVector3d;
        }
        public static implicit operator Vector3 (PolarCoordinates polar) {
            return polar.asVector3;
        }

        #endregion

        #region miscellaneous

        public override string ToString () {
            double lon = Mathematik.Round(value: longitude);
            double lat = Mathematik.Round(value: latitude);

            string lonD = Math.Abs(value: lon - 180)
                        + (lon < 0
                               ? "W"
                               : lon > 0
                                   ? "E"
                                   : "");

            string latD = Math.Abs(value: lat)
                        + (lat < 0
                               ? "N"
                               : lat > 0
                                   ? "S"
                                   : "");

            return "(" + lonD + ", " + latD + ", " + Mathematik.Round(value: radialDistance) + ")";
        }
        public override bool Equals (object obj) {
            if (obj == null) {
                return false;
            }

            if (obj.GetType() != typeof(PolarCoordinates)) {
                return false;
            }

            PolarCoordinates other = (PolarCoordinates) obj;

            return base.Equals(obj: obj) && longitude == other.longitude && latitude == other.latitude && radialDistance == other.radialDistance;
        }

        /// <summary>
        ///     Binary equality operator. Uses PolarCoordinate.Equals.
        /// </summary>
        /// <param name="lhs"> Left-hand side object to compare. </param>
        /// <param name="rhs"> Right-hand side object to compare. </param>
        /// <returns> True if both objects are polar coordinates and represent the same position. </returns>
        public static bool operator == (PolarCoordinates lhs, PolarCoordinates rhs) {
            return lhs.Equals(obj: rhs);
        }

        /// <summary>
        ///     Binary non-equality operator. Uses PolarCoordinate.Equals.
        /// </summary>
        /// <param name="lhs"> Left-hand side object to compare. </param>
        /// <param name="rhs"> Right-hand side object to compare. </param>
        /// <returns> False if both objects are polar coordinates and represent the same position. </returns>
        public static bool operator != (PolarCoordinates lhs, PolarCoordinates rhs) {
            return lhs.Equals(obj: rhs) == false;
        }

        public static bool ApproximatelyEquals (PolarCoordinates lhs, PolarCoordinates rhs) {
            return Mathematik.IsEqualApprox(a: lhs.latitude, b: rhs.latitude)
                && Mathematik.IsEqualApprox(a: lhs.longitude, b: rhs.longitude)
                && Mathematik.IsEqualApprox(a: lhs.radialDistance, b: rhs.radialDistance);
        }

        /// <summary>
        ///     Override of Object.GetHashCode. TODO: Do some suitable hashing. At present it merely uses Object.GetHashCode.
        /// </summary>
        /// <returns> An integer hash code representing this object. </returns>
        public override int GetHashCode () {
            return base.GetHashCode();
        }

        public PolarCoordinates ShiftRadius (
            double addition,
            double minRadius = 0,
            double maxRadius = double.MaxValue
        ) {
            return new PolarCoordinates(
                                        longitude: longitude,
                                        latitude: latitude,
                                        radialDistance: Math.Clamp(
                                                                    value: radialDistance + addition,
                                                                    min: minRadius,
                                                                    max: maxRadius
                                                                   )
                                       );
        }

        #endregion

        public Vector3d ToVector3d () {
            return asVector3d;
        }

        public PolarCoordinates RelativePolarCoordinatesDouble () {
            return this;
        }

        public double CartesianDistanceDouble (Coordinates other) {
            return DirectDistance(other: other.Polar());
        }

        public Vector3 ToVector3 () {
            return asVector3;
        }

        public double MagnitudeDouble () {
            return radialDistance;
        }

        public double PolarDistanceDouble (Coordinates other, double projectionRadius) {
            return PolarDistanceDouble(this, other, projectionRadius);
        }
        public static double PolarDistanceDouble (Coordinates one, Coordinates other, double projectionRadius = 1.0) {
            PolarCoordinates oneCoordinates = one.Polar();
            PolarCoordinates otherCoordinates = other.Polar();
            return Mathematik.GreatCircleDistance(oneCoordinates.latitude, oneCoordinates.longitude, otherCoordinates.latitude, otherCoordinates.longitude, projectionRadius);
        }

        public double DirectDistance (PolarCoordinates other) {
            return Vector3d.Distance(a: this, b: other);
        }

        public static double DirectDistance (PolarCoordinates lhs, PolarCoordinates rhs) {
            return lhs.DirectDistance(other: rhs);
        }

        public PolarCoordinates Polar () {
            return this;
        }

        public static PolarCoordinates operator + (PolarCoordinates a, PolarCoordinates b) {
            return new PolarCoordinates(
                                        longitude: a.longitude + b.longitude,
                                        latitude: a.latitude + b.latitude,
                                        radialDistance: (a.radialDistance + b.radialDistance) / 2.0
                                       );
        }

        public static PolarCoordinates operator - (PolarCoordinates a, PolarCoordinates b) {
            PolarCoordinates result = new PolarCoordinates(
                                        longitude: a.longitude - b.longitude,
                                        latitude: a.latitude - b.latitude,
                                        radialDistance: (a.radialDistance + b.radialDistance) / 2.0
                                       );
            return result;
        }


    }

}