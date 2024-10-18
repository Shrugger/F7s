using F7s.Utility;
using F7s.Geometry;
using Stride.Core.Mathematics;
using System;
using System.Collections.Generic;

namespace F7s.Modell.Physical.Localities {

    public class Orbiting : Locality {
        public readonly Orbit orbit;

        public const float rotationSpeed = 0.1f;

        public static float OrbitSpeedMultiplier = 1;

        public Orbiting (PhysicalEntity orbiter, double semimajorAxis, PhysicalEntity orbitee) : base(orbiter, orbitee) {
            this.orbit = new Orbit(0, 0, 0, 0, semimajorAxis, Alea.AngleDouble(), orbitee, angularVelocity: Vector3.UnitY * rotationSpeed / OrbitSpeedMultiplier);
            this.orbit.SetOrbiter(orbiter);
        }

        public override Visualizabilities Visualizability () {
            return Visualizabilities.Cyclical;
        }

        public override List<Locality> PredictedItinerary (int legs, Locality parent, double? duration = null) {
            if (!duration.HasValue) {
                duration = this.orbit.OrbitalPeriod();
            }
            double legLength = duration.Value / legs;
            List<Locality> itinerary = new List<Locality>();

            double currentDate = GetTime();
            for (int i = 0; i < legs; i++) {
                double date = currentDate + (legLength * i);
                Ephemeris ephemeris = this.orbit.OrbitToRelativeKinematics(date);
                Locality locality = new Fixed(
                    null,
                    new Transform3D(new Basis(ephemeris.GetRotation()), ephemeris.GetTranslation().ToVector3()),
                    parent
                    );
                itinerary.Add(locality);
            }

            return itinerary;
        }

        public override double GetTime () {
            return base.GetTime() * OrbitSpeedMultiplier;
        }

        public override Transform3D GetLocalTransform () {
            double time = GetTime();
            Ephemeris ephemeris = this.orbit.OrbitToRelativeKinematics(time);
            return new Transform3D(new Basis(ephemeris.GetRotation()), ephemeris.GetTranslation().ToVector3());
        }

        public override Locality HierarchySuperior () {
            return this.orbit?.GetParent();
        }

        public override string ToString () {
            return base.ToString() + " around " + this.orbit.GetParent();
        }
        public override bool InheritsRotation () {
            return false;
        }

        public override void Rotate (float yaw, float pitch, float roll = 0) {
            this.orbit.rotationOffset += new Vector3(pitch, yaw, roll);
        }

        public override void Translate (Vector3 relativeOffset) {
            throw new System.Exception("Translation is not applicable to orbits.");
        }

        public override void RotateEcliptic (float yaw, float pitch, float roll = 0) {
            this.Rotate(yaw, pitch);
        }
        protected override void ReplaceSuperior (Locality replacement) {
            throw new Exception("Does this even make sense?");
        }
    }
}
