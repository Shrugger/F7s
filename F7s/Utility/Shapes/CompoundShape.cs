using Stride.Core.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace F7s.Utility.Shapes {


    public class CompoundShape : Shape3Dim {

        public class Constituent {

            public readonly Shape3Dim shape;
            public Matrix transform;
            public readonly string name;
            public readonly float externalVolume;
            public readonly float substantialVolume;
            public Farbe? Color;

            public Vector3 position { get { return transform.TranslationVector; } set { transform.TranslationVector = value; } }
            public Quaternion rotation => Mathematik.ExtractRotation(transform);

            public Constituent (Shape3Dim shape, Vector3 position, Vector3? rotation = null, string name = null, Farbe? color = null)
                : this(shape, position, rotation != null ? Mathematik.DegreesToQuaternion(rotation.Value) : null, name, color) { }
            public Constituent (Shape3Dim shape, Vector3 position, Quaternion? rotation = null, string name = null, Farbe? color = null) {
                this.shape = shape;
                transform = new Matrix();
                Matrix.Transformation(Vector3.One, Quaternion.Identity, position, out transform);
                this.name = name;
                externalVolume = shape.ExternalVolume();
                substantialVolume = shape.SubstantialVolume();
                Color = color;
            }

            public void Paint (Farbe color) {
                Color = color;
            }


            public override string ToString () {
                return name;
            }

            public void Normalize () {
                shape.Normalize();
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

            Normalize();
        }

        public void ApplyToConstituents (Action<Constituent> action) {
            constituents.ForEach(c => action.Invoke(c));
        }
        public Constituent AddConstituent (Shape3Dim shape, Vector3 relativePosition, Vector3 relativeRotation, string name, Farbe? color = null) {
            return AddConstituent(new Constituent(shape, relativePosition, Mathematik.DegreesToQuaternion(relativeRotation), name, color));
        }
        public Constituent AddConstituent (Shape3Dim shape, Vector3 relativePosition, Quaternion relativeRotation, string name, Farbe? color = null) {
            return AddConstituent(new Constituent(shape, relativePosition, relativeRotation, name, color));
        }


        public Constituent AddConstituent (Constituent constituent) {
            constituents.Add(constituent);
            normalized = false;
            return constituent;
        }


        public override bool IsConvex () {
            return false;
        }

        public override Shape3Dim GetShapePlusSize (float addition) {
            throw new NotImplementedException("GetShapePlusSize is not implemented for type " + GetType() + ".");
        }

        public override float ProfileArea (Vector3 angle) {
            throw new NotImplementedException();
        }

        public override ShapeType ShapeType () {
            return F7s.Utility.Shapes.ShapeType.Compound;
        }

        public override float SubstantialVolume () {
            return constituents.Sum(d => d.shape.SubstantialVolume());
        }

        public override float SurfaceArea () {
            return constituents.Sum(d => d.shape.SurfaceArea());
        }

        public override Vector3 FullExtents () {
            Normalize();
            return fullExtents.Value;
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
                foreach (Constituent constituent in constituents) {
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
                return Shape3Dim.Intersect(GetBoundingBox(),
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

            if (constituents != null && constituents.Count > 0) {

                constituents.ForEach(c => c.Normalize());

                if (!normalized) {

                    Stride.Core.Mathematics.BoundingBox bb = CalculateBounds();

                    fullExtents = fullExtents ?? bb.Extent;

                    Vector3 offset = bb.Center;

                    constituents.ForEach(c => c.position -= offset);

                    if (bb.Extent != CalculateBounds().Extent) {
                        throw new Exception(this + " bounds (" + bb + ") -> (" + CalculateBounds() + ").");
                    }

                    normalized = true;
                }
            }

            base.Normalize();
        }


        public Stride.Core.Mathematics.BoundingBox CalculateBounds () {
            if (constituents == null || constituents.Count == 0) {
                return new Stride.Core.Mathematics.BoundingBox();
            }

            if (normalized) {
                return new Stride.Core.Mathematics.BoundingBox(Stride.Core.Mathematics.Vector3.Zero, fullExtents.Value);
            } else {
                List<Vector3> vertices = RelativeVertices();
                Stride.Core.Mathematics.BoundingBox bounds = new Stride.Core.Mathematics.BoundingBox(vertices.First(), Stride.Core.Mathematics.Vector3.Zero);

                vertices.ForEach(v => BoundingBox.Merge(ref bounds, v, out bounds));
                return bounds;
            }
        }

        public override List<Vector3> RelativeVertices () {
            return constituents.SelectMany(c => c.shape.RelativeVertices().Select(v => c.transform * v)).ToList(); // TODO: The Transform(Vector) method in this case is the matrix * vector multiplication.
        }


        public override Box GetBoundingBox () {
            Normalize();
            return CalculateBounds();
        }

        public override Vector3 RandomPointWithin () {
            Normalize();
            return Alea.Item(constituents, c => c.externalVolume).shape.RandomPointWithin();
        }
    }

}