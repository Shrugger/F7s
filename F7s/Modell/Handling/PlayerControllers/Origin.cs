using F7s.Modell.Physical.Localities;
using F7s.Utility;
using F7s.Utility.Geometry.Double;
using Stride.Core.Mathematics;
using System;
using System.Diagnostics;

namespace F7s.Modell.Handling.PlayerControllers {


    /* TODO: Ha-ha, need to split it into Player, Origin and Camera, after all. 
     * 
     * Want floating origins independent of the player position (for performance and physics)
     * Want camera independent of player (for debugging or third-person perspective)
     * 
     * The Camera independence is mostly needed for forced perspective.
     * 
     * Wait, we already do this, to some extent...better just to add the floating origin and see what breaks.
     * 
     * TODO: It's all kinds of wrong. Think carefully about why.
     */

    /// <summary>
    /// Provides an Origin from which to derive a coordinate system.
    /// </summary>
    public static class Origin {

        private static readonly bool logOriginSwitchs = false;

        private static Locality originLocality;
        private static ProjectionOrigin projectionOrigin;

        public static bool Initialized => originLocality != null;

        public static void SetOriginLocality (Locality value) {
            if (value.Name == null) {
                throw new Exception("Nameless Origin.");
            }
            if (value == originLocality) {
                return;
            }
            if (originLocality != null) {
                System.Diagnostics.Debug.WriteLine("Setting Origin from " + (originLocality?.ToString() ?? "NULL") + " to " + value + ".");
            }
            originLocality = value;
        }

        public static MatrixD ForcedProjectionBaseTransform (Locality locality) {
            InitializeProjectionOrigin();
            if (projectionOrigin == null) {
                throw new Exception();
            }
            return locality.GetRelativeTransform(projectionOrigin);
        }


        public static MatrixD ForcedPerspectiveTransform (Locality locality, float desiredDistanceFromCamera) {
            MatrixD forcedPerspectiveBaseTransform = ForcedProjectionBaseTransform(locality);
            Double3 forcedPerspectiveBaseOrigin = forcedPerspectiveBaseTransform.TranslationVector;
            Debug.Assert(Double3.Zero != forcedPerspectiveBaseOrigin);
            Double3 origin = MM.Normalize(forcedPerspectiveBaseOrigin) * desiredDistanceFromCamera;
            forcedPerspectiveBaseTransform.TranslationVector = origin;
            return forcedPerspectiveBaseTransform;
        }

        public static MatrixD TransformRelativeToOrigin (Locality locality) {
            if (originLocality == null) {
                throw new Exception("You forgot to set an Origin.");
            }
            return locality.GetRelativeTransform(originLocality);
        }

        public static Locality GetFloatingOriginFloatingAnchor () {
            FloatingOrigin floatingOrigin = originLocality as FloatingOrigin;
            Locality anchor;
            if (floatingOrigin != null) {
                anchor = floatingOrigin.FloatingAnchor;
            } else {
                anchor = originLocality;
            }
            anchor.Validate();
            return anchor;
        }

        public static void UseKameraAsOrigin () {
            Locality newLocality = new Fixed(null, Kamera.GetLocality(), MatrixD.Identity);
            newLocality.Name = "Ori-Cam";
            SetOriginLocality(newLocality);
            if (logOriginSwitchs) {
                System.Diagnostics.Debug.WriteLine("Origin set to Camera.");
            }
        }

        public static void UseKameraAsFloatingOrigin () {
            Locality oldLocality = Kamera.GetLocality();
            if (oldLocality is FloatingOrigin) {
                return;
            }
            if (oldLocality == Player.GetLocality()) {
                throw new Exception("Kamera Locality equals Player Locality; meaning of floating origin anchor switch unclear.");
            }
            Locality newLocality = new FloatingOrigin(oldLocality);
            newLocality.Name = "Flo-Ori-Cam";
            SetOriginLocality(newLocality);

            if (logOriginSwitchs) {
                System.Diagnostics.Debug.WriteLine("Floating origin set to Camera.");
            }
        }

        public static void UsePlayerAsOrigin () {
            Locality newLocality = new Fixed(Player.GetPhysicalEntity(), Player.GetLocality(), MatrixD.Identity);
            newLocality.Name = "Ori-Ple";
            SetOriginLocality(newLocality);
            if (logOriginSwitchs) {
                System.Diagnostics.Debug.WriteLine("Origin set to Player.");
            }
        }

        public static void UsePlayerAsFloatingOrigin () {
            Locality oldLocality = Player.GetLocality();
            if (oldLocality is FloatingOrigin) {
                return;
            }
            if (oldLocality == Kamera.GetLocality()) {
                throw new Exception("Kamera Locality equals Player Locality; meaning of floating origin anchor switch unclear.");
            }
            Locality newLocality = new FloatingOrigin(oldLocality);
            newLocality.Name = "Flo-Ori-Ple";
            SetOriginLocality(newLocality);

            if (logOriginSwitchs) {
                System.Diagnostics.Debug.WriteLine("Floating origin set to Player.");
            }
        }

        private static void InitializeProjectionOrigin () {
            if (projectionOrigin == null) {
                if (originLocality != null) {
                    projectionOrigin = new ProjectionOrigin();
                    projectionOrigin.Name = "Pro-Ori";
                }
            }
        }

        public static void Update (double deltaTime) {
            originLocality?.Update(deltaTime);
        }

        public static Locality GetOriginLocality () {
            return originLocality;
        }
    }
}
