using Stride.Core.Mathematics;
using Stride.Input;
using System;
using System.Collections.Generic;

namespace F7s.Engine.InputHandling {

    public static class InputHandler {

        // TODO: Make sure that all AbstractInputActions are actually registered, triggerable and handled.

        private static InputManager strideInputManager = new InputManager();

        private static readonly List<InputVectorAction> eventActions = new List<InputVectorAction>();
        private static readonly List<InputVectorAction> keyHoldActionsOnUpdate = new List<InputVectorAction>();
        private static readonly List<InputVectorAction> keyHoldActionsOnPhysicsUpdate = new List<InputVectorAction>();
        private static readonly List<MouseDeltaAction> onMouseMoveDelta = new List<MouseDeltaAction>();
        private static readonly List<MouseVelocityAction> onMouseMoveVelocity = new List<MouseVelocityAction>();

        public static void Update () {
            if (strideInputManager.KeyEvents.Count > 0) {
                keyHoldActionsOnUpdate.ForEach(a => a.TriggerIfMatch());
            }
        }

        public static void PhysicsUpdate () {
            keyHoldActionsOnPhysicsUpdate.ForEach(a => a.TriggerIfMatch());
        }

        public static void RegisterEventAction (InputVectorAction action) {
            eventActions.Add(action);
        }
        public static void DeregisterEventAction (InputVectorAction action) {
            eventActions.Remove(action);
        }
        public static void RegisterKeyHoldActionOnProcess (InputVectorAction action) {
            keyHoldActionsOnUpdate.Add(action);
        }
        public static void DeregisterKeyHoldActionOnProcess (InputVectorAction action) {
            keyHoldActionsOnUpdate.Remove(action);
        }
        public static void RegisterKeyHoldActionOnPhysicsProcess (InputVectorAction action) {
            keyHoldActionsOnPhysicsUpdate.Add(action);
        }
        public static void DeregisterKeyHoldActionOnPhysicsProcess (InputVectorAction action) {
            keyHoldActionsOnPhysicsUpdate.Remove(action);
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
            strideInputManager.LockMousePosition();
            strideInputManager.Mouse.LockPosition();
        }
        public static void UnlockMousePosition () {
            strideInputManager.UnlockMousePosition();
            strideInputManager.Mouse.UnlockPosition();
        }

        public static void SetMouseVisible () {
            throw new NotImplementedException();
        }

        public static void SetMouseInvisible () {
            throw new NotImplementedException();
        }

        public static void SetMousePosition (Vector2 position) {
            strideInputManager.Mouse.SetPosition(position);
        }

        public static Vector2 GetMousePosition () {
            return strideInputManager.MousePosition; // Alternatively: strideInputManager.Mouse.Position;
        }

        public static Vector2 GetMouseDelta () {
            return strideInputManager.MouseDelta; // Alternatively: strideInputManager.Mouse.Delta;
        }
        public static float GetMouseWheelDelta () {
            return strideInputManager.MouseWheelDelta;
        }

        public static bool AnyMouseButtonInteraction () {
            return PressedAnyMouseButton() || HeldAnyMouseButton() || ReleasedAnyMouseButton();
        }
        public static bool PressedAnyMouseButton () {
            return strideInputManager.HasPressedMouseButtons;
        }
        public static bool HeldAnyMouseButton () {
            return strideInputManager.HasDownMouseButtons;
        }
        public static bool ReleasedAnyMouseButton () {
            return strideInputManager.HasReleasedMouseButtons;
        }
        public static bool MouseButtonPressed (MouseButton mouseButton) {
            return strideInputManager.IsMouseButtonPressed(mouseButton);
        }
        public static bool MouseButtonHeld (MouseButton mouseButton) {
            return strideInputManager.IsMouseButtonDown(mouseButton);
        }
        public static bool MouseButtonReleased (MouseButton mouseButton) {
            return strideInputManager.IsMouseButtonReleased(mouseButton);
        }

        public static bool AnyKeysInteraction () {
            return PressedAnyKeys() || HeldAnyKeys() || ReleasedAnyKeys();
        }
        public static bool PressedAnyKeys () {
            return strideInputManager.HasPressedKeys;
        }
        public static bool HeldAnyKeys () {
            return strideInputManager.HasDownKeys;
        }
        public static bool ReleasedAnyKeys () {
            return strideInputManager.HasReleasedKeys;
        }
        public static bool KeyPressed (Keys key) {
            return strideInputManager.IsKeyPressed(key);
        }
        public static bool KeyHeld (Keys key) {
            return strideInputManager.IsKeyDown(key);
        }
        public static bool KeyReleased (Keys key) {
            return strideInputManager.IsKeyReleased(key);
        }

    }

}
