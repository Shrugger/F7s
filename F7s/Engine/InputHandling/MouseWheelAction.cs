using System; using F7s.Utility.Geometry.Double; using Stride.Core.Mathematics;

namespace F7s.Engine.InputHandling {
    public class MouseWheelAction : AbstractInputAction {
        public readonly Action<int> action;

        public MouseWheelAction (Action<int> action) : base(action.Method.Name) {
            this.action = action;
        }

        public override void Deregister () {
            throw new NotImplementedException();
        }

        public override void Register () {
            throw new NotImplementedException();
        }
        public void Trigger (int value) {
            action.Invoke(value);
            Triggered();
        }
    }

}
