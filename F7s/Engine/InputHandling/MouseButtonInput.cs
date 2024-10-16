using Stride.Input;
using System;

namespace F7s.Engine.InputHandling {
    public class MouseButtonInput : ButtonInput {
        public readonly MouseButton MouseButton;
        public override bool Matched () {
            switch (this.State) {
                case ButtonStates.Pressed:
                    return InputHandler.MouseButtonPressed(this.MouseButton);
                case ButtonStates.HeldDown:
                    return InputHandler.MouseButtonHeld(this.MouseButton);
                case ButtonStates.Released:
                    return InputHandler.MouseButtonReleased(this.MouseButton);
                default:
                    throw new NotImplementedException();
            }
        }
    }

}
