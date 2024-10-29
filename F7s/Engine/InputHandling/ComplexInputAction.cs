using System;
using System.Collections.Generic;
using System.Linq;

namespace F7s.Engine.InputHandling {

    public class ComplexInputAction : AbstractInputAction {

        public readonly Action Action;
        private readonly List<ButtonInput> buttons;

        public ComplexInputAction (Action action, params ButtonInput[] buttons) : base(action.Method.Name) {
            Action = action;
            this.buttons = buttons.ToList();
        }

        public override string ToString () {
            return buttons.Aggregate("", (b, s) => s + " + " + b) + " -> " + Action.Target + "." + Action.Method;
        }

        public bool Match () {
            return buttons.All(b => b.Matched());
        }

        public static bool operator == (ComplexInputAction left, ComplexInputAction right) {
            return EqualityComparer<ComplexInputAction>.Default.Equals(left, right);
        }

        public static bool operator != (ComplexInputAction left, ComplexInputAction right) {
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
            return obj is ComplexInputAction other &&
                other.Action.Method == Action.Method &&
                other.Action.Target == Action.Target &&
                other.buttons.All(b => other.buttons.Contains(b));
        }

        public override int GetHashCode () {
            HashCode hash = new HashCode();
            hash.Add(buttons);
            hash.Add(Action);
            return hash.ToHashCode();
        }

        public override void Register () {
            if (CanBeHeldDown()) {
                InputHandler.RegisterKeyHoldActionOnUpdate(this);
            } else {
                InputHandler.RegisterEventAction(this);
            }
        }

        public override void Deregister () {
            if (CanBeHeldDown()) {
                InputHandler.DeregisterKeyHoldActionOnUpdate(this);
            } else {
                InputHandler.DeregisterEventAction(this);
            }
        }

        public bool CanBeHeldDown () {
            return buttons.Any(b => b.State == ButtonInput.ButtonStates.HeldDown);
        }
    }

}
