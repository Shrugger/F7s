using F7s.Utility.Geometry.Double;
using Stride.Core.Mathematics;

namespace F7s.Modell.Physical.Localities {
    public class Kinematic : Fixed {
        private Double3 velocity;
        public Kinematic (PhysicalEntity entity, MatrixD transform, Locality anchor = null, Double3? velocity = null) : base(entity, transform, anchor) {
            this.velocity = velocity ?? Double3.Zero;
        }


        public override void Update (double deltaTime) {
            base.Update(deltaTime);
            if (velocity.LengthSquared() > 0) {
                float deltaTimeFloat = (float) deltaTime;
                Double3 translation = velocity * deltaTimeFloat;
                MatrixD translated = MatrixD.Translation(translation) * Transform;
                SetTransform(translated);
                Validate(); // TODO: Remove after debugging.
            }
        }

        public override Double3 GetLocalVelocity () {
            return velocity;
        }
        public override void AddLocalVelocity (Double3 additionalVelocity) {
            velocity += additionalVelocity;
        }
        public override void SetLocalVelocity (Double3 value) {
            velocity = value;
        }
    }
}
