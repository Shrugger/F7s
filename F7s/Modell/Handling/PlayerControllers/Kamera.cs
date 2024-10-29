using F7s.Mains;
using F7s.Modell.Physical;
using F7s.Modell.Physical.Localities;
using F7s.Utility;
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
                    locality = new Fixed(null, fallback, MatrixD.Identity);
                    locality.Name = "Camera Locality";
                } else {
                    throw new Exception("No Kamera locality and no fallback (Player locality) found.");
                }
            }
            return locality;
        }

        public static MatrixD TransformRelativeToCamera (Locality locality) {
            return locality.GetRelativeTransform(GetLocality());
        }

        public static void Translate (Double3 offset) {
            GetLocality().Translate(offset);
            UpdateCameraNodeTransform();
        }

        public static void SetTransform (MatrixD value) {
            GetLocality().SetTransform(value);
        }

        private static void UpdateCameraNodeTransform () {
            MatrixD localityTransform = GetLocality().GetAbsoluteTransform();

            Matrix parentTransform = Mathematik.Downscale(MatrixD.Transformation(localityTransform.TranslationVector, QuaternionD.Identity));
            Matrix cameraTransform = Mathematik.Downscale(MatrixD.Transformation(Double3.Zero, Mathematik.ExtractRotation(localityTransform)));

            MainSync.CameraParentEntity.Transform.UseTRS = false;
            MainSync.CameraParentEntity.Transform.LocalMatrix = parentTransform;
            MainSync.CameraEntity.Transform.UseTRS = false;
            MainSync.CameraEntity.Transform.LocalMatrix = cameraTransform;
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

        public static void LookAt (Double3 relativePosition, Double3? up = null) {
            locality.LookAt(relativePosition, up ?? Double3.UnitY);
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
                Fixed newCameraLocality = new Fixed(null, playerLocality.HierarchySuperior(), MatrixD.Identity);
                newCameraLocality.Name = "Cam-Loc";
                SetLocality(newCameraLocality);
            }
        }

        public static void SetAnchor (Locality locality, Fixed.ReanchorMethodology methodology, MatrixD newTransform) {
            Fixed oldLocality = GetLocality();
            SetLocality(oldLocality.Reanchored(locality, methodology, newTransform));
        }

        public static void View (PhysicalEntity entity, Double3 desiredRelativePosition, Double3? up = null) {
            Debug.Assert(Double3.Zero != desiredRelativePosition);
            MatrixD newTransform = MatrixD.Transformation(desiredRelativePosition, Mathematik.ExtractRotation(locality.Transform));
            SetAnchor(entity.Locality, Fixed.ReanchorMethodology.UseNewTransform, newTransform);
            LookAt(-newTransform.TranslationVector, up);
        }
    }
}
