using Stride.Input;
using System; using F7s.Utility.Geometry.Double;

namespace F7s.Engine.InputHandling {
    public class KeyButtonInput : ButtonInput {
        public readonly Keys Key;

        public KeyButtonInput (Keys key, ButtonStates state) : base(state) {
            Key = key;
        }

        public override bool Matched () {
            switch (this.State) {
                case ButtonStates.Pressed:
                    return InputHandler.KeyPressed(this.Key);
                case ButtonStates.HeldDown:
                    return InputHandler.KeyHeld(this.Key);
                case ButtonStates.Released:
                    return InputHandler.KeyReleased(this.Key);
                default:
                    throw new NotImplementedException();
            }
        }
    }

}
