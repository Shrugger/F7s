using System;
namespace F7s.Engine.InputHandling {
    public class MouseWheelAction : AbstractInputAction {
        public readonly Action<float> action;

        public MouseWheelAction (Action<float> action) : base(action.Method.Name) {
            this.action = action;
        }

        public override void Deregister () {
            InputHandler.DeregisterMouseWheelAction(this);
        }

        public override void Register () {
            InputHandler.RegisterMouseWheelAction(this);
        }
        public void Trigger () {
            float value = InputHandler.GetMouseWheelDelta();
            action.Invoke(value);
            Triggered();
        }
    }

}
