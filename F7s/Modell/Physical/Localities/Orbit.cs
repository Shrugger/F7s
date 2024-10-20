using F7s.Utility;
using F7s.Utility.Lazies;
using F7s.Utility.Measurements;
using Stride.Core.Mathematics;
using System;
using F7s.Utility.Geometry;
using F7s.Utility.Geometry.Double;

namespace F7s.Modell.Physical.Localities {

    public class Ephemeris {
        public Vector3d GetTranslation () {
            return this.absolutePosition;
        }

        public Vector3d GetLinearVelocity () {
            return this.linearVelocity;
        }

        public Vector3 GetAngularVelocity () {
            return this.angularVelocity;
        }

        public Quaternion GetRotation () {
            if (!Mathematik.Valid(this.absoluteRotation)) {
                throw new Exception("Invalid rotation " + this.absoluteRotation + " out of " + this.ToString());
            }
            return this.absoluteRotation;
        }

        public override string ToString () {
            return "Ephemeris " + this.absolutePosition + " + " + this.linearVelocity + ", " + this.absoluteRotation.ToString() + " + " + this.angularVelocity;
        }

        private readonly Vector3d absolutePosition;
        private readonly Vector3d linearVelocity;
        private readonly Quaternion absoluteRotation;
        private readonly Vector3 angularVelocity;

        public Ephemeris (Vector3d absolutePosition, Vector3d linearVelocity, Quaternion absoluteRotation, Vector3 angularVelocity) {
            this.absolutePosition = absolutePosition;
            this.linearVelocity = linearVelocity;
            this.absoluteRotation = absoluteRotation;
            this.angularVelocity = angularVelocity;
        }
    }

    public class Orbit {

        private double inclination;
        private double longitudeOfAscendingNode;
        private double argumentOfPeriapsis;
        private double eccentricity;
        private double semimajorAxis;
        private double trueAnomaly;
        public double startingTrueAnomaly;

        public Vector3 rotationOffset;
        public Vector3 angularVelocity;

        private PhysicalEntity orbiter;
        private PhysicalEntity parent;

        private LazyMonoMemory<double, Ephemeris> lastAbsoluteEphemera;
        private LazyMonoMemory<double, Ephemeris> lastRelativeEphemera;

        public override string ToString () {
            return this.orbiter.Name + " " + Measurement.MeasureLength((float) this.semimajorAxis) + " and " + Measurement.MeasureTime((float) this.OrbitalPeriod()) + " around " + this.parent.Name;
        }

        public float RotationalPeriod () {
            return 360f / this.angularVelocity.Length();
        }

        public Quaternion GetRotation (double time) {
            float modulodTime = (float) (time % (double) this.RotationalPeriod());
            return Mathematik.DegreesToQuaternion(this.rotationOffset + this.angularVelocity * modulodTime);
        }

        public double GetInclination () {
            return this.inclination;
        }
        public double GetLongitudeOfAscendingNode () {
            return this.longitudeOfAscendingNode;
        }
        public double GetArgumentOfPeriapsis () {
            return this.argumentOfPeriapsis;
        }
        public double GetEccentricity () {
            return this.eccentricity;
        }
        public double GetSemimajorAxis () {
            return this.semimajorAxis;
        }
        public double GetTrueAnomaly () {
            return this.trueAnomaly;
        }
        public PhysicalEntity GetParent () {
            return this.parent;
        }
        public double GetStartingTrueAnomaly () {
            return this.startingTrueAnomaly;
        }

        /// <summary>
        /// A keplerian orbit.
        /// </summary>
        /// <param name="inclination">In degrees.</param>
        /// <param name="longitudeOfAscendingNode">In degrees.</param>
        /// <param name="argumentOfPeriapsis">In degrees.</param>
        /// <param name="eccentricity">Dimensionless. 0 is circular, between 0 and 1 is elliptic, 1 is a parabolic escape or capture orbit, greater than 1 is a hperbola.</param>
        /// <param name="semimajorAxis">In meters.</param>
        /// <param name="trueAnomaly">In degrees.</param>
        /// <param name="parent">The dominant gravity well.</param>
        public Orbit (double inclination, double longitudeOfAscendingNode, double argumentOfPeriapsis, double eccentricity, double semimajorAxis, double trueAnomaly, PhysicalEntity parent, Vector3? angularVelocity = null) {

            if (parent == null) {
                throw new Exception();
            }

            this.inclination = inclination;
            this.longitudeOfAscendingNode = longitudeOfAscendingNode;
            this.argumentOfPeriapsis = argumentOfPeriapsis;
            this.eccentricity = eccentricity;
            this.semimajorAxis = semimajorAxis;
            this.trueAnomaly = trueAnomaly;
            this.startingTrueAnomaly = trueAnomaly;
            this.parent = parent;

            if (angularVelocity.HasValue) {
                this.angularVelocity = angularVelocity.Value;
            }

            this.lastAbsoluteEphemera = new LazyMonoMemory<double, Ephemeris>(time => OrbitToAbsoluteKinematics(this, time));
            this.lastRelativeEphemera = new LazyMonoMemory<double, Ephemeris>(time => OrbitToRelativeKinematics(this, time));
        }
        public Orbit (Orbit original) : this(original.inclination, original.longitudeOfAscendingNode, original.argumentOfPeriapsis, original.eccentricity, original.semimajorAxis, original.trueAnomaly, original.parent) { }

        private static double GravitationalConstant () {
            return Constants.GravitationalConstant;
        }

        public void SetOrbiter (PhysicalEntity entity) {
            this.orbiter = entity;
        }

        public double CalculateMu () {
            return CalculateMu(this.orbiter, this.parent);
        }

        public static double CalculateMu (PhysicalEntity orbiter, PhysicalEntity parent) {
            double aMass = (orbiter?.CollectiveMass() ?? 0.0);
            double bMass = (parent?.CollectiveMass() ?? 0.0);
            if (double.IsInfinity(aMass) || aMass <= 0) {
                throw new Exception("Mass " + aMass + " from " + orbiter);
            }
            if (double.IsInfinity(bMass) || bMass <= 0) {
                throw new Exception("Mass " + bMass + " from " + parent);
            }
            return GravitationalConstant() * (aMass + bMass);
        }
        public static double OrbitalPeriod (double semiMajorAxis, double mu) {
            return 2 * Math.PI * Math.Sqrt((semiMajorAxis * semiMajorAxis * semiMajorAxis) / mu);
        }

        public static Orbit KinematicsToOrbit (Ephemeris kinematics, double mu, double time, PhysicalEntity parent) {
            Vector3d p = kinematics.GetTranslation();                                                 //Position
            Vector3d v = kinematics.GetLinearVelocity();                                                          //Velocity
            Vector3d l = Vector3d.Cross(p, v);                                                              //Specific Angular Momentum
            double r = p.Length();                                                                     //Radius	
            double en = ((v.Length() * v.Length()) / 2.0) - (mu / r);                                     //Specific Energy
            double a = -1 * (mu / (2 * en));                                                                //Semi-Major Axis
            double e = Math.Sqrt(1 - ((l.Length() * l.Length()) / (a * mu)));                            //Eccentricity
            double i = l.Z / l.Length();                                                                   //Inclination
            double om = Math.Atan2(l.X, -l.Y);                                                         //Right Ascension
            double lat = a * Math.Atan2(p.Z / Math.Sin(i), p.X * Math.Cos(om) + p.Y * Math.Sin(om));    //Argument of Latitude
            double f = Math.Acos((e * r) / (Math.Abs(e) * Math.Abs(r)));                             //True Anomaly
            double w = lat - f;                                                                         //Argument of Periapsis

            /*double inclination = i;
			double longitudeOfAscendingNode = om;
			double argumentOfPeriapsis = w;
			double eccentricity = e;
			double semimajorAxis = a;
			double trueAnomaly = f;*/

            return new Orbit(i, om, w, e, a, f, parent);
        }

        private static Ephemeris OrbitToAbsoluteKinematics (Orbit orbit, double time) {
            return OrbitToKinematics(orbit, time, true);
        }
        private static Ephemeris OrbitToRelativeKinematics (Orbit orbit, double time) {
            return OrbitToKinematics(orbit, time, false);
        }

        public Ephemeris OrbitToAbsoluteKinematics (double time) {
            return this.lastAbsoluteEphemera.GetValue(time);
        }
        public Ephemeris OrbitToRelativeKinematics (double time) {
            return this.lastRelativeEphemera.GetValue(time);
        }

        private static Ephemeris OrbitToKinematics (Orbit orbit, double time, bool absolute) {

            double mu = CalculateMu(orbit.orbiter, orbit.parent);
            if (!double.IsFinite(mu) || mu <= 0) {
                throw new Exception();
            }

            double i = orbit.GetInclination() * Mathematik.Deg2Rad;
            double om = orbit.GetLongitudeOfAscendingNode() * Mathematik.Deg2Rad;
            double e = orbit.GetEccentricity();
            double a = orbit.GetSemimajorAxis();

            double m = Math.Sqrt(mu / (a * a * a)) * (time);
            double ea = m;
            if (double.IsInfinity(ea)) {
                throw new Exception("From mu " + mu + ", a " + a + ", time " + time + ".");
            }
            double dea = (m + e * Math.Sin(ea) - ea) / (1 - e * Math.Cos(ea));
            while (dea > 0.00001) { //Default: 0.000001
                ea += dea;
                dea = (m + e * Math.Sin(ea) - ea) / (1 - e * Math.Cos(ea));
            }
            double r = a * (1 - e * Math.Cos(ea));
            double f = 2 * Math.Atan((Math.Sqrt((1 + e) / (1 - e))) * Math.Tan(ea / 2.0));
            if (double.IsInfinity(f)) {
                throw new Exception("From e " + e + ", ea " + ea + ".");
            }
            //if(!ignoreActualTrueAnomaly){
            f += orbit.GetStartingTrueAnomaly();
            if (double.IsNaN(f)) {
                throw new Exception("From " + f);
            }
            //}
            f = f % (2 * Math.PI);
            if (double.IsNaN(f)) {
                throw new Exception("From " + f);
            }

            double fCos = Math.Cos(f);
            double fSin = Math.Sin(f);
            if (double.IsNaN(fCos)) {
                throw new Exception("From " + f);
            }
            if (double.IsNaN(fSin)) {
                throw new Exception("From " + f);
            }
            Vector3d x = new Vector3d(fCos, 0, fSin) * r;
            Vector3d v = new Vector3d(-fSin, 0, e + fCos) * Math.Sqrt(mu / (a * (1 - e * e)));

            double wf = ((orbit.GetArgumentOfPeriapsis()) * Mathematik.Deg2Rad);
            /*Matrix3x3d m1 = new Matrix3x3d	(	Math.Cos(om * Geometry.Deg2Rad),	-Math.Sin(om * Geometry.Deg2Rad),	0,
												Math.Sin(om * Geometry.Deg2Rad),	Math.Cos(om * Geometry.Deg2Rad),	1,
												0,				0,				0);
			Matrix3x3d m2 = new Matrix3x3d	(	1,				0,				0,
												0,				Math.Cos(i * Geometry.Deg2Rad),	-Math.Sin(i * Geometry.Deg2Rad),
												0,				Math.Sin(i * Geometry.Deg2Rad),	Math.Cos(i * Geometry.Deg2Rad));
			Matrix3x3d m3 = new Matrix3x3d	(	Math.Cos(wf),	-Math.Sin(wf),	0,
												Math.Sin(wf),	Math.Cos(wf),	0,
												0,				0,				1);

			Matrix3x3d mFinal = m1 * m2 * m3;*/

            Matrix3x3d mF2 = new Matrix3x3d((Math.Cos(om) * Math.Cos(wf)) - (Math.Sin(om) * Math.Sin(wf) * Math.Cos(i)), (-Math.Cos(om) * Math.Sin(wf)) - (Math.Sin(om) * Math.Cos(wf) * Math.Cos(i)), Math.Sin(om) * Math.Sin(i),
                                            (Math.Sin(om) * Math.Cos(wf)) + (Math.Cos(om) * Math.Sin(wf) * Math.Cos(i)), (-Math.Sin(om) * Math.Sin(wf)) + (Math.Cos(om) * Math.Cos(wf) * Math.Cos(i)), -Math.Cos(om) * Math.Sin(i),
                                            Math.Sin(wf) * Math.Sin(i), Math.Cos(wf) * Math.Sin(i), Math.Cos(i));

            Vector3d x2 = mF2 * new Vector3d(x.X, x.Z, x.Y);
            x = new Vector3d(x2.X, x2.Z, x2.Y);
            /*double xMag = x.Length();
			x = new Vector3d(	xMag * (Math.Cos(om * Geometry.Deg2Rad) * Math.Cos(wf) - Math.Sin(om * Geometry.Deg2Rad) * Math.Sin(wf) * Math.Cos(i * Geometry.Deg2Rad)), 
								xMag * (Math.Sin(wf) * Math.Sin(i * Geometry.Deg2Rad)), 
								xMag * (Math.Sin(om * Geometry.Deg2Rad) * Math.Cos(wf) + Math.Cos(om * Geometry.Deg2Rad) * Math.Sin(wf) * Math.Cos(i * Geometry.Deg2Rad)));
			*/

            Vector3d parentPosition = absolute ? orbit.GetParent().CalculatePositionAtTime(time) : Vector3.Zero;
            return new Ephemeris((x + parentPosition).ToVector3(), v.ToVector3(), orbit.GetRotation(time), orbit.angularVelocity);
        }

        public Vector3d CalculateRelativePositionAtTime (double time) {
            return this.OrbitToRelativeKinematics(time).GetTranslation();
        }

        public Vector3d CalculateAbsolutePositionAtTime (double time) {
            return this.OrbitToAbsoluteKinematics(time).GetTranslation();
        }

        public double OrbitalPeriod () {
            return OrbitalPeriod(this.semimajorAxis, this.CalculateMu());
        }

    }
}