using Stride.Input;
using System;

namespace F7s.Engine.InputHandling {

    public class ContinuousInputAction : ComplexInputAction {

        public ContinuousInputAction (Action action, params Keys[] keys) : base(action, ConvertKeysToButtonInputs(keys)) { }

        protected static ButtonInput[] ConvertKeysToButtonInputs (Keys[] keys) {
            ButtonInput[] buttons = new ButtonInput[keys.Length];
            for (int i = 0; i < keys.Length; i++) {
                buttons[i] = new KeyButtonInput(keys[i], ButtonInput.ButtonStates.HeldDown);
            }
            return buttons;
        }

    }

    public class PhysicalInputVectorAction : ContinuousInputAction {

        public PhysicalInputVectorAction (Action action, params Keys[] keys) : base(action, keys) { }

        public override void Register () {
            if (!CanBeHeldDown()) {
                throw new Exception("Doesn't make sense for an instantaneous action to be handled during physics updates.");
            }
            InputHandler.RegisterKeyHoldActionBeforePhysicsUpdate(this);
        }

        public override void Deregister () {
            InputHandler.DeregisterKeyHoldActionBeforePhysicsUpdate(this);
        }
    }

}
