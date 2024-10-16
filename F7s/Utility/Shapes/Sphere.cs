using F7s.Utility.Shapes.Shapes2D;
using Stride.Core.Mathematics; using System;
using System;
using System.Collections.Generic;

namespace F7s.Utility.Shapes
{

    [Serializable]
    public class Sphere : Shape3Dim {

        public Sphere(double radius) : this((float)radius) { }
        public Sphere(float radius) {
            if (float.IsNaN(f: radius) || radius <= 0) {
                throw new Exception(message: "Impossible radius " + radius + ".");
            }

            this.Radius = radius;
        }

        public override bool IsConvex() {
            return true;
        }
        public override string ToString() {
            return this.GetType().Name + "(" + Rounding.RoundToFirstInterestingDigit(this.Radius) + ")";
        }
        public float Radius { get; protected set; }

        public override float GetBoundingRadius() {
            return this.Radius;
        }

        public static float Volume(float radius) {
            return (4.0f / 3.0f * MathF.PI * MathF.Pow(radius, 3));
        }

        public static float Diameter(float radius) {
            return radius * 2.0f;
        }

        public override float LongestAxisLength() {
            return this.Diameter();
        }

        public override Shape3Dim GetShapePlusSize(float addition) {
            return new Sphere(radius: Math.Clamp(this.Radius + addition, 0, float.MaxValue));
        }

        public override Vector3 FullExtents() {
            float diameter = this.Diameter();
            return new Vector3(diameter, diameter, diameter);
        }

        public override Vector3 HalfExtents() {
            float radius = this.Radius;
            return new Vector3(radius, radius, radius);
        }

        public override float SubstantialVolume() {
            return Sphere.Volume(radius: this.Radius);
        }

        public float Diameter() {
            return Sphere.Diameter(radius: this.Radius);
        }

        public static implicit operator ShapeType(Sphere shape) {
            return Shapes.ShapeType.Sphere;
        }

        public override ShapeType ShapeType() {
            return Shapes.ShapeType.Sphere;
        }

        public override List<Vector3> RelativeVertices() {
            return new List<Vector3> {
                                           new Vector3(x: this.MinX(), y: 0,           z: 0),
                                           new Vector3(x: this.MaxX(), y: 0,           z: 0),
                                           new Vector3(x: 0,           y: this.MinY(), z: 0),
                                           new Vector3(x: 0,           y: this.MaxY(), z: 0),
                                           new Vector3(x: 0,           y: 0,           z: this.MinZ()),
                                           new Vector3(x: 0,           y: 0,           z: this.MaxZ())
                                       };
        }

        public override float SurfaceArea() {
            return 4.0f * MathF.PI * this.Radius * this.Radius;
        }

        public override float ProfileArea(Vector3 angle) {
            throw new NotImplementedException(
                                              message:
                                              "The profile surface at any angle is simply a circle with the same radius as this sphere."
                                             );
        }

        public override bool Equals(object obj) {
            return obj is Sphere sphere &&
                   base.Equals(obj) &&
                   this.Radius == sphere.Radius;
        }

        public override int GetHashCode() {
            var hashCode = 1605559401;
            hashCode = hashCode * -1521134295 + base.GetHashCode();
            hashCode = hashCode * -1521134295 + this.Radius.GetHashCode();
            return hashCode;
        }


        public override Circle GetCircularCrossSection() {
            return new Circle(this.Radius);
        }

        public override Vector3 RandomPointWithin() {
            return Alea.InUnitSphere() * this.Radius;
        }
    }

}