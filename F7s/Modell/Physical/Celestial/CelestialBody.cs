using F7s.Modell.Handling;
using F7s.Modell.Physical.Bodies;
using F7s.Utility;
using Stride.Core.Mathematics;
using System.Collections.Generic;

namespace F7s.Modell.Physical.Celestial {


    public class CelestialBody : Body {

        public static List<CelestialBody> AllCelestialBodies { get; } = new List<CelestialBody>();

        public readonly double radius;

        public override void ConfigureInfoblock (Infoblock infoblock) {
            base.ConfigureInfoblock(infoblock);
        }


        public CelestialBody (string name, double radius, Farbe color) : base(name, Double3.One * radius, color) {
            this.radius = radius;
            AllCelestialBodies.Add(this);
        }

        public override void Delete () {
            base.Delete();
            AllCelestialBodies.Remove(this);
        }

        public override double BoundingRadius () {
            return radius;
        }

        protected override Farbe Color () {
            return color;
        }

        protected override Quantity CalculateFallbackQuantity () {
            return new Quantity(MM.VolumeFromSphereRadius(BoundingRadius()), FallbackDensity());
        }

        protected virtual float FallbackDensity () {
            return 1.0f;
        }

    }
}
