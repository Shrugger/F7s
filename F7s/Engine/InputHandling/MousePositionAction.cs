using Stride.Core.Mathematics; using F7s.Utility.Geometry.Double; using Stride.Core.Mathematics;
using System; using F7s.Utility.Geometry.Double; using Stride.Core.Mathematics;

namespace F7s.Engine.InputHandling {

    public class MousePositionAction : AbstractInputAction {
        public readonly Action<Vector2> action;
        public MousePositionAction (Action<Vector2> action) : base(action.Method.Name) {

            this.action = action;
        }

        public override void Deregister () {
            throw new NotImplementedException();
        }

        public override void Register () {
            throw new NotImplementedException();
        }

        public void Trigger (Vector2 value) {
            action.Invoke(value);
            Triggered();
        }
    }

}
