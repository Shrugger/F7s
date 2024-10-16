
using Stride.Core.Mathematics;
using System;

namespace F7s.Engine.InputHandling {
    public class MouseDeltaAction : MousePositionAction {
        public MouseDeltaAction (Action<Vector2> action) : base(action) {
        }

        public override void Deregister () {
            InputHandler.DeregisterMouseDeltaAction(this);
        }

        public override void Register () {
            InputHandler.RegisterMouseDeltaAction(this);
        }
    }

}
