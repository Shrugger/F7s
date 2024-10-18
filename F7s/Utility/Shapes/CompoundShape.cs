using F7s.Geometry;
using Stride.Core.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace F7s.Utility.Shapes
{


    public class CompoundShape : Shape3Dim {

        public class Constituent {

            public readonly Shape3Dim shape;
            public Transform3D transform;
            public readonly string name;
            public readonly float externalVolume;
            public readonly float substantialVolume;
            public Farbe? Color;

            public Vector3 position { get { return this.transform.Origin.ToVector3(); } set { this.transform.Origin = value; } }
            public Quaternion rotation => this.transform.Basis.GetRotationQuaternion();

            public Constituent (Shape3Dim shape, Vector3 position, Vector3? rotation = null, string name = null, Farbe? color = null)
                : this(shape, position, rotation != null ? Geom.DegreesToQuaternion(rotation.Value) : null, name, color) { }
            public Constituent (Shape3Dim shape, Vector3 position, Quaternion? rotation = null, string name = null, Farbe? color = null) {
                this.shape = shape;
                this.transform = new Transform3D(new Basis(rotation ?? Quaternion.Identity), position);
                this.name = name;
                this.externalVolume = shape.ExternalVolume();
                this.substantialVolume = shape.SubstantialVolume();
                this.Color = color;
            }

            public void Paint (Farbe color) {
                this.Color = color;
            }


            public override string ToString () {
                return this.name;
            }

            public void Normalize () {
                this.shape.Normalize();
            }
        }

        private bool normalized = false;

        protected Vector3? fullExtents;
        private List<Constituent> constituents { get; set; }

        public CompoundShape () : this(null) { }
        public CompoundShape (Vector3? fullExtents = null) : this(fullExtents, new List<Constituent>()) { }
        public CompoundShape (Vector3? fullExtents = null, params Constituent[] constituents) : this(fullExtents, new List<Constituent>(constituents)) { }
        public CompoundShape (Vector3? fullExtents = null, List<Constituent> constituents = null) {
            this.fullExtents = fullExtents;
            this.constituents = constituents ?? this.constituents ?? new List<Constituent>();

            this.Normalize();
        }

        public void ApplyToConstituents (Action<Constituent> action) {
            this.constituents.ForEach(c => action.Invoke(c));
        }
        public Constituent AddConstituent (Shape3Dim shape, Vector3 relativePosition, Vector3 relativeRotation, string name, Farbe? color = null) {
            return this.AddConstituent(new Constituent(shape, relativePosition, Geom.DegreesToQuaternion(relativeRotation), name, color));
        }
        public Constituent AddConstituent (Shape3Dim shape, Vector3 relativePosition, Quaternion relativeRotation, string name, Farbe? color = null) {
            return this.AddConstituent(new Constituent(shape, relativePosition, relativeRotation, name, color));
        }


        public Constituent AddConstituent (Constituent constituent) {
            this.constituents.Add(constituent);
            this.normalized = false;
            return constituent;
        }


        public override bool IsConvex () {
            return false;
        }

        public override Shape3Dim GetShapePlusSize (float addition) {
            throw new NotImplementedException("GetShapePlusSize is not implemented for type " + this.GetType() + ".");
        }

        public override float ProfileArea (Vector3 angle) {
            throw new NotImplementedException();
        }

        public override ShapeType ShapeType () {
            return F7s.Utility.Shapes.ShapeType.Compound;
        }

        public override float SubstantialVolume () {
            return this.constituents.Sum(d => d.shape.SubstantialVolume());
        }

        public override float SurfaceArea () {
            return this.constituents.Sum(d => d.shape.SurfaceArea());
        }

        public override Vector3 FullExtents () {
            this.Normalize();
            return this.fullExtents.Value;
        }

        public bool Intersects (
            Vector3 position,
            Quaternion rotation,
            Shape3Dim other,
            Vector3 otherPosition,
            Quaternion otherRotation,
            float shapeSizeAssumptionAdjustment) {

            bool queryEachConstituent = false;
            if (queryEachConstituent) {
                foreach (Constituent constituent in this.constituents) {
                    if (Shape3Dim.Intersect(
                                          constituent.shape,
                                          position + constituent.position,
                                          rotation * constituent.rotation,
                                          other,
                                          otherPosition,
                                          otherRotation,
                                          shapeSizeAssumptionAdjustment
                                         )) {
                        return true;
                    }
                }
                return false;
            } else {
                return Shape3Dim.Intersect(this.GetBoundingBox(),
                                          position,
                                          rotation,
                                          other,
                                          otherPosition,
                                          otherRotation,
                                          shapeSizeAssumptionAdjustment
                                         );
            }
        }

        public override void Normalize () {

            if (this.constituents != null && this.constituents.Count > 0) {

                this.constituents.ForEach(c => c.Normalize());

                if (!this.normalized) {

                    Stride.Core.Mathematics.BoundingBox bb = this.CalculateBounds();

                    this.fullExtents = this.fullExtents ?? bb.Extent;

                    Vector3 offset = bb.Center;

                    this.constituents.ForEach(c => c.position -= offset);

                    if (bb.Extent != this.CalculateBounds().Extent) {
                        throw new Exception(this + " bounds (" + bb + ") -> (" + this.CalculateBounds() + ").");
                    }

                    this.normalized = true;
                }
            }

            base.Normalize();
        }


        public Stride.Core.Mathematics.BoundingBox CalculateBounds () {
            if (this.constituents == null || this.constituents.Count == 0) {
                return new Stride.Core.Mathematics.BoundingBox();
            }

            if (this.normalized) {
                return new Stride.Core.Mathematics.BoundingBox(Stride.Core.Mathematics.Vector3.Zero, this.fullExtents.Value);
            } else {
                List<Vector3> vertices = this.RelativeVertices();
                Stride.Core.Mathematics.BoundingBox bounds = new Stride.Core.Mathematics.BoundingBox(vertices.First(), Stride.Core.Mathematics.Vector3.Zero);

                vertices.ForEach(v => BoundingBox.Merge(ref bounds, v, out bounds));
                return bounds;
            }
        }

        public override List<Vector3> RelativeVertices () {
            return this.constituents.SelectMany(c => c.shape.RelativeVertices().Select(v => c.transform.Transform(v))).ToList();
        }


        public override Box GetBoundingBox () {
            this.Normalize();
            return this.CalculateBounds();
        }

        public override Vector3 RandomPointWithin () {
            this.Normalize();
            return Alea.Item(this.constituents, c => c.externalVolume).shape.RandomPointWithin();
        }
    }

}