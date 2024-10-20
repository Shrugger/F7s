using F7s.Engine;
using F7s.Modell.Handling.PlayerControllers;
using F7s.Modell.Physical.Localities;
using F7s.Utility;
using F7s.Utility.Geometry;
using Stride.Core.Mathematics;
using System;

namespace F7s.Modell.Handling.PhysicalData {
    public abstract class PhysicalRepresentationData {
        public abstract Locality Locality ();
        public abstract float BoundingRadius ();
        public float AngularSize () {
            return Mathematik.AngularDiameterInDegreesFromDistanceAndRadius(DistanceToCamera(), BoundingRadius());
        }

        private static float GetMainCameraFieldOfView () {
            throw new NotImplementedException();
        }

        public float SizeInPixels () {
            float boundingDiameter = BoundingRadius() * 2;
            float distance = DistanceToCamera();
            float screenSize = ScreenSizeLength();
            float fieldOfView = GetMainCameraFieldOfView();

            float sizeInPixels = Mathematik.AngularSizeInPixels(boundingDiameter, distance, screenSize, fieldOfView);

            return sizeInPixels;
        }
        protected float ScreenSizeLength () {
            return ScreenSize().Length();
        }
        public abstract Vector3 Scale ();

        public virtual float DistanceToCamera () {
            return (float) Kamera.TransformRelativeToCamera(Locality()).Origin.Length();
        }

        public Vector2 ScreenSize () {
            return Viewport.GetScreenSize();
        }

        public float SizeInScreenFraction () {
            return SizeInPixels() / ScreenSizeLength();
        }

        public bool TooSmallToSee () {
            return SizeInPixels() < 1;
        }

        public virtual Transform3D ForcedPerspectiveTransform (float desiredDistanceFromCamera) {
            return Origin.ForcedPerspectiveTransform(Locality(), desiredDistanceFromCamera);
        }
        public Transform3D CameraCentricTransform () {
            return Kamera.TransformRelativeToCamera(Locality());
        }
        public Transform3D ForcedPerspectiveBaseTransform () {
            return Origin.ForcedProjectionBaseTransform(Locality());
        }

        public Transform3D OriginCentricTransform () {
            return Origin.TransformRelativeToOrigin(Locality());
        }
        public Transform3D PlayerCentricTransform () {
            return Player.TransformRelativeToPlayer(Locality());
        }
    }
}
