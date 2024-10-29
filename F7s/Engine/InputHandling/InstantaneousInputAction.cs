using Stride.Input;
using System;

namespace F7s.Engine.InputHandling {
    public class InstantaneousInputAction : ComplexInputAction {

        public InstantaneousInputAction (Action action, Keys key) : base(action, new KeyButtonInput(key, ButtonInput.ButtonStates.Pressed)) { }
    }

}
