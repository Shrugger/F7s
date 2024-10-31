using F7s.Utility.Geometry;
using F7s.Utility.Shapes.Shapes2D;
using Stride.Core.Mathematics;
using System;
using System.Collections.Generic;

namespace F7s.Utility.Shapes {


    public abstract class Shape3Dim {


        public virtual Circle GetCircularCrossSection () {
            throw new Exception("Shape type " + GetType().Name + " has no circular cross-section.");
        }



        public static bool AreAdjacent (
            Shape3Dim shape1,
            Vector3 position1,
            Quaternion rotation1,
            Shape3Dim shape2,
            Vector3 position2,
            Quaternion rotation2,
            float shapeSizeAssumptionAdjustment
        ) {

            bool overlapIfSmaller = Shape3Dim.Intersect(
                                                  shape1: shape1.GetShapeMinusEpsilonVolume(),
                                                  position1: position1,
                                                  rotation1: rotation1,
                                                  shape2: shape1.GetShapeMinusEpsilonVolume(),
                                                  position2: position2,
                                                  rotation2: rotation2,
                                                  shapeSizeAssumptionAdjustment: shapeSizeAssumptionAdjustment
                                                 );

            bool overlapIfLarger = Shape3Dim.Intersect(
                                                 shape1: shape1.GetShapePlusEpsilonVolume(),
                                                 position1: position1,
                                                 rotation1: rotation1,
                                                 shape2: shape1.GetShapePlusEpsilonVolume(),
                                                 position2: position2,
                                                 rotation2: rotation2,
                                                 shapeSizeAssumptionAdjustment: shapeSizeAssumptionAdjustment
                                                );

            if (overlapIfSmaller == false) {
                System.Diagnostics.Debug.WriteLine(shape1 + " is too far from " + shape2 + " to be adjacent." + "\n");
            }

            return overlapIfSmaller == false && overlapIfLarger;
        }

        public virtual float ExternalVolume () {
            return SubstantialVolume();
        }


        public static bool Intersect (
            Shape3Dim shape1,
            Vector3 position1,
            Quaternion rotation1,
            Shape3Dim shape2,
            Vector3 position2,
            Quaternion rotation2,
            float shapeSizeAssumptionAdjustment
        ) {

            float radius1 = shape1.GetBoundingRadius();
            float radius2 = shape2.GetBoundingRadius();

            if (radius1 <= 0 || radius2 <= 0) {
                throw new Exception("Unusable radii: " + shape1 + " " + radius1 + " and " + shape2 + " " + radius2 + ".");
            }

            bool boundingSphereIntersection = Shape3Dim.SpheresIntersect(
                                                                     shape1: new Sphere(radius1),
                                                                     position1: position1,
                                                                     shape2: new Sphere(radius2),
                                                                     position2: position2,
                                                                     shapeSizeAssumptionAdjustment: shapeSizeAssumptionAdjustment
                                                                    );

            if (boundingSphereIntersection == false) {
                return false;
            }

            ShapeType st1 = shape1.ShapeType();
            ShapeType st2 = shape2.ShapeType();

            if (st1 == F7s.Utility.Shapes.ShapeType.Compound) {
                return (shape1 as CompoundShape).Intersects(position1, rotation1, shape2, position2, rotation2, shapeSizeAssumptionAdjustment);
            }
            if (st2 == F7s.Utility.Shapes.ShapeType.Compound) {
                return (shape2 as CompoundShape).Intersects(position2, rotation2, shape1, position1, rotation1, shapeSizeAssumptionAdjustment);
            }
            if (st1 == F7s.Utility.Shapes.ShapeType.Sphere && st2 == F7s.Utility.Shapes.ShapeType.Sphere) {
                return Shape3Dim.SpheresIntersect(
                                                shape1: (Sphere) shape1,
                                                position1: position1,
                                                shape2: (Sphere) shape2,
                                                position2: position2,
                                                shapeSizeAssumptionAdjustment: shapeSizeAssumptionAdjustment
                                               );
            }
            if (st1 == F7s.Utility.Shapes.ShapeType.Sphere) {
                return Shape3Dim.SphereIntersects(
                                               sphere: (Sphere) shape1,
                                               spherePosition: position1,
                                               other: shape2,
                                               otherPosition: position2,
                                               otherRotation: rotation2,
                                               shapeSizeAssumptionAdjustment: shapeSizeAssumptionAdjustment
                                              );
            }
            if (st2 == F7s.Utility.Shapes.ShapeType.Sphere) {
                return Shape3Dim.SphereIntersects(
                                               sphere: (Sphere) shape2,
                                               spherePosition: position2,
                                               other: shape1,
                                               otherPosition: position1,
                                               otherRotation: rotation1,
                                               shapeSizeAssumptionAdjustment: shapeSizeAssumptionAdjustment
                                              );
            }

            if (st1 == F7s.Utility.Shapes.ShapeType.Box) {
                return Shape3Dim.BoxIntersects(
                                            box: shape1,
                                            boxPosition: position1,
                                            boxRotation: rotation1,
                                            other: shape2,
                                            otherPosition: position2,
                                            otherRotation: rotation2,
                                            shapeSizeAssumptionAdjustment: shapeSizeAssumptionAdjustment
                                           );
            }
            if (st2 == F7s.Utility.Shapes.ShapeType.Box) {
                return Shape3Dim.BoxIntersects(
                                            box: shape2,
                                            boxPosition: position2,
                                            boxRotation: rotation2,
                                            other: shape1,
                                            otherPosition: position1,
                                            otherRotation: rotation1,
                                            shapeSizeAssumptionAdjustment: shapeSizeAssumptionAdjustment
                                           );
            }


            if (st1 == F7s.Utility.Shapes.ShapeType.Capsule || st1 == F7s.Utility.Shapes.ShapeType.Cylinder) {
                return Shape3Dim.CapsuleIntersects((Capsule) shape1, position1, rotation1, shape2, position2, rotation2, shapeSizeAssumptionAdjustment);
            }
            if (st2 == F7s.Utility.Shapes.ShapeType.Capsule || st2 == F7s.Utility.Shapes.ShapeType.Cylinder) {
                return Shape3Dim.CapsuleIntersects((Capsule) shape2, position2, rotation2, shape1, position1, rotation1, shapeSizeAssumptionAdjustment);
            }

            throw new NotImplementedException(message: "Unhandled shape type combination: " + st1 + " and " + st2);

        }
        public virtual Vector3 InternalNegativeShapeOffset () {
            return Vector3.Zero;
        }
        public virtual Shape3Dim GetInternalNegativeSpace () {
            return null;
        }

        public virtual bool CanContain (Shape3Dim prospectiveContent) {
            Shape3Dim internalNegativeSpace = GetInternalNegativeSpace();

            if (internalNegativeSpace == null) {
                return false;
            }

            return internalNegativeSpace.CanContainIfIsNegativeSpace(prospectiveContent);
        }

        protected virtual bool CanContainIfIsNegativeSpace (Shape3Dim prospectiveContent) {
            AxesLengthsOrdered thisExtents = FullExtents();
            AxesLengthsOrdered otherExtents = prospectiveContent.FullExtents();

            return AxesLengthsOrdered.CanFitInside(thisExtents, otherExtents);
        }

        public virtual bool ShapeIsPitchedNinetyDegrees () {
            return false;
        }
        public virtual bool ShapeIsRolledFortyFiveDegrees () {
            return false;
        }

        public static bool SpheresIntersect (
            Sphere shape1,
            Vector3 position1,
            Sphere shape2,
            Vector3 position2,
            float shapeSizeAssumptionAdjustment
        ) {

            float distance = Vector3.Distance(position1, position2);
            float combinedRadius = shape1.Radius + shape2.Radius;

            bool intersects = distance < combinedRadius + shapeSizeAssumptionAdjustment;
            return intersects;

        }

        public static bool SphereIntersects (
            Sphere sphere,
            Vector3 spherePosition,
            Shape3Dim other,
            Vector3 otherPosition,
            Quaternion otherRotation,
            float shapeSizeAssumptionAdjustment
        ) {
            throw new NotImplementedException();
        }


        public static bool BoxIntersects (
            Shape3Dim box,
            Vector3 boxPosition,
            Quaternion boxRotation,
            Shape3Dim other,
            Vector3 otherPosition,
            Quaternion otherRotation,
            float shapeSizeAssumptionAdjustment
        ) {
            throw new NotImplementedException();
        }

        public static bool CapsuleIntersects (
            Capsule capsule,
            Vector3 capsulePosition,
            Quaternion capsuleRotation,
            Shape3Dim other,
            Vector3 otherPosition,
            Quaternion otherRotation,
            float shapeSizeAssumptionAdjustment
        ) {
            throw new NotImplementedException();
        }


        public virtual Shape3Dim GetBestBoundingShape () {
            Box bb = GetBoundingBox();
            Sphere bs = GetBoundingSphere();

            if (bb.SubstantialVolume() < bs.SubstantialVolume()) {
                return bb;
            }

            return bs;
        }

        public virtual Box GetBoundingBox () {
            return new Box(fullExtents: FullExtents());
        }

        public virtual Sphere GetBoundingSphere () {
            return new Sphere(radius: GetBoundingRadius());
        }

        public abstract Vector3 RandomPointWithin ();

        public virtual bool UsesVertexColoring () {
            return false;
        }
        public virtual Vector3 HalfExtents () {
            return FullExtents() / 2.0f;
        }

        public virtual float LongestAxisLength () {
            return Math.Max(Math.Max(FullExtents().X, FullExtents().Y), FullExtents().Z);
        }

        public virtual AxesLengthsOrdered OrderedAxisLengths () {
            return new AxesLengthsOrdered(axes: FullExtents());
        }

        public Shape3Dim GetShapePlusEpsilonVolume () {
            return GetShapePlusSize(addition: Shape3Dim.EpsilonVolume);
        }

        public abstract Shape3Dim GetShapePlusSize (float addition);

        public Shape3Dim GetShapeMinusEpsilonVolume () {
            return GetShapeMinusVolume(subtraction: Shape3Dim.EpsilonVolume);
        }

        private const float EpsilonVolume = 0.00001f;

        public float BoundingWidth { get { return FullExtents().X; } }
        public float BoundingHeight { get { return FullExtents().Y; } }
        public float BoundingLength { get { return FullExtents().Z; } }

        public Shape3Dim GetShapeMinusVolume (float subtraction) {
            return GetShapePlusSize(addition: -subtraction);
        }

        public virtual bool UnityColliderIsPitchedForward () {
            return false;
        }
        public abstract Vector3 FullExtents ();
        public abstract List<Vector3> RelativeVertices ();

        public abstract float SubstantialVolume ();
        public abstract ShapeType ShapeType ();
        public abstract float SurfaceArea ();
        public abstract float ProfileArea (Vector3 angle);

        public abstract bool IsConvex ();

        public virtual float GetBoundingRadius () {
            float magnitude = FullExtents().Length();
            if (magnitude <= 0) {
                throw new Exception(this + " has magnitude " + magnitude + ".");
            }
            return magnitude / 2.0f;
        }

        public virtual float GetBoundingDiameter () {
            return FullExtents().Length();
        }

        public float MinX () {
            return -HalfExtents().X;
        }

        public float MaxX () {
            return HalfExtents().X;
        }

        public float MinY () {
            return -HalfExtents().Y;
        }

        public float MaxY () {
            return HalfExtents().Y;
        }

        public float MinZ () {
            return -HalfExtents().Z;
        }

        public float MaxZ () {
            return HalfExtents().Z;
        }

        public virtual void Normalize () {
            // Subtypes may want to implement this. Meant to make sure that the shape and its bounds have a common center.
        }

        public static implicit operator AxesLengthsOrdered (Shape3Dim shape) {
            return shape.OrderedAxisLengths();
        }

        public static implicit operator ShapeType (Shape3Dim shape) {
            return shape.ShapeType();
        }


        public override string ToString () {
            return GetType().Name;
        }

        public override bool Equals (object obj) {
            Shape3Dim other = obj as Shape3Dim;
            return base.Equals(obj) || (GetType() == other.GetType() && MM.ApproximatelyEqual(FullExtents(), other.FullExtents()));
        }

        public override int GetHashCode () {
            return base.GetHashCode();
        }

    }

}