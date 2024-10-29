using F7s.Mains;
using F7s.Utility;
using Stride.Core.Mathematics;
using Stride.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace F7s.Engine.InputHandling {

    public class InputHandler : Frogram {

        // TODO: Make sure that all AbstractInputActions are actually registered, triggerable and handled.

        private static readonly List<ComplexInputAction> eventActions = new List<ComplexInputAction>();
        private static readonly List<ComplexInputAction> keyHoldActionsOnUpdate = new List<ComplexInputAction>();
        private static readonly List<ComplexInputAction> keyHoldActionsBeforePhysicsUpdate = new List<ComplexInputAction>();
        private static readonly List<MouseDeltaAction> onMouseMoveDelta = new List<MouseDeltaAction>();
        private static readonly List<MouseVelocityAction> onMouseMoveVelocity = new List<MouseVelocityAction>();
        private static readonly List<MouseWheelAction> mouseWheelActions = new List<MouseWheelAction>();

        public static readonly HashSet<Keys> pressedButUnreleasedKeys = new HashSet<Keys>(); // TODO: Make private.
        private static readonly HashSet<MouseButton> pressedButUnreleasedMouseButtons = new HashSet<MouseButton>();

        public InputHandler () {
            Debug.WriteLine("Registered event actions: " + eventActions.Aggregate("", (l, ae) => l + "\n" + ae));
            Debug.WriteLine("Registered hold actions: " + keyHoldActionsOnUpdate.Aggregate("", (l, ae) => l + "\n" + ae));
        }

        protected override void Update () {

            if (MainSync.InputManager.KeyEvents.Count > 0) {
                foreach (ButtonEvent e in MainSync.InputManager.Events) {
                    if (e is KeyEvent key) {
                        if (key.IsDown) {
                            pressedButUnreleasedKeys.Add(key.Key);
                        } else {
                            pressedButUnreleasedKeys.Remove(key.Key);
                        }
                    } else if (e is MouseButtonEvent mbe) {
                        if (mbe.IsDown) {
                            pressedButUnreleasedMouseButtons.Add(mbe.Button);
                        } else {
                            pressedButUnreleasedMouseButtons.Remove(mbe.Button);
                        }
                    }
                }
                eventActions.ForEach(a => a.TriggerIfMatch());
            }

            keyHoldActionsOnUpdate.ForEach(a => a.TriggerIfMatch());
            if (InputHandler.GetMouseDelta() != Vector2.Zero) {
                onMouseMoveDelta.ForEach(a => a.Trigger());
                onMouseMoveVelocity.ForEach(a => a.Trigger());
            }

            float mouseWheelDelta = InputHandler.GetMouseWheelDelta();
            if (mouseWheelDelta != 0) {
                mouseWheelActions.ForEach(a => a.Trigger(mouseWheelDelta));
            }
        }

        protected override void PrePhysicsUpdate (Stride.Physics.Simulation sender, float tick) {
            keyHoldActionsBeforePhysicsUpdate.ForEach(a => a.TriggerIfMatch());
        }

        protected override void PostPhysicsUpdate (Stride.Physics.Simulation sender, float tick) {
        }

        public static void RegisterEventAction (ComplexInputAction action) {
            eventActions.Add(action);
        }
        public static void DeregisterEventAction (ComplexInputAction action) {
            eventActions.Remove(action);
        }
        public static void RegisterKeyHoldActionOnUpdate (ComplexInputAction action) {
            keyHoldActionsOnUpdate.Add(action);
        }
        public static void DeregisterKeyHoldActionOnUpdate (ComplexInputAction action) {
            keyHoldActionsOnUpdate.Remove(action);
        }
        public static void RegisterKeyHoldActionBeforePhysicsUpdate (ComplexInputAction action) {
            keyHoldActionsBeforePhysicsUpdate.Add(action);
        }
        public static void DeregisterKeyHoldActionBeforePhysicsUpdate (ComplexInputAction action) {
            keyHoldActionsBeforePhysicsUpdate.Remove(action);
        }
        public static void RegisterMouseDeltaAction (MouseDeltaAction action) {
            onMouseMoveDelta.Add(action);
        }
        public static void DeregisterMouseDeltaAction (MouseDeltaAction action) {
            onMouseMoveDelta.Remove(action);
        }
        public static void RegisterMouseVelocityAction (MouseVelocityAction action) {
            onMouseMoveVelocity.Add(action);
        }
        public static void DeregisterMouseVelocityAction (MouseVelocityAction action) {
            onMouseMoveVelocity.Remove(action);
        }

        public static void LockMousePosition () {
            MainSync.InputManager.LockMousePosition();
        }
        public static void UnlockMousePosition () {
            MainSync.InputManager.UnlockMousePosition();
        }

        public static void SetMouseVisible () {
            MainSync.Game.IsMouseVisible = true;
        }

        public static void SetMouseInvisible () {
            MainSync.Game.IsMouseVisible = false;
        }

        public static void SetMousePosition (Vector2 position) {
            MainSync.InputManager.Mouse.SetPosition(position);
        }

        public static Vector2 GetMousePosition () {
            return MainSync.InputManager.MousePosition; // Alternatively: MainSync.InputManager.Mouse.Position;
        }

        public static Vector2 GetMouseDelta () {
            return MainSync.InputManager.MouseDelta; // Alternatively: MainSync.InputManager.Mouse.Delta;
        }
        public static float GetMouseWheelDelta () {
            return MainSync.InputManager.MouseWheelDelta;
        }

        public static bool AnyMouseButtonInteraction () {
            return PressedAnyMouseButton() || HeldAnyMouseButton() || ReleasedAnyMouseButton();
        }
        public static bool PressedAnyMouseButton () {
            return MainSync.InputManager.HasPressedMouseButtons;
        }
        public static bool HeldAnyMouseButton () {
            return MainSync.InputManager.HasDownMouseButtons;
        }
        public static bool ReleasedAnyMouseButton () {
            return MainSync.InputManager.HasReleasedMouseButtons;
        }
        public static bool MouseButtonPressed (MouseButton mouseButton) {
            return MainSync.InputManager.IsMouseButtonPressed(mouseButton);
        }
        public static bool MouseButtonHeld (MouseButton mouseButton) {
            return MainSync.InputManager.IsMouseButtonDown(mouseButton) || pressedButUnreleasedMouseButtons.Contains(mouseButton);
        }
        public static bool MouseButtonReleased (MouseButton mouseButton) {
            return MainSync.InputManager.IsMouseButtonReleased(mouseButton);
        }

        public static bool AnyKeysInteraction () {
            return PressedAnyKeys() || HeldAnyKeys() || ReleasedAnyKeys();
        }
        public static bool PressedAnyKeys () {
            return MainSync.InputManager.HasPressedKeys;
        }
        public static bool HeldAnyKeys () {
            return MainSync.InputManager.HasDownKeys;
        }
        public static bool ReleasedAnyKeys () {
            return MainSync.InputManager.HasReleasedKeys;
        }
        public static bool KeyPressed (Keys key) {
            return MainSync.InputManager.IsKeyPressed(key);
        }
        public static bool KeyHeld (Keys key) {
            return MainSync.InputManager.IsKeyDown(key) || pressedButUnreleasedKeys.Contains(key);
        }
        public static bool KeyReleased (Keys key) {
            return MainSync.InputManager.IsKeyReleased(key);
        }

        public static Vector2 GetMouseVelocity () {
            throw new NotImplementedException();
        }

        public static void RegisterMouseWheelAction (MouseWheelAction mouseWheelAction) {
            mouseWheelActions.Add(mouseWheelAction);
        }

        public static void DeregisterMouseWheelAction (MouseWheelAction mouseWheelAction) {
            mouseWheelActions.Remove(mouseWheelAction);
        }
    }

}
