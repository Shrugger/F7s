using F7s.Utility.Geometry;
using Stride.Core.Mathematics;
using System;
using System.Collections.Generic;

namespace F7s.Utility.Shapes {


    public class Box : Shape3Dim {

        protected Vector3 fullExtents;

        public Box (double x, double y, double z)
            : this((float) x, (float) y, (float) z) { }
        public Box (float x, float y, float z)
            : this(fullExtents: new Vector3(x: x, y: y, z: z)) { }

        public Box (Vector3 aspectRatio, float volume)
            : this(fullExtents: Geom.ScaleBoxToVolume(boxExtents: aspectRatio, volume: volume)) { }

        public Box (Vector3 fullExtents) {
            this.fullExtents = fullExtents;
        }

        public static float Volume (Vector3 fullExtents) {
            return (fullExtents.X * fullExtents.Y * fullExtents.Z);
        }

        public override Shape3Dim GetShapePlusSize (float addition) {
            return new Box(
                           fullExtents: new Vector3(
                                                     x: Math.Clamp(this.fullExtents.X + addition, 0, float.MaxValue),
                                                     y: Math.Clamp(this.fullExtents.Y + addition, 0, float.MaxValue),
                                                     z: Math.Clamp(this.fullExtents.Z + addition, 0, float.MaxValue)
                                                    )
                          );
        }

        public override Vector3 FullExtents () {
            return this.fullExtents;
        }

        public override float SubstantialVolume () {
            return Box.Volume(fullExtents: this.FullExtents());
        }

        public static implicit operator ShapeType (Box shape) {
            return F7s.Utility.Shapes.ShapeType.Box;
        }

        public override ShapeType ShapeType () {
            return F7s.Utility.Shapes.ShapeType.Box;
        }

        public Vector3 RightUpperForwardCorner () {
            return new Vector3(x: this.MaxX(), y: this.MaxY(), z: this.MaxZ());
        }

        public Vector3 RightUpperBackCorner () {
            return new Vector3(x: this.MaxX(), y: this.MaxY(), z: this.MinZ());
        }

        public Vector3 RightLowerForwardCorner () {
            return new Vector3(x: this.MaxX(), y: this.MinY(), z: this.MaxZ());
        }

        public Vector3 RightLowerBackCorner () {
            return new Vector3(x: this.MaxX(), y: this.MinY(), z: this.MinZ());
        }

        public Vector3 LeftUpperForwardCorner () {
            return new Vector3(x: this.MinX(), y: this.MaxY(), z: this.MaxZ());
        }

        public Vector3 LeftUpperBackCorner () {
            return new Vector3(x: this.MinX(), y: this.MaxY(), z: this.MinZ());
        }

        public Vector3 LeftLowerForwardCorner () {
            return new Vector3(x: this.MinX(), y: this.MinY(), z: this.MaxZ());
        }

        public Vector3 LeftLowerBackCorner () {
            return new Vector3(x: this.MinX(), y: this.MinY(), z: this.MinZ());
        }

        public override List<Vector3> RelativeVertices () {
            return new List<Vector3> {
                                           this.RightUpperForwardCorner(),
                                           this.RightUpperBackCorner(),
                                           this.RightLowerForwardCorner(),
                                           this.RightLowerBackCorner(),
                                           this.LeftUpperForwardCorner(),
                                           this.LeftUpperBackCorner(),
                                           this.LeftLowerForwardCorner(),
                                           this.LeftLowerBackCorner()
                                       };

        }

        public override float SurfaceArea () {
            return Box.SurfaceArea(fullExtents: this.fullExtents);
        }

        public static float SurfaceArea (Vector3 fullExtents) {
            return 2.0f
                 * (fullExtents.X * fullExtents.Y + fullExtents.Y * fullExtents.Z + fullExtents.Z * fullExtents.X);
        }

        /// <summary>
        ///     Calculates the area orthographically exposed to a certain angle.
        /// </summary>
        public override float ProfileArea (Vector3 angle) {
            throw new NotImplementedException();
        }

        public override string ToString () {
            return this.GetType().Name + Rounding.RoundToFirstInterestingDigit(this.fullExtents);
        }

        public override bool IsConvex () {
            return true;
        }

        public override Vector3 RandomPointWithin () {
            Vector3 halfExtents = this.HalfExtents();
            return Alea.Vector3(halfExtents.X, halfExtents.Y, halfExtents.Z);
        }

        public static implicit operator Box (BoundingBox bounds) {
            return new Box(bounds.Extent);
        }
    }

}