using F7s.Utility;
using Stride.Engine;
using Stride.Physics;
using System; using F7s.Utility.Geometry.Double; using Stride.Core.Mathematics;

namespace F7s {
    public class JumpySphere : SyncScript {
        // Declared public member fields and properties will show in the game studio

        private double cooldown = 0;

        public override void Start () {

        }

        public override void Update () {
            double delta = (float) Game.UpdateTime.Elapsed.TotalSeconds;
            cooldown -= delta;
            if (cooldown < 0) {
                cooldown = 1 + (Alea.Float() * 4);
                Jump();
            }
        }

        private void Jump () {
            // Jump!
            Entity.Get<RigidbodyComponent>().ApplyImpulse(Alea.Vector3() * 10);
            System.Diagnostics.Debug.WriteLine("Jump!");
        }
    }
}
