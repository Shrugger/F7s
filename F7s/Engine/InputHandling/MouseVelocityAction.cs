using Stride.Core.Mathematics; using F7s.Utility.Geometry.Double;
using System; using F7s.Utility.Geometry.Double;

namespace F7s.Engine.InputHandling {
    public class MouseVelocityAction : AbstractInputAction {
        public readonly Action<Vector2> action;
        public MouseVelocityAction (Action<Vector2> action) : base(action.Method.Name) {
            this.action = action;
        }

        public override void Deregister () {
            InputHandler.DeregisterMouseVelocityAction(this);
        }

        public override void Register () {
            InputHandler.RegisterMouseVelocityAction(this);
        }
        public void Trigger (Vector2 value) {
            action.Invoke(value);
            Triggered();
        }
    }

}
