using F7s.Engine.InputHandling;
using Stride.Core.Mathematics;

namespace F7s.Modell.Handling.PlayerControllers.ControlSets {
    public class FirstPersonPlayerControlsSet : InputSet {
        public FirstPersonPlayerControlsSet () {

            this.Add(new InputVectorAction(Player.ActivateUiControls, Key.Tab));

            {
                void MoveLeft (double delta) {
                    Player.Translate(-Vector3.UnitX * (float) delta * Player.PanSpeed);
                }
                void MoveRight (double delta) {
                    Player.Translate(Vector3.UnitX * (float) delta * Player.PanSpeed);
                }
                void MoveForward (double delta) {
                    Player.Translate(Vector3.UnitZ * (float) delta * Player.PanSpeed);
                }
                void MoveBackward (double delta) {
                    Player.Translate(-Vector3.UnitZ * (float) delta * Player.PanSpeed);
                }
                void MoveUp (double delta) {
                    Player.Translate(Vector3.UnitY * (float) delta * Player.PanSpeed);
                }
                void MoveDown (double delta) {
                    Player.Translate(-Vector3.UnitY * (float) delta * Player.PanSpeed);
                }

                Add(new KeyHoldAction(MoveLeft, KeyHoldAction.ProcessTypes.Process, Key.A, shift: false));
                Add(new KeyHoldAction(MoveRight, KeyHoldAction.ProcessTypes.Process, Key.D, shift: false));
                Add(new KeyHoldAction(MoveForward, KeyHoldAction.ProcessTypes.Process, Key.W, shift: false));
                Add(new KeyHoldAction(MoveBackward, KeyHoldAction.ProcessTypes.Process, Key.S, shift: false));
                Add(new KeyHoldAction(MoveUp, KeyHoldAction.ProcessTypes.Process, Key.Space, shift: false));
                Add(new KeyHoldAction(MoveDown, KeyHoldAction.ProcessTypes.Process, Key.Alt, shift: false));
                Add(new InputVectorAction(Player.SpeedUp, Key.Up, shift: false));
                Add(new InputVectorAction(Player.SpeedDown, Key.Down, shift: false));
            }

            {
                InputHandler.SetMouseMode(Input.MouseModeEnum.Captured);
                Add(new MouseDeltaAction(Player.RotatePlayer));
            }

            Add(new InputVectorAction(() => { Player.SetTransform(Transform3D.Identity); }, Key.R, control: true));
        }
        protected override void OnActivation () {
            base.OnActivation();
            InputHandler.SetMouseMode(Input.MouseModeEnum.Captured);

        }
    }
}