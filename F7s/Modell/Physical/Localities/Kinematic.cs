using F7s.Utility.Geometry;
using Stride.Core.Mathematics;

namespace F7s.Modell.Physical.Localities {
    public class Kinematic : Fixed {
        private Vector3 velocity;
        public Kinematic (PhysicalEntity entity, MatrixD transform, Locality anchor = null, Vector3? velocity = null) : base(entity, transform, anchor) {
            this.velocity = velocity ?? Vector3.Zero;
        }


        public override void Update (double deltaTime) {
            base.Update(deltaTime);
            if (this.velocity.LengthSquared() > 0) {
                float deltaTimeFloat = (float) deltaTime;
                Vector3 translation = this.velocity * deltaTimeFloat;
                MatrixD translated = this.Transform.Translated(translation);
                this.SetTransform(translated);
                this.Validate(); // TODO: Remove after debugging.
            }
        }

        public override Vector3 GetLocalVelocity () {
            return this.velocity;
        }
        public override void AddLocalVelocity (Vector3 additionalVelocity) {
            this.velocity += additionalVelocity;
        }
        public override void SetLocalVelocity (Vector3 value) {
            this.velocity = value;
        }
    }
}
