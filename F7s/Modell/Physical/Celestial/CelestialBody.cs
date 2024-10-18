using F7s.Modell.Handling;
using F7s.Modell.Physical.Bodies;
using F7s.Utility;
using Stride.Core.Mathematics;
using System.Collections.Generic;

namespace F7s.Modell.Physical.Celestial
{


    public class CelestialBody : Body {

        public static List<CelestialBody> AllCelestialBodies { get; } = new List<CelestialBody>();

        public readonly double radius;

        public override void ConfigureInfoblock (Infoblock infoblock) {
            base.ConfigureInfoblock(infoblock);
        }


        public CelestialBody (string name, double radius, Farbe color) : base(name, Vector3.One * (float) radius, color) {
            this.radius = radius;
            AllCelestialBodies.Add(this);
        }

        public override void Delete () {
            base.Delete();
            AllCelestialBodies.Remove(this);
        }

        public override float BoundingRadius () {
            return (float) this.radius;
        }

        protected override Farbe Color () {
            return this.color;
        }

        protected override Quantity CalculateFallbackQuantity () {
            return new Quantity(Geom.VolumeFromSphereRadius(this.BoundingRadius()), this.FallbackDensity());
        }

        protected virtual float FallbackDensity () {
            return 1.0f;
        }

    }
}
