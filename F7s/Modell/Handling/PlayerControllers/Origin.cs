﻿using F7s.Engine.PlayerControllers;
using F7s.Modell.Physical.Localities;
using F7s.Geometry;
using System;
using System.Diagnostics;
using F7s.Utility.Geometry;

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
                Console.WriteLine("Setting Origin from " + (originLocality?.ToString() ?? "NULL") + " to " + value + ".");
            }
            originLocality = value;
        }

        public static Transform3D ForcedProjectionBaseTransform (Locality locality) {
            InitializeProjectionOrigin();
            if (projectionOrigin == null) {
                throw new Exception();
            }
            return locality.GetRelativeTransform(projectionOrigin);
        }


        public static Transform3D ForcedPerspectiveTransform (Locality locality, float desiredDistanceFromCamera) {
            Transform3D forcedPerspectiveBaseTransform = ForcedProjectionBaseTransform(locality);
            Vector3d forcedPerspectiveBaseOrigin = forcedPerspectiveBaseTransform.Origin;
            Debug.Assert(Vector3d.Zero != forcedPerspectiveBaseOrigin);
            Vector3d origin = forcedPerspectiveBaseOrigin.Normalized() * desiredDistanceFromCamera;
            forcedPerspectiveBaseTransform.Origin = origin;
            return forcedPerspectiveBaseTransform;
        }

        public static Transform3D TransformRelativeToOrigin (Locality locality) {
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
            Locality newLocality = new Fixed(null, Transform3D.Identity, Kamera.GetLocality());
            newLocality.Name = "Ori-Cam";
            SetOriginLocality(newLocality);
            if (logOriginSwitchs) {
                Console.WriteLine("Origin set to Camera.");
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
                Console.WriteLine("Floating origin set to Camera.");
            }
        }

        public static void UsePlayerAsOrigin () {
            Locality newLocality = new Fixed(Player.GetPhysicalEntity(), Transform3D.Identity, Player.GetLocality());
            newLocality.Name = "Ori-Ple";
            SetOriginLocality(newLocality);
            if (logOriginSwitchs) {
                Console.WriteLine("Origin set to Player.");
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
                Console.WriteLine("Floating origin set to Player.");
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
