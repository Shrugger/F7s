using F7s.Modell.Abstract;
using F7s.Modell.Economics.Facilities;
using F7s.Modell.Handling;
using F7s.Modell.Handling.PhysicalData;
using F7s.Modell.Handling.PlayerControllers;
using F7s.Modell.Physical.Localities;
using F7s.Utility;
using F7s.Utility.Geometry;
using F7s.Utility.Measurements;
using Stride.Core.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace F7s.Modell.Physical {

    public class PhysicalEntity : GameEntity {
        public PhysicalEntity (string name = null) : base(name) {
        }

        public Locality Locality { get; private set; }
        public Quantity Quantity { get; private set; }

        private readonly List<Structure> structures = new List<Structure>();



        public override void ConfigureContextMenu (ContextMenu contextMenu) {
            base.ConfigureContextMenu(contextMenu);
        }


        public void AddStructure (Structure structure) {
            if (!this.structures.Contains(structure)) {
                this.structures.Add(structure);
                structure.SetPhysicalEntity(this);
            }
        }

        public double DistanceFromSuperior () {
            return this.GetLocality().DistanceFromSuperior();
        }

        public double DistanceFromSuperiorBounds () {
            PhysicalEntity superior = this.GetLocality()?.HierarchySuperior()?.physicalEntity;
            if (superior == null) {
                throw new Exception();
            } else {
                return BoundsDistance(superior);
            }
        }

        public override void ConfigureInfoblock (Infoblock infoblock) {
            base.ConfigureInfoblock(infoblock);


            if (this.structures.Count > 0) {
                foreach (Structure structure in this.structures) {
                    infoblock.AddInformation(structure);
                }
            }

            {
                infoblock.AddInformation(Locality);
                infoblock.AddInformation(Quantity, "Quantity");
                infoblock.AddInformation("Distance " + GetGeographicInformation());
            }
        }


        public void SetLocality (Locality value) {
            if (this.Locality) {
                throw new Exception(this + " already has a " + value.GetType().Name + " assigned.");
            } else {
                this.Locality?.Delete();
                this.Locality = value;
            }
        }

        public bool Touches (PhysicalEntity other, float bias = 0) {
            if (bias <= -(this.BoundingRadius() + other.BoundingRadius())) {
                throw new Exception("Excesive bias; contact impossible.");
            }
            return this.BoundsDistance(other) <= 0 - bias;
        }

        public override Locality GetLocality () {
            if (this.Locality == null) {
                throw new NullReferenceException();
            }
            return this.Locality;
        }

        public override int SubentityCount () {
            return base.SubentityCount() + SubEntities().Count();
        }

        public override List<GameEntity> SubEntities () {
            return PhysicalSubEntities().ConvertAll<GameEntity>(pe => pe);
        }

        public List<PhysicalEntity> PhysicalSubEntities () {
            return this.Locality?.HierarchySubordinates().Where(s => s.HasPhysicalEntity()).Select(s => s.physicalEntity).ToList();
        }

        public List<PhysicalEntity> SubentitiesRecursive () {
            HashSet<PhysicalEntity> all = new HashSet<PhysicalEntity>();
            List<PhysicalEntity> pending = new List<PhysicalEntity>() { this };
            while (pending.Count > 0) {
                PhysicalEntity next = pending.Last();
                pending.Remove(next);
                pending.AddRange(next.PhysicalSubEntities());
                all.Add(next);
            }
            return all.ToList();
        }

        public virtual void SetQuantity (Quantity quantity) {
            this.Quantity = quantity;
        }

        public static void TransferMass (PhysicalEntity source, PhysicalEntity target, double mass) {
            double drawn = source.AlterMass(-mass);
            target.AlterMass(drawn);
        }

        public virtual double AlterMass (double alteration) {
            double properMass = this.ProperMass();
            if (alteration <= -properMass) {
                // Consume entirely.
                this.SetQuantity(new Quantity(0));
                this.Delete();
                return properMass;
            } else {
                double resultingMass = properMass + alteration;
                this.SetQuantity(new Quantity(resultingMass));
                return -alteration;
            }
        }

        public override GameEntity UiHierarchyParent () {
            return this.GetLocality().HierarchySuperior()?.physicalEntity;
        }

        public virtual float BoundingRadius () {
            return 0;
        }

        public override Farbe RepresentativeColor () {
            return this.UiColor() ?? base.RepresentativeColor();
        }

        public virtual Vector3 Scale () {
            throw new Exception(this.GetType().Name);
        }

        public virtual double DistanceToPlayer () {
            return Player.TransformRelativeToPlayer(this).Origin.Length();
        }

        public virtual double DistanceToCamera () {
            return Kamera.TransformRelativeToCamera(this).Origin.Length();
        }

        public virtual double DistanceToOrigin () {
            return Origin.TransformRelativeToOrigin(this).Origin.Length();
        }

        public double CenterDistance (PhysicalEntity other) {
            return this.Locality.DistanceTo(other);
        }

        public double BoundsDistance (PhysicalEntity other) {
            return this.CenterDistance(other) - this.BoundingRadius() - other.BoundingRadius();
        }


        public Vector3d RelativePosition (PhysicalEntity relativeTo) {
            return this.Locality.GetRelativeTransform(relativeTo).Origin;
        }


        public override int UiHierarchyIndentationLevel () {
            return this.Locality.hierarchyDepth;
        }

        public string GetGeographicInformation () {
            return Measurement.MeasureLength(this.DistanceToCamera());
        }

        public double CollectiveMass () {
            List<PhysicalEntity> contents = this.ImmediateContents();
            double properMass = this.ProperMass();
            if (contents != null && contents.Count > 0) {
                return properMass + contents.Sum(c => c.CollectiveMass());
            } else {
                return properMass;
            }
        }

        public double ProperMass () {
            if (this.Quantity == null) {
                this.Quantity = this.CalculateFallbackQuantity();
            }
            if (this.Quantity == null) {
                return 0;
            }
            return this.Quantity.Mass;
        }

        protected virtual Quantity CalculateFallbackQuantity () {
            return null;
        }

        public List<PhysicalEntity> ImmediateContents () {
            return null;
        }

        public Vector3 CalculatePositionAtTime (double time) {
            throw new NotImplementedException();
        }

        public Vector3d RelativePosition (Locality relativeTo) {
            return this.Locality.GetRelativeTransform(relativeTo).Origin;
        }

        public override PhysicalRepresentationData GetPhysicalData () {
            return new DirectPhysicalData(this);
        }

        public static implicit operator Locality (PhysicalEntity entity) {
            return entity?.Locality;
        }

        public virtual Transform3D ForcedPerspectiveTransform (float desiredDistanceFromCamera) {
            return Origin.ForcedPerspectiveTransform(GetLocality(), desiredDistanceFromCamera);
        }
        public Transform3D CameraCentricTransform () {
            return Kamera.TransformRelativeToCamera(GetLocality());
        }
        public Transform3D ForcedPerspectiveBaseTransform () {
            return Origin.ForcedProjectionBaseTransform(GetLocality());
        }

        public Transform3D OriginCentricTransform () {
            return Origin.TransformRelativeToOrigin(GetLocality());
        }
        public Transform3D PlayerCentricTransform () {
            return Player.TransformRelativeToPlayer(GetLocality());
        }
    }

}
