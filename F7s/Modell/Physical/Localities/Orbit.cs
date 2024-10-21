using F7s.Utility;
using F7s.Utility.Geometry;
using F7s.Utility.Geometry.Double;
using F7s.Utility.Lazies;
using F7s.Utility.Measurements;
using Stride.Core.Mathematics;
using System;

namespace F7s.Modell.Physical.Localities {

    public class Ephemeris {
        public Double3 GetTranslation () {
            return absolutePosition;
        }

        public Double3 GetLinearVelocity () {
            return linearVelocity;
        }

        public Double3 GetAngularVelocity () {
            return angularVelocity;
        }

        public QuaternionD GetRotation () {
            if (!Mathematik.Valid(absoluteRotation)) {
                throw new Exception("Invalid rotation " + absoluteRotation + " out of " + ToString());
            }
            return absoluteRotation;
        }

        public override string ToString () {
            return "Ephemeris " + absolutePosition + " + " + linearVelocity + ", " + absoluteRotation.ToString() + " + " + angularVelocity;
        }

        private readonly Double3 absolutePosition;
        private readonly Double3 linearVelocity;
        private readonly QuaternionD absoluteRotation;
        private readonly Double3 angularVelocity;

        public Ephemeris (Double3 absolutePosition, Double3 linearVelocity, QuaternionD absoluteRotation, Double3 angularVelocity) {
            this.absolutePosition = absolutePosition;
            this.linearVelocity = linearVelocity;
            this.absoluteRotation = absoluteRotation;
            this.angularVelocity = angularVelocity;
        }
    }

    public class Orbit {

        private readonly double inclination;
        private readonly double longitudeOfAscendingNode;
        private readonly double argumentOfPeriapsis;
        private readonly double eccentricity;
        private readonly double semimajorAxis;
        private readonly double trueAnomaly;
        public double startingTrueAnomaly;

        public Double3 rotationOffset;
        public Double3 angularVelocity;

        private PhysicalEntity orbiter;
        private readonly PhysicalEntity parent;

        private readonly LazyMonoMemory<double, Ephemeris> lastAbsoluteEphemera;
        private readonly LazyMonoMemory<double, Ephemeris> lastRelativeEphemera;

        public override string ToString () {
            return orbiter.Name + " " + Measurement.MeasureLength(semimajorAxis) + " and " + Measurement.MeasureTime((double) OrbitalPeriod()) + " around " + parent.Name;
        }

        public double RotationalPeriod () {
            return 360 / angularVelocity.Length();
        }

        public QuaternionD GetRotation (double time) {
            double modulodTime = (double) (time % (double) RotationalPeriod());
            return Mathematik.DegreesToQuaternionD(rotationOffset + (angularVelocity * modulodTime));
        }

        public double GetInclination () {
            return inclination;
        }
        public double GetLongitudeOfAscendingNode () {
            return longitudeOfAscendingNode;
        }
        public double GetArgumentOfPeriapsis () {
            return argumentOfPeriapsis;
        }
        public double GetEccentricity () {
            return eccentricity;
        }
        public double GetSemimajorAxis () {
            return semimajorAxis;
        }
        public double GetTrueAnomaly () {
            return trueAnomaly;
        }
        public PhysicalEntity GetParent () {
            return parent;
        }
        public double GetStartingTrueAnomaly () {
            return startingTrueAnomaly;
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
        public Orbit (double inclination, double longitudeOfAscendingNode, double argumentOfPeriapsis, double eccentricity, double semimajorAxis, double trueAnomaly, PhysicalEntity parent, Double3? angularVelocity = null) {

            if (parent == null) {
                throw new Exception();
            }

            this.inclination = inclination;
            this.longitudeOfAscendingNode = longitudeOfAscendingNode;
            this.argumentOfPeriapsis = argumentOfPeriapsis;
            this.eccentricity = eccentricity;
            this.semimajorAxis = semimajorAxis;
            this.trueAnomaly = trueAnomaly;
            startingTrueAnomaly = trueAnomaly;
            this.parent = parent;

            if (angularVelocity.HasValue) {
                this.angularVelocity = angularVelocity.Value;
            }

            lastAbsoluteEphemera = new LazyMonoMemory<double, Ephemeris>(time => OrbitToAbsoluteKinematics(this, time));
            lastRelativeEphemera = new LazyMonoMemory<double, Ephemeris>(time => OrbitToRelativeKinematics(this, time));
        }
        public Orbit (Orbit original) : this(original.inclination, original.longitudeOfAscendingNode, original.argumentOfPeriapsis, original.eccentricity, original.semimajorAxis, original.trueAnomaly, original.parent) { }

        private static double GravitationalConstant () {
            return Constants.GravitationalConstant;
        }

        public void SetOrbiter (PhysicalEntity entity) {
            orbiter = entity;
        }

        public double CalculateMu () {
            return CalculateMu(orbiter, parent);
        }

        public static double CalculateMu (PhysicalEntity orbiter, PhysicalEntity parent) {
            double aMass = orbiter?.CollectiveMass() ?? 0.0;
            double bMass = parent?.CollectiveMass() ?? 0.0;
            if (double.IsInfinity(aMass) || aMass <= 0) {
                throw new Exception("Mass " + aMass + " from " + orbiter);
            }
            if (double.IsInfinity(bMass) || bMass <= 0) {
                throw new Exception("Mass " + bMass + " from " + parent);
            }
            return GravitationalConstant() * (aMass + bMass);
        }
        public static double OrbitalPeriod (double semiMajorAxis, double mu) {
            return 2 * Math.PI * Math.Sqrt(semiMajorAxis * semiMajorAxis * semiMajorAxis / mu);
        }

        public static Orbit KinematicsToOrbit (Ephemeris kinematics, double mu, double time, PhysicalEntity parent) {
            Double3 p = kinematics.GetTranslation();                                                 //Position
            Double3 v = kinematics.GetLinearVelocity();                                                          //Velocity
            Double3 l = Double3.Cross(p, v);                                                              //Specific Angular Momentum
            double r = p.Length();                                                                     //Radius	
            double en = (v.Length() * v.Length() / 2.0) - (mu / r);                                     //Specific Energy
            double a = -1 * (mu / (2 * en));                                                                //Semi-Major Axis
            double e = Math.Sqrt(1 - (l.Length() * l.Length() / (a * mu)));                            //Eccentricity
            double i = l.Z / l.Length();                                                                   //Inclination
            double om = Math.Atan2(l.X, -l.Y);                                                         //Right Ascension
            double lat = a * Math.Atan2(p.Z / Math.Sin(i), (p.X * Math.Cos(om)) + (p.Y * Math.Sin(om)));    //Argument of Latitude
            double f = Math.Acos(e * r / (Math.Abs(e) * Math.Abs(r)));                             //True Anomaly
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
            return lastAbsoluteEphemera.GetValue(time);
        }
        public Ephemeris OrbitToRelativeKinematics (double time) {
            return lastRelativeEphemera.GetValue(time);
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

            double m = Math.Sqrt(mu / (a * a * a)) * time;
            double ea = m;
            if (double.IsInfinity(ea)) {
                throw new Exception("From mu " + mu + ", a " + a + ", time " + time + ".");
            }
            double dea = (m + (e * Math.Sin(ea)) - ea) / (1 - (e * Math.Cos(ea)));
            while (dea > 0.00001) { //Default: 0.000001
                ea += dea;
                dea = (m + (e * Math.Sin(ea)) - ea) / (1 - (e * Math.Cos(ea)));
            }
            double r = a * (1 - (e * Math.Cos(ea)));
            double f = 2 * Math.Atan(Math.Sqrt((1 + e) / (1 - e)) * Math.Tan(ea / 2.0));
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
            Double3 x = new Double3(fCos, 0, fSin) * r;
            Double3 v = new Double3(-fSin, 0, e + fCos) * Math.Sqrt(mu / (a * (1 - (e * e))));

            double wf = orbit.GetArgumentOfPeriapsis() * Mathematik.Deg2Rad;
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

            Double3 x2 = mF2 * new Double3(x.X, x.Z, x.Y);
            x = new Double3(x2.X, x2.Z, x2.Y);
            /*double xMag = x.Length();
			x = new Double3(	xMag * (Math.Cos(om * Geometry.Deg2Rad) * Math.Cos(wf) - Math.Sin(om * Geometry.Deg2Rad) * Math.Sin(wf) * Math.Cos(i * Geometry.Deg2Rad)), 
								xMag * (Math.Sin(wf) * Math.Sin(i * Geometry.Deg2Rad)), 
								xMag * (Math.Sin(om * Geometry.Deg2Rad) * Math.Cos(wf) + Math.Cos(om * Geometry.Deg2Rad) * Math.Sin(wf) * Math.Cos(i * Geometry.Deg2Rad)));
			*/

            Double3 parentPosition = absolute ? orbit.GetParent().CalculatePositionAtTime(time) : Double3.Zero;
            return new Ephemeris(x + parentPosition, v, orbit.GetRotation(time), orbit.angularVelocity);
        }

        public Double3 CalculateRelativePositionAtTime (double time) {
            return OrbitToRelativeKinematics(time).GetTranslation();
        }

        public Double3 CalculateAbsolutePositionAtTime (double time) {
            return OrbitToAbsoluteKinematics(time).GetTranslation();
        }

        public double OrbitalPeriod () {
            return OrbitalPeriod(semimajorAxis, CalculateMu());
        }

    }
}