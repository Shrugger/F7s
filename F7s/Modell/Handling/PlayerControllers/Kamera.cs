﻿using F7s.Modell.Physical;
using F7s.Modell.Physical.Localities;
using F7s.Utility.Geometry;
using F7s.Utility.Geometry.Double;
using Stride.Core.Mathematics;
using System;
using System.Diagnostics;

namespace F7s.Modell.Handling.PlayerControllers {

    /// <summary>
    /// Handles the camera, mostly for moving it independently of the player entity, and for interacting with rendering.
    /// </summary>
    public static class Kamera {

        public static float MaximumRenderDistance () {
            throw new NotImplementedException();
        }
        public static float MinimumRenderDistance () {
            throw new NotImplementedException();
        }

        private static Fixed locality;

        public static void SetLocality (Fixed locality) {
            Kamera.locality = locality;
        }

        public static Fixed GetLocality () {
            if (locality == null) {
                Locality fallback = Player.GetLocality();
                if (fallback != null) {
                    fallback.Validate();
                    locality = new Fixed(null, Transform3D.Identity, fallback);
                    locality.Name = "Camera Locality";
                } else {
                    throw new Exception("No player locality and no fallback found.");
                }
            }
            return locality;
        }

        public static Transform3D TransformRelativeToCamera (Locality locality) {
            return locality.GetRelativeTransform(GetLocality());
        }

        public static void Translate (Vector3 offset) {
            GetLocality().Translate(offset);
            UpdateCameraNodeTransform();
        }

        public static void SetTransform (Transform3D value) {
            GetLocality().SetTransform(value);
        }

        private static void UpdateCameraNodeTransform () {
            Transform3D localityTransform = GetLocality().GetAbsoluteTransform();
            Transform3D parentTransform = new Transform3D(localityTransform.Origin, Matrix3x3d.Identity);
            Transform3D cameraTransform = new Transform3D(Vector3d.Zero, localityTransform.Basis);

            throw new NotImplementedException(); // See below.
            // MainNode.Instance.CameraParent.Transform = parentTransform; // TODO: redo for Stride.
            // MainNode.Instance.Camera.Transform = cameraTransform;
        }

        public static Vector3 CameraNodeGlobalPosition () {
            throw new NotImplementedException();
            // return MainNode.Instance.CameraParent.GlobalPosition; // TODO: redo for Stride.
        }

        public static void Rotate (float yaw, float pitch, float roll = 0) {
            GetLocality().Rotate(yaw, pitch, roll);
        }

        public static void Update (double delta) {
            UpdateCameraNodeTransform();
        }

        public static void RotateEcliptic (float yaw, float pitch, float roll = 0) {
            GetLocality().RotateEcliptic(yaw, pitch, roll);
        }

        public static void LookAt (Vector3 relativePosition, Vector3? up = null) {
            locality.LookAt(relativePosition, up ?? Vector3.UnitY);
        }

        public static void AttachToPlayer () {
            Fixed cameraFixedLocality = GetLocality();
            Locality playerLocality = Player.GetLocality();
            SetLocality(cameraFixedLocality.Reanchored(playerLocality));
        }

        public static void DetachFromPlayer () {
            Fixed cameraFixedLocality = GetLocality();
            Locality playerLocality = Player.GetLocality();

            if (cameraFixedLocality != null) {
                SetLocality(cameraFixedLocality.Reanchored(playerLocality.HierarchySuperior()));
            } else {
                Fixed newCameraLocality = new Fixed(null, Transform3D.Identity, playerLocality.HierarchySuperior());
                newCameraLocality.Name = "Cam-Loc";
                SetLocality(newCameraLocality);
            }
        }

        public static void SetAnchor (Locality locality, Fixed.ReanchorMethodology methodology, Transform3D newTransform) {
            Fixed oldLocality = GetLocality();
            SetLocality(oldLocality.Reanchored(locality, methodology, newTransform));
        }

        public static void View (PhysicalEntity entity, Vector3d desiredRelativePosition, Vector3? up = null) {
            Debug.Assert(Vector3d.Zero != desiredRelativePosition);
            Transform3D newTransform = new Transform3D(desiredRelativePosition, locality.Transform.Basis);
            SetAnchor(entity.Locality, Fixed.ReanchorMethodology.UseNewTransform, newTransform);
            LookAt(-newTransform.Origin.ToVector3(), up);
        }
    }
}
