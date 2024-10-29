using F7s.Engine.InputHandling;
using Stride.Input;


namespace F7s.Modell.Handling.PlayerControllers.ControlSets {
    public class UiControlsSet : InputSet {
        public UiControlsSet () {
            Add(new InstantaneousInputAction(Player.DeactivateUiControls, Keys.Tab));
        }
        protected override void OnActivation () {
            base.OnActivation();
            InputHandler.SetMouseVisible();
        }
    }
}