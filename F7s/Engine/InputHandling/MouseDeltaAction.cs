
using Stride.Core.Mathematics;
using System;

namespace F7s.Engine.InputHandling {
    public class MouseDeltaAction : AbstractInputAction {
        public readonly Action<Vector2> action;
        public MouseDeltaAction (Action<Vector2> action) : base(action.Method.Name) {
            this.action = action;
        }

        public override void Deregister () {
            InputHandler.DeregisterMouseDeltaAction(this);
        }

        public override void Register () {
            InputHandler.RegisterMouseDeltaAction(this);
        }
        public void Trigger (Vector2 value) {
            action.Invoke(value);
            Triggered();
        }
    }

}
