using F7s.Engine;
using F7s.Modell.Handling.PlayerControllers;
using F7s.Modell.Physical.Localities;
using F7s.Utility;
using F7s.Utility.Geometry.Double;
using Stride.Core.Mathematics;
using System;
namespace F7s.Modell.Handling.PhysicalData {
    public abstract class PhysicalRepresentationData {
        public abstract Locality Locality ();
        public abstract double BoundingRadius ();

        public float AngularSize () {
            return (float) MM.AngularDiameterInDegreesFromDistanceAndRadius(DistanceToCamera(), BoundingRadius());
        }

        private static float GetMainCameraFieldOfView () {
            throw new NotImplementedException();
        }

        public float SizeInPixels () {
            double boundingDiameter = BoundingRadius() * 2;
            double distance = DistanceToCamera();
            float screenSize = ScreenSizeLength();
            float fieldOfView = GetMainCameraFieldOfView();

            float sizeInPixels = MM.AngularSizeInPixels(boundingDiameter, distance, screenSize, fieldOfView);

            return sizeInPixels;
        }
        protected float ScreenSizeLength () {
            return ScreenSize().Length();
        }
        public abstract Double3 Scale ();

        public virtual double DistanceToCamera () {
            return Kamera.TransformRelativeToCamera(Locality()).TranslationVector.Length();
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

        public virtual MatrixD ForcedPerspectiveTransform (float desiredDistanceFromCamera) {
            return Origin.ForcedPerspectiveTransform(Locality(), desiredDistanceFromCamera);
        }
        public MatrixD CameraCentricTransform () {
            return Kamera.TransformRelativeToCamera(Locality());
        }
        public MatrixD ForcedPerspectiveBaseTransform () {
            return Origin.ForcedProjectionBaseTransform(Locality());
        }

        public MatrixD OriginCentricTransform () {
            return Origin.TransformRelativeToOrigin(Locality());
        }
        public MatrixD PlayerCentricTransform () {
            return Player.TransformRelativeToPlayer(Locality());
        }
    }
}
