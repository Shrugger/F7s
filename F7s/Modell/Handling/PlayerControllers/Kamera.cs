using F7s.Modell.Handling.PlayerControllers;
using F7s.Modell.Physical;
using F7s.Modell.Physical.Localities;
using F7s.Utility.Geometry;
using Stride.Core.Mathematics;
using System;
using System.Diagnostics;

namespace F7s.Engine.PlayerControllers {

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
                    throw new System.Exception("No player locality and no fallback found.");
                }
            }
            return locality;
        }

        public static Transform3D TransformRelativeToCamera (Locality locality) {
            return locality.GetRelativeTransform(Kamera.GetLocality());
        }

        public static void Translate (Vector3 offset) {
            Kamera.GetLocality().Translate(offset);
            Kamera.UpdateCameraNodeTransform();
        }

        public static void SetTransform (Transform3D value) {
            Kamera.GetLocality().SetTransform(value);
        }

        private static void UpdateCameraNodeTransform () {
            Transform3D localityTransform = Kamera.GetLocality().GetAbsoluteTransform();
            Transform3D parentTransform = new Transform3D(Matrix3x3d.Identity, localityTransform.Origin);
            Transform3D cameraTransform = new Transform3D(localityTransform.Basis, Vector3d.Zero);

            throw new NotImplementedException(); // See below.
            // MainNode.Instance.CameraParent.Transform = parentTransform; // TODO: redo for Stride.
            // MainNode.Instance.Camera.Transform = cameraTransform;
        }

        public static Vector3 CameraNodeGlobalPosition () {
            throw new NotImplementedException();
            // return MainNode.Instance.CameraParent.GlobalPosition; // TODO: redo for Stride.
        }

        public static void Rotate (float yaw, float pitch, float roll = 0) {
            Kamera.GetLocality().Rotate(yaw, pitch, roll);
        }

        public static void Update (double delta) {
            Kamera.UpdateCameraNodeTransform();
        }

        public static void RotateEcliptic (float yaw, float pitch, float roll = 0) {
            Kamera.GetLocality().RotateEcliptic(yaw, pitch, roll);
        }

        public static void LookAt (Vector3 relativePosition, Vector3? up = null) {
            locality.LookAt(relativePosition, up ?? Vector3.UnitY);
        }

        public static void AttachToPlayer () {
            Fixed cameraFixedLocality = Kamera.GetLocality();
            Locality playerLocality = Player.GetLocality();
            Kamera.SetLocality(cameraFixedLocality.Reanchored(playerLocality));
        }

        public static void DetachFromPlayer () {
            Fixed cameraFixedLocality = Kamera.GetLocality();
            Locality playerLocality = Player.GetLocality();

            if (cameraFixedLocality != null) {
                Kamera.SetLocality(cameraFixedLocality.Reanchored(playerLocality.HierarchySuperior()));
            } else {
                Fixed newCameraLocality = new Fixed(null, Transform3D.Identity, playerLocality.HierarchySuperior());
                newCameraLocality.Name = "Cam-Loc";
                Kamera.SetLocality(newCameraLocality);
            }
        }

        public static void SetAnchor (Locality locality, Fixed.ReanchorMethodology methodology, Transform3D? newTransform) {
            Fixed oldLocality = Kamera.GetLocality();
            SetLocality(oldLocality.Reanchored(locality, methodology, newTransform));
        }

        public static void View (PhysicalEntity entity, Vector3d desiredRelativePosition, Vector3? up = null) {
            Debug.Assert(Vector3d.Zero != desiredRelativePosition);
            Transform3D newTransform = new Transform3D(locality.Transform.Basis, desiredRelativePosition);
            SetAnchor(entity.Locality, Fixed.ReanchorMethodology.UseNewTransform, newTransform);
            Kamera.LookAt(-newTransform.Origin.ToVector3(), up);
        }
    }
}
