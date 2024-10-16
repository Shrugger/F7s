using F7s.Utility;

namespace F7s.Modell.Physical.Celestial {
    public class Star : CelestialBody {

        public Star (string name, double radius, Farbe color) : base(name, radius, color) {
        }

        protected override void Update (double deltaTime) {
            base.Update(deltaTime);
        }

        protected override float FallbackDensity () {
            return 1.5f;
        }
    }
}
