using F7s.Modell.Abstract;
using F7s.Modell.Economics.Facilities;
using F7s.Modell.Handling;
using F7s.Modell.Handling.PhysicalData;
using F7s.Modell.Handling.PlayerControllers;
using F7s.Modell.Physical.Localities;
using F7s.Utility;
using F7s.Utility.Geometry.Double;
using F7s.Utility.Measurements;
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
            if (!structures.Contains(structure)) {
                structures.Add(structure);
                structure.SetPhysicalEntity(this);
            }
        }

        public double DistanceFromSuperior () {
            return GetLocality().DistanceFromSuperior();
        }

        public double DistanceFromSuperiorBounds () {
            PhysicalEntity superior = GetLocality()?.HierarchySuperior()?.physicalEntity;
            if (superior == null) {
                throw new Exception();
            } else {
                return BoundsDistance(superior);
            }
        }

        public override void ConfigureInfoblock (Infoblock infoblock) {
            base.ConfigureInfoblock(infoblock);


            if (structures.Count > 0) {
                foreach (Structure structure in structures) {
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
            if (Locality) {
                throw new Exception(this + " already has a " + value.GetType().Name + " assigned.");
            } else {
                Locality?.Delete();
                Locality = value;
            }
        }

        public bool Touches (PhysicalEntity other, float bias = 0) {
            if (bias <= -(BoundingRadius() + other.BoundingRadius())) {
                throw new Exception("Excesive bias; contact impossible.");
            }
            return BoundsDistance(other) <= 0 - bias;
        }

        public override Locality GetLocality () {
            if (Locality == null) {
                throw new NullReferenceException();
            }
            return Locality;
        }

        public override int SubentityCount () {
            return base.SubentityCount() + SubEntities().Count();
        }

        public override List<GameEntity> SubEntities () {
            return PhysicalSubEntities().ConvertAll<GameEntity>(pe => pe);
        }

        public List<PhysicalEntity> PhysicalSubEntities () {
            return Locality?.HierarchySubordinates().Where(s => s.HasPhysicalEntity()).Select(s => s.physicalEntity).ToList();
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
            Quantity = quantity;
        }

        public static void TransferMass (PhysicalEntity source, PhysicalEntity target, double mass) {
            double drawn = source.AlterMass(-mass);
            target.AlterMass(drawn);
        }

        public virtual double AlterMass (double alteration) {
            double properMass = ProperMass();
            if (alteration <= -properMass) {
                // Consume entirely.
                SetQuantity(new Quantity(0));
                Delete();
                return properMass;
            } else {
                double resultingMass = properMass + alteration;
                SetQuantity(new Quantity(resultingMass));
                return -alteration;
            }
        }

        public override GameEntity UiHierarchyParent () {
            return GetLocality().HierarchySuperior()?.physicalEntity;
        }

        public virtual double BoundingRadius () {
            return 0;
        }

        public override Farbe RepresentativeColor () {
            return UiColor() ?? base.RepresentativeColor();
        }

        public virtual Vector3d Scale () {
            throw new Exception(GetType().Name);
        }

        public virtual double DistanceToPlayer () {
            return Player.TransformRelativeToPlayer(this).TranslationVector.Length();
        }

        public virtual double DistanceToCamera () {
            return Kamera.TransformRelativeToCamera(this).TranslationVector.Length();
        }

        public virtual double DistanceToOrigin () {
            return Origin.TransformRelativeToOrigin(this).TranslationVector.Length();
        }

        public double CenterDistance (PhysicalEntity other) {
            return Locality.DistanceTo(other);
        }

        public double BoundsDistance (PhysicalEntity other) {
            return CenterDistance(other) - BoundingRadius() - other.BoundingRadius();
        }


        public Vector3d RelativePosition (PhysicalEntity relativeTo) {
            return Locality.GetRelativeTransform(relativeTo).TranslationVector;
        }


        public override int UiHierarchyIndentationLevel () {
            return Locality.hierarchyDepth;
        }

        public string GetGeographicInformation () {
            return Measurement.MeasureLength(DistanceToCamera());
        }

        public double CollectiveMass () {
            List<PhysicalEntity> contents = ImmediateContents();
            double properMass = ProperMass();
            if (contents != null && contents.Count > 0) {
                return properMass + contents.Sum(c => c.CollectiveMass());
            } else {
                return properMass;
            }
        }

        public double ProperMass () {
            if (Quantity == null) {
                Quantity = CalculateFallbackQuantity();
            }
            if (Quantity == null) {
                return 0;
            }
            return Quantity.Mass;
        }

        protected virtual Quantity CalculateFallbackQuantity () {
            return null;
        }

        public List<PhysicalEntity> ImmediateContents () {
            return null;
        }

        public Vector3d CalculatePositionAtTime (double time) {
            throw new NotImplementedException();
        }

        public Vector3d RelativePosition (Locality relativeTo) {
            return Locality.GetRelativeTransform(relativeTo).TranslationVector;
        }

        public override PhysicalRepresentationData GetPhysicalData () {
            return new DirectPhysicalData(this);
        }

        public static implicit operator Locality (PhysicalEntity entity) {
            return entity?.Locality;
        }

        public virtual MatrixD ForcedPerspectiveTransform (float desiredDistanceFromCamera) {
            return Origin.ForcedPerspectiveTransform(GetLocality(), desiredDistanceFromCamera);
        }
        public MatrixD CameraCentricTransform () {
            return Kamera.TransformRelativeToCamera(GetLocality());
        }
        public MatrixD ForcedPerspectiveBaseTransform () {
            return Origin.ForcedProjectionBaseTransform(GetLocality());
        }

        public MatrixD OriginCentricTransform () {
            return Origin.TransformRelativeToOrigin(GetLocality());
        }
        public MatrixD PlayerCentricTransform () {
            return Player.TransformRelativeToPlayer(GetLocality());
        }
    }

}
