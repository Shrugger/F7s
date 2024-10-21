using F7s.Utility;
using F7s.Utility.Geometry.Double;

namespace F7s.Modell.Physical.Bodies {
    public class Body : PhysicalEntity {

        public Vector3d scale { get; protected set; }
        public Farbe color { get; private set; }

        public Body (string name, Vector3d scale, Farbe color) : base(name) {
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

        public override Vector3d Scale () {
            return scale;
        }

        public override double BoundingRadius () {
            return scale.Length();
        }
    }

}
