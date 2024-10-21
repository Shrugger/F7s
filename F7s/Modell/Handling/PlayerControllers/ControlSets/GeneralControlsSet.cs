using F7s.Engine;
using F7s.Engine.InputHandling;
using Stride.Input;
using System; using F7s.Utility.Geometry.Double; using Stride.Core.Mathematics;

namespace F7s.Modell.Handling.PlayerControllers.ControlSets
{
    public class GeneralControlsSet : InputSet {
        public GeneralControlsSet () {
            Add(new InputVectorAction(Zeit.TogglePause, new KeyButtonInput(Keys.P, ButtonInput.ButtonStates.Pressed)));
            //  Add(new InputVectorAction(Main.Quit, Keys.Escape)); // TODO: Restore quit functionality.

            this.Activate();
        }

        protected override void OnDeactivation () {
            base.OnDeactivation();
            throw new Exception("NO! This must remain active.");
        }
    }
}