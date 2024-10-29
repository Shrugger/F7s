using F7s.Utility;
using F7s.Utility.Geometry.Double;
using Stride.Core.Mathematics;
using System;
using System.Collections.Generic;

namespace F7s.Modell.Physical.Localities {

    public class Orbiting : Locality {
        public readonly Orbit orbit;

        public const double rotationSpeed = 0.1;

        public static double OrbitSpeedMultiplier = 1;

        public Orbiting (PhysicalEntity orbiter, double semimajorAxis, PhysicalEntity orbitee) : base(orbiter, orbitee) {
            orbit = new Orbit(0, 0, 0, 0, semimajorAxis, Alea.AngleDouble(), orbitee, angularVelocity: Double3.UnitY * rotationSpeed / OrbitSpeedMultiplier);
            orbit.SetOrbiter(orbiter);
        }

        public override Visualizabilities Visualizability () {
            return Visualizabilities.Cyclical;
        }

        public override List<Locality> PredictedItinerary (int legs, Locality parent, double? duration = null) {
            if (!duration.HasValue) {
                duration = orbit.OrbitalPeriod();
            }
            double legLength = duration.Value / legs;
            List<Locality> itinerary = new List<Locality>();

            double currentDate = GetTime();
            for (int i = 0; i < legs; i++) {
                double date = currentDate + (legLength * i);
                Ephemeris ephemeris = orbit.OrbitToRelativeKinematics(date);
                Locality locality = new Fixed(
                    null,
                    parent
,
                    MatrixD.Transformation(ephemeris.GetTranslation(), ephemeris.GetRotation()));
                itinerary.Add(locality);
            }

            return itinerary;
        }

        public override double GetTime () {
            return base.GetTime() * OrbitSpeedMultiplier;
        }

        public override MatrixD GetLocalTransform () {
            double time = GetTime();
            Ephemeris ephemeris = orbit.OrbitToRelativeKinematics(time);
            return MatrixD.Transformation(ephemeris.GetTranslation(), ephemeris.GetRotation());
        }

        public override Locality HierarchySuperior () {
            return orbit?.GetParent();
        }

        public override string ToString () {
            return base.ToString() + " around " + orbit.GetParent();
        }
        public override bool InheritsRotation () {
            return false;
        }

        public override void Rotate (double yaw, double pitch, double roll = 0) {
            orbit.rotationOffset += new Double3(pitch, yaw, roll);
        }

        public override void Translate (Double3 relativeOffset) {
            throw new System.Exception("Translation is not applicable to orbits.");
        }

        public override void RotateEcliptic (double yaw, double pitch, double roll = 0) {
            Rotate(yaw, pitch);
        }
        protected override void ReplaceSuperior (Locality replacement) {
            throw new Exception("Does this even make sense?");
        }
    }
}
