using F7s.Engine;
using F7s.Engine.InputHandling;
using F7s.Utility.Geometry.Double;
using Stride.Core.Mathematics;
using Stride.Input;
namespace F7s.Modell.Handling.PlayerControllers.ControlSets {
    public class FreeCameraControlsSet : InputSet {
        public FreeCameraControlsSet () {
            Add(new ComplexInputAction(Player.ActivateUiControls, new KeyButtonInput(Keys.Tab, ButtonInput.ButtonStates.Pressed)));

            float PanSpeed () {
                return (float) Zeit.DeltaTimeSeconds() * Player.PanSpeed;
            }

            {
                void MoveLeft () {
                    Kamera.Translate(-Double3.UnitX * PanSpeed());
                }
                void MoveRight () {
                    Kamera.Translate(Double3.UnitX * PanSpeed());
                }
                void MoveForward () {
                    Kamera.Translate(Double3.UnitZ * PanSpeed());
                }
                void MoveBackward () {
                    Kamera.Translate(-Double3.UnitZ * PanSpeed());
                }
                void MoveUp () {
                    Kamera.Translate(Double3.UnitY * PanSpeed());
                }
                void MoveDown () {
                    Kamera.Translate(-Double3.UnitY * PanSpeed());
                }
                const float RollSensitivity = 50;
                void RollLeft () {
                    Kamera.RotateEcliptic(0, 0, (float) -Zeit.DeltaTimeSeconds() * RollSensitivity);
                }
                void RollRight () {
                    Kamera.RotateEcliptic(0, 0, (float) Zeit.DeltaTimeSeconds() * RollSensitivity);
                }

                Add(new ContinuousInputAction(MoveLeft, Keys.A));
                Add(new ContinuousInputAction(MoveRight, Keys.D));
                Add(new ContinuousInputAction(MoveForward, Keys.W));
                Add(new ContinuousInputAction(MoveBackward, Keys.S));
                Add(new ContinuousInputAction(MoveUp, Keys.Space));
                Add(new ContinuousInputAction(MoveDown, Keys.LeftAlt));
                Add(new ContinuousInputAction(RollLeft, Keys.Q));
                Add(new ContinuousInputAction(RollRight, Keys.E));
                Add(new InstantaneousInputAction(Player.SpeedUp, Keys.Up));
                Add(new InstantaneousInputAction(Player.SpeedDown, Keys.Down));

                void AlterGlobalScaleFactor (float multiplier) {
                    float oldScaleFactor = RepresentationSettings.GlobalScaleFactor;
                    RepresentationSettings.SetGlobalScaleFactor(oldScaleFactor * multiplier);
                    System.Diagnostics.Debug.WriteLine("Global Scale Factor " + oldScaleFactor + " * " + multiplier + " => " + RepresentationSettings.GlobalScaleFactor + ".");
                }

                Add(
                    new MouseWheelAction((float v) => {
                        if (v > 0) {
                            AlterGlobalScaleFactor(2.0f);
                        } else if (v < 0) {
                            AlterGlobalScaleFactor(0.5f);
                        }
                    }
                    )
                );
            }

            {
                Add(new MouseDeltaAction(Player.RotateCamera));
            }

            Add(new ComplexInputAction(() => { Kamera.SetTransform(MatrixD.Identity); }, new KeyButtonInput(Keys.R, ButtonInput.ButtonStates.Pressed), new KeyButtonInput(Keys.LeftCtrl, ButtonInput.ButtonStates.HeldDown)));
        }

        protected override void OnActivation () {
            base.OnActivation();
            InputHandler.SetMouseInvisible();
            InputHandler.LockMousePosition();

        }
    }
}