﻿using F7s.Engine;
using F7s.Engine.InputHandling;
using F7s.Utility;
using F7s.Utility.Geometry.Double;
using Stride.Input;

namespace F7s.Modell.Handling.PlayerControllers.ControlSets {
    public class FirstPersonPlayerControlsSet : InputSet {
        public FirstPersonPlayerControlsSet () {

            Add(new InstantaneousInputAction(Player.ActivateUiControls, Keys.Tab));

            {
                void MoveLeft () {
                    Player.Translate(-MM.RightD * (float) Zeit.DeltaTimeSeconds() * Player.PanSpeed);
                }
                void MoveRight () {
                    Player.Translate(MM.RightD * (float) Zeit.DeltaTimeSeconds() * Player.PanSpeed);
                }
                void MoveForward () {
                    Player.Translate(MM.BackwardD * (float) Zeit.DeltaTimeSeconds() * Player.PanSpeed);
                }
                void MoveBackward () {
                    Player.Translate(MM.ForwardD * (float) Zeit.DeltaTimeSeconds() * Player.PanSpeed);
                }
                void MoveUp () {
                    Player.Translate(MM.UpD * (float) Zeit.DeltaTimeSeconds() * Player.PanSpeed);
                }
                void MoveDown () {
                    Player.Translate(MM.DownD * (float) Zeit.DeltaTimeSeconds() * Player.PanSpeed);
                }

                Add(new PhysicalInputVectorAction(MoveLeft, Keys.A));
                Add(new PhysicalInputVectorAction(MoveRight, Keys.D));
                Add(new PhysicalInputVectorAction(MoveForward, Keys.W));
                Add(new PhysicalInputVectorAction(MoveBackward, Keys.S));
                Add(new PhysicalInputVectorAction(MoveUp, Keys.Space));
                Add(new PhysicalInputVectorAction(MoveDown, Keys.LeftAlt));
                Add(new InstantaneousInputAction(Player.SpeedUp, Keys.Up));
                Add(new InstantaneousInputAction(Player.SpeedDown, Keys.Down));
            }

            {
                Add(new MouseDeltaAction(Player.RotatePlayer));
            }

            Add(new ComplexInputAction(() => { Player.SetTransform(MatrixD.Identity); }, new KeyButtonInput(Keys.R, ButtonInput.ButtonStates.Pressed), new KeyButtonInput(Keys.LeftCtrl, ButtonInput.ButtonStates.HeldDown)));
        }
        protected override void OnActivation () {
            base.OnActivation();
            InputHandler.SetMouseInvisible();
            InputHandler.LockMousePosition();

        }
    }
}