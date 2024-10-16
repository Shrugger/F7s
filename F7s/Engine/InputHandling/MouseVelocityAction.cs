using System;
using Stride.Core.Mathematics;

namespace F7s.Engine.InputHandling {
    public class MouseVelocityAction : MousePositionAction {
        public MouseVelocityAction (Action<Vector2> action) : base(action) {
        }

        public override void Deregister () {
            InputHandler.DeregisterMouseVelocityAction(this);
        }

        public override void Register () {
            InputHandler.RegisterMouseVelocityAction(this);
        }
    }

}
