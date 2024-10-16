using F7s.Utility;
using Stride.Core.Mathematics;

namespace F7s.Modell.Physical.Bodies {
    public class Body : PhysicalEntity {

        public Vector3 scale { get; protected set; }
        public Farbe color { get; private set; }

        public Body (string name, Vector3 scale, Farbe color) : base(name) {
            this.scale = scale;
            this.SetColor(color);
        }

        protected void SetColor (Farbe color) {
            this.color = color;
        }
        protected virtual Farbe Color () {
            return this.color;
        }

        public override Farbe? UiColor () {
            return this.color;
        }

        public override Vector3 Scale () {
            return this.scale;
        }

        public override float BoundingRadius () {
            return this.scale.Length();
        }
    }

}
