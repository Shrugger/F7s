using F7s.Utility.Geometry.Double;

namespace F7s.Modell.Physical.Localities {
    public class Kinematic : Fixed {
        private Vector3d velocity;
        public Kinematic (PhysicalEntity entity, MatrixD transform, Locality anchor = null, Vector3d? velocity = null) : base(entity, transform, anchor) {
            this.velocity = velocity ?? Vector3d.Zero;
        }


        public override void Update (double deltaTime) {
            base.Update(deltaTime);
            if (velocity.LengthSquared() > 0) {
                float deltaTimeFloat = (float) deltaTime;
                Vector3d translation = velocity * deltaTimeFloat;
                MatrixD translated = MatrixD.Translation(translation) * Transform;
                SetTransform(translated);
                Validate(); // TODO: Remove after debugging.
            }
        }

        public override Vector3d GetLocalVelocity () {
            return velocity;
        }
        public override void AddLocalVelocity (Vector3d additionalVelocity) {
            velocity += additionalVelocity;
        }
        public override void SetLocalVelocity (Vector3d value) {
            velocity = value;
        }
    }
}
