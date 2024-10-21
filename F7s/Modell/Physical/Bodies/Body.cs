using F7s.Utility;
using Stride.Core.Mathematics;

namespace F7s.Modell.Physical.Bodies {
    public class Body : PhysicalEntity {

        public Double3 scale { get; protected set; }
        public Farbe color { get; private set; }

        public Body (string name, Double3 scale, Farbe color) : base(name) {
            this.scale = scale;
            SetColor(color);
        }

        protected void SetColor (Farbe color) {
            this.color = color;
        }
        protected virtual Farbe Color () {
            return color;
        }

        public override Farbe? UiColor () {
            return color;
        }

        public override Double3 Scale () {
            return scale;
        }

        public override double BoundingRadius () {
            return scale.Length();
        }
    }

}
