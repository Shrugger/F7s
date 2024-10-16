using Stride.Core.Mathematics;
using System;

namespace F7s.Engine.InputHandling {

    public abstract class MousePositionAction : AbstractInputAction {
        public readonly Action<Vector2> action;
        public MousePositionAction (Action<Vector2> action) : base(action.Method.Name) {

            this.action = action;
        }

        public void Trigger (Vector2 value) {
            action.Invoke(value);
            Triggered();
        }
    }

}
