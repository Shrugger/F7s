using F7s.Engine.InputHandling;
using F7s.Modell.Abstract;
using F7s.Modell.Handling.PlayerControllers.ControlSets;
using F7s.Modell.Physical;
using F7s.Modell.Physical.Localities;
using F7s.Utility;
using F7s.Utility.Geometry.Double;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Stride.Core.Mathematics;
using System;
using System.Diagnostics;

namespace F7s.Modell.Handling.PlayerControllers {

    /// <summary>
    /// Handles control of the player entity, or blocks it in favor of controlling the camera or the UI.
    /// </summary>
    public static class Player {

        private static GameEntity gameEntity;
        private static PhysicalEntity physicalEntity;

        public static float PanSpeed { get; private set; } = 10.0f;

        public static InputSet GeneralControls { get; } = new GeneralControlsSet();
        public static InputSet FirstPersonPlayerControls { get; } = new FirstPersonPlayerControlsSet();
        public static InputSet FreeCameraControls { get; } = new FreeCameraControlsSet();
        public static InputSet UiControls { get; } = new UiControlsSet();
        private static InputSet activeControlSet = null;
        private static InputSet fallbackControlSet = null;

        public static void SetGameEntity (GameEntity entity) {
            Assert.IsNotNull(entity);
            gameEntity = entity;
        }

        public static void SetPhysicalEntity (PhysicalEntity entity) {
            Assert.IsNotNull(entity);
            physicalEntity = entity;
            Player.SetGameEntity(entity);
        }

        public static void UnsetPhysicalEntity () {
            physicalEntity = null;
            UnsetGameEntity();
        }

        public static void UnsetGameEntity () {
            gameEntity = null;
        }

        static Player () {
            Debug.Assert(null != FreeCameraControls);
            fallbackControlSet = FreeCameraControls;
        }

        public static string Report () {
            return "Player " + gameEntity + " " + physicalEntity;
        }

        public static MatrixD TransformRelativeToPlayer (Locality locality) {
            return locality.GetRelativeTransform(GetLocality());
        }


        public static Locality GetLocality () {
            if (GetPhysicalEntity() != null) {
                Locality locality = GetPhysicalEntity().GetLocality();
                locality.Validate();
                return locality;
            } else {
                return null;
            }
        }

        public static PhysicalEntity GetPhysicalEntity () {
            if (physicalEntity != null) {
                return physicalEntity;
            } else {
                return null;
            }
        }

        public static MatrixD GetAbsoluteTransform () {
            if (physicalEntity != null) {
                return MatrixD.Identity;
            } else {
                throw new Exception();
            }
        }

        public static void Update (double delta) {

        }

        public static bool IsEntity (PhysicalEntity physicalEntity) {
            return Player.physicalEntity == physicalEntity;
        }
        public static bool IsEntity (GameEntity entity) {
            return gameEntity == entity;
        }

        public static void Translate (Vector3 relativeOffset) {
            physicalEntity.Locality.Translate(relativeOffset);
        }

        public static void SetTransform (MatrixD value) {
            physicalEntity.Locality.SetTransform(value);
        }

        public static void ActivateFirstPersonPlayerControls () {
            ActivateControlSet(FirstPersonPlayerControls);
        }
        public static void ActivateFreeCameraControls () {
            ActivateControlSet(FreeCameraControls);
        }
        public static void ActivateUiControls () {
            if (activeControlSet != null) {
                fallbackControlSet = activeControlSet;
            }
            ActivateControlSet(UiControls);
        }
        public static void DeactivateUiControls () {
            ActivateControlSet(fallbackControlSet);
            fallbackControlSet = null;
        }
        private static void ActivateControlSet (InputSet controlSet) {
            Debug.Assert(null != controlSet);
            activeControlSet?.Deactivate();
            controlSet.Activate();
            activeControlSet = controlSet;
            Console.WriteLine("Control mode: " + controlSet);
        }

        public static void RotatePlayer (Vector2 mouseDelta) {
            (float, float) yawAndPitch = CalculateYawAndPitchFromMouseDelta(mouseDelta);
            float yaw = yawAndPitch.Item1;
            float pitch = yawAndPitch.Item2;

            Locality locality = physicalEntity.Locality;
            locality.RotateEcliptic(yaw, pitch);
        }
        public static void RotateCamera (Vector2 mouseDelta) {
            (float, float) yawAndPitch = CalculateYawAndPitchFromMouseDelta(mouseDelta);
            float yaw = yawAndPitch.Item1;
            float pitch = yawAndPitch.Item2;

            Kamera.RotateEcliptic(yaw, pitch);
        }

        private static (float, float) CalculateYawAndPitchFromMouseDelta (Vector2 mouseDelta) {
            const float sensitivityX = 30f;
            const float sensitivityY = 10f;

            float yaw = Mathematik.DegToRad(Math.Clamp(mouseDelta.X * -1 * sensitivityY, -720, 720));
            float pitch = Mathematik.DegToRad(Math.Clamp(mouseDelta.Y * sensitivityX, -720, 720));

            return (yaw, pitch);
        }


        public static void SpeedUp () {
            PanSpeed *= 10;
        }
        public static void SpeedDown () {
            PanSpeed /= 10;
        }
        public static void SetPanSpeed (float value) {
            PanSpeed = value;
        }

        public static Locality GetLocalityParent () {
            return GetLocality()?.HierarchySuperior();
        }
    }
}