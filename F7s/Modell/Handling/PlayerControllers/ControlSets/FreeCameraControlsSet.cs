﻿using F7s.Engine;
using F7s.Engine.InputHandling;
using F7s.Geometry;
using Stride.Core.Mathematics;
using Stride.Input;
using System;

namespace F7s.Modell.Handling.PlayerControllers.ControlSets {
    public class FreeCameraControlsSet : InputSet {
        public FreeCameraControlsSet () {
            this.Add(new InputVectorAction(Player.ActivateUiControls, new KeyButtonInput(Keys.Tab, ButtonInput.ButtonStates.Pressed)));

            {
                void MoveLeft () {
                    Kamera.Translate(-Vector3.UnitX * (float) Zeit.DeltaTimeSeconds() * Player.PanSpeed);
                }
                void MoveRight () {
                    Kamera.Translate(Vector3.UnitX * (float) Zeit.DeltaTimeSeconds() * Player.PanSpeed);
                }
                void MoveForward () {
                    Kamera.Translate(Vector3.UnitZ * (float) Zeit.DeltaTimeSeconds() * Player.PanSpeed);
                }
                void MoveBackward () {
                    Kamera.Translate(-Vector3.UnitZ * (float) Zeit.DeltaTimeSeconds() * Player.PanSpeed);
                }
                void MoveUp () {
                    Kamera.Translate(Vector3.UnitY * (float) Zeit.DeltaTimeSeconds() * Player.PanSpeed);
                }
                void MoveDown () {
                    Kamera.Translate(-Vector3.UnitY * (float) Zeit.DeltaTimeSeconds() * Player.PanSpeed);
                }
                const float RollSensitivity = 50;
                void RollLeft () {
                    Kamera.RotateEcliptic(0, 0, (float) -Zeit.DeltaTimeSeconds() * RollSensitivity);
                }
                void RollRight () {
                    Kamera.RotateEcliptic(0, 0, (float) Zeit.DeltaTimeSeconds() * RollSensitivity);
                }

                Add(new InputVectorAction(MoveLeft, Keys.A));
                Add(new InputVectorAction(MoveRight, Keys.D));
                Add(new InputVectorAction(MoveForward, Keys.W));
                Add(new InputVectorAction(MoveBackward, Keys.S));
                Add(new InputVectorAction(MoveUp, Keys.Space));
                Add(new InputVectorAction(MoveDown, Keys.LeftAlt));
                Add(new InputVectorAction(RollLeft, Keys.Q));
                Add(new InputVectorAction(RollRight, Keys.E));
                Add(new InputVectorAction(Player.SpeedUp, Keys.Up));
                Add(new InputVectorAction(Player.SpeedDown, Keys.Down));

                void AlterGlobalScaleFactor (float multiplier) {
                    float oldScaleFactor = RepresentationSettings.GlobalScaleFactor;
                    RepresentationSettings.SetGlobalScaleFactor(oldScaleFactor * multiplier);
                    Console.WriteLine("Global Scale Factor " + oldScaleFactor + " * " + multiplier + " => " + RepresentationSettings.GlobalScaleFactor + ".");
                }

                Add(
                    new MouseWheelAction((int v) => {
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

            Add(new InputVectorAction(() => { Kamera.SetTransform(Transform3D.Identity); }, new KeyButtonInput(Keys.R, ButtonInput.ButtonStates.Pressed), new KeyButtonInput(Keys.LeftCtrl, ButtonInput.ButtonStates.HeldDown)));
        }

        protected override void OnActivation () {
            base.OnActivation();
            InputHandler.SetMouseInvisible();
            InputHandler.LockMousePosition();

        }
    }
}