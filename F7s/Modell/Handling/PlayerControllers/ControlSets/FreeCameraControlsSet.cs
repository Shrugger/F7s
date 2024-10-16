using F7s.Engine.InputHandling;
using Stride.Core.Mathematics;
using Stride.Input;
using System;

namespace F7s.Modell.Handling.PlayerControllers.ControlSets {
    public class FreeCameraControlsSet : InputSet {
        public FreeCameraControlsSet () {
            this.Add(new InputVectorAction(Player.ActivateUiControls, new KeyButtonInput(Keys.Tab, ButtonInput.ButtonStates.Pressed)));

            {
                void MoveLeft (double delta) {
                    Kamera.Translate(-Vector3.UnitX * (float) delta * Player.PanSpeed);
                }
                void MoveRight (double delta) {
                    Kamera.Translate(Vector3.UnitX * (float) delta * Player.PanSpeed);
                }
                void MoveForward (double delta) {
                    Kamera.Translate(Vector3.UnitZ * (float) delta * Player.PanSpeed);
                }
                void MoveBackward (double delta) {
                    Kamera.Translate(-Vector3.UnitZ * (float) delta * Player.PanSpeed);
                }
                void MoveUp (double delta) {
                    Kamera.Translate(Vector3.UnitY * (float) delta * Player.PanSpeed);
                }
                void MoveDown (double delta) {
                    Kamera.Translate(-Vector3.UnitY * (float) delta * Player.PanSpeed);
                }
                const float RollSensitivity = 50;
                void RollLeft (double delta) {
                    Kamera.RotateEcliptic(0, 0, (float) -delta * RollSensitivity);
                }
                void RollRight (double delta) {
                    Kamera.RotateEcliptic(0, 0, (float) delta * RollSensitivity);
                }

                Add(new KeyHoldAction(MoveLeft, KeyHoldAction.ProcessTypes.Process, Key.A, shift: false));
                Add(new KeyHoldAction(MoveRight, KeyHoldAction.ProcessTypes.Process, Key.D, shift: false));
                Add(new KeyHoldAction(MoveForward, KeyHoldAction.ProcessTypes.Process, Key.W, shift: false));
                Add(new KeyHoldAction(MoveBackward, KeyHoldAction.ProcessTypes.Process, Key.S, shift: false));
                Add(new KeyHoldAction(MoveUp, KeyHoldAction.ProcessTypes.Process, Key.Space, shift: false));
                Add(new KeyHoldAction(MoveDown, KeyHoldAction.ProcessTypes.Process, Key.Alt, shift: false));
                Add(new KeyHoldAction(RollLeft, KeyHoldAction.ProcessTypes.Process, Key.Q, shift: false));
                Add(new KeyHoldAction(RollRight, KeyHoldAction.ProcessTypes.Process, Key.E, shift: false));
                Add(new InputVectorAction(Player.SpeedUp, Key.Up, shift: false));
                Add(new InputVectorAction(Player.SpeedDown, Key.Down, shift: false));

                void AlterGlobalScaleFactor (float multiplier) {
                    float oldScaleFactor = RepresentationSettings.GlobalScaleFactor;
                    RepresentationSettings.SetGlobalScaleFactor(oldScaleFactor * multiplier);
                    Console.WriteLine("Global Scale Factor " + oldScaleFactor + " * " + multiplier + " => " + RepresentationSettings.GlobalScaleFactor + ".");
                }

                Add(new InputVectorAction(() => AlterGlobalScaleFactor(2.0f), null, MouseButton.WheelUp));
                Add(new InputVectorAction(() => AlterGlobalScaleFactor(0.5f), null, MouseButton.WheelDown));
            }

            {
                InputHandler.SetMouseMode(Input.MouseModeEnum.Captured);
                Add(new MouseDeltaAction(Player.RotateCamera));
            }

            Add(new InputVectorAction(() => { Kamera.SetTransform(Transform3D.Identity); }, Key.R, control: true));
        }

        protected override void OnActivation () {
            base.OnActivation();
            InputHandler.SetMouseMode(Input.MouseModeEnum.Captured);

        }
    }
}