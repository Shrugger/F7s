using Stride.Input;
using System; using F7s.Utility.Geometry.Double; using Stride.Core.Mathematics;
using System.Collections.Generic;
using System.Linq;

namespace F7s.Engine.InputHandling {

    public class InputVectorAction : AbstractInputAction {

        public readonly Action Action;

        private List<ButtonInput> buttons;

        public InputVectorAction (Action action, Keys key) : this(action, new KeyButtonInput(key, ButtonInput.ButtonStates.Pressed)) { }
        public InputVectorAction (Action action, params ButtonInput[] buttons) : base(action.Method.Name) {
            this.Action = action;
            this.buttons = buttons.ToList();
        }

        public bool Match () {
            return this.buttons.All(b => b.Matched());
        }

        public static bool operator == (InputVectorAction left, InputVectorAction right) {
            return EqualityComparer<InputVectorAction>.Default.Equals(left, right);
        }

        public static bool operator != (InputVectorAction left, InputVectorAction right) {
            return !(left == right);
        }

        public void Trigger () {
            Action.Invoke();
            Triggered();
        }

        public void TriggerIfMatch () {
            if (Match()) {
                Trigger();
            }
        }

        public override bool Equals (object obj) {
            return obj is InputVectorAction other &&
                other.Action.Method == this.Action.Method &&
                other.Action.Target == this.Action.Target &&
                other.buttons.All(b => other.buttons.Contains(b));
        }

        public override int GetHashCode () {
            HashCode hash = new HashCode();
            hash.Add(buttons);
            hash.Add(Action);
            return hash.ToHashCode();
        }

        public override void Register () {
            InputHandler.RegisterEventAction(this);
        }

        public override void Deregister () {
            InputHandler.DeregisterEventAction(this);
        }
    }

}
