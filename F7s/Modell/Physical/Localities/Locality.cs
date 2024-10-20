﻿using F7s.Engine;
using F7s.Modell.Abstract;
using F7s.Modell.Handling.PlayerControllers;
using F7s.Utility;
using F7s.Utility.Geometry;
using F7s.Utility.Geometry.Double;
using F7s.Utility.Lazies;
using Stride.Core.Mathematics;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace F7s.Modell.Physical.Localities {


    public abstract class Locality : AbstractGameValue, Hierarchical<Locality> {

        public string Name { get; set; }

        public static double TimeFactor = 1;

        private readonly LazyFrameMonoMemory<Locality, Transform3D> cachedRelativeGeometry;

        private List<Locality> hierarchySubordinates = new List<Locality>();

        private int hierarchyDepthBackingField = -1;
        public int hierarchyDepth {
            get {
                if (hierarchyDepthBackingField == -1) {
                    RecalulcateHierarchyDepth();
                    if (hierarchyDepthBackingField == -1) {
                        throw new Exception();
                    }
                }
                return hierarchyDepthBackingField;
            }
            set {
                hierarchyDepthBackingField = value;
            }
        }
        public bool Obsolete { get; private set; } = false;
        public PhysicalEntity physicalEntity { get; private set; }
        protected Locality (PhysicalEntity entity, Locality anchor) {
            anchor?.Validate();
            if (entity != null) {
                SetPhysicalEntity(entity);
            }
            cachedRelativeGeometry = new LazyFrameMonoMemory<Locality, Transform3D>(CalculateRelativeTransform);
            anchor?.RegisterHierarchySubordinate(this);
        }

        public virtual Locality Sibling (PhysicalEntity entity) {
            throw new NotImplementedException(GetType().Name);
        }

        public void SetPhysicalEntity (PhysicalEntity physicalEntity, bool forceReplace = false) {
            if (!forceReplace && this.physicalEntity != null) {
                throw new Exception();
            }
            if (this.physicalEntity == physicalEntity) {
                throw new Exception();
            }
            this.physicalEntity = physicalEntity;
            physicalEntity.SetLocality(this);
        }

        public enum Visualizabilities { Undefined, Static, Mobile, Cyclical }
        public virtual Visualizabilities Visualizability () {
            return Visualizabilities.Undefined;
        }

        public virtual List<Locality> PredictedItinerary (int legs, Locality parent, double? duration = null) {
            throw new NotImplementedException();
        }

        public virtual void Validate () {
            if (Obsolete) {
                throw new Exception(this + " is obsolete and cannot be validated.");
            }
        }

        public static void AssertEquivalence (Locality a, Locality b, float delta = 0.1f) {
            Debug.Assert(AbsolutelyEqual(a, b, delta), "\n" + a.GetAbsoluteTransform() + "\n" + " != " + "\n" + b.GetAbsoluteTransform());
        }
        public static bool AbsolutelyEqual (Locality a, Locality b, float delta = 0.1f) {
            return Mathematik.ApproximatelyEqual(a.GetAbsoluteTransform(), b.GetAbsoluteTransform(), delta);
        }

        public virtual bool InheritsRotation () {
            return true;
        }

        public virtual void Update (double deltaTime) {
            Validate();
            // Note that this is NOT called automatically.
        }

        public static double GetLocalityTime () {
            return Zeit.GetSimulationDateSeconds() * TimeFactor;
        }

        public virtual double GetTime () {
            return Locality.GetLocalityTime();
        }

        public override string ToString () {
            if (Name != null) {
                return Name;
            } else {
                return "[" +
                    (Obsolete ? "DELETED " : "") +
                    (Name != null ? Name + " " : "") +
                    (physicalEntity ? physicalEntity.ToString() + " " : "") +
                    GetType().Name +
                    (hierarchyDepthBackingField > 0 ? " D" + hierarchyDepthBackingField : "") +
                    "]";
            }
        }
        private void RecalulcateHierarchyDepth (Locality superior = null) {
            Validate();
            superior ??= HierarchySuperior();

            int depth;
            if (superior != null) {
                depth = superior.hierarchyDepth + 1;
            } else {
                depth = 0;
            }
            if (depth != hierarchyDepthBackingField) {
                hierarchyDepth = depth;
                HierarchySubordinates().ForEach(s => s.RecalulcateHierarchyDepth());
            }
        }

        public void Replace (Locality replacement) {
            Validate();
            Locality superior = HierarchySuperior();
            superior.RegisterHierarchySubordinate(replacement);
            List<Locality> subordinates = HierarchySubordinates();
            subordinates.ForEach(sub => sub.ReplaceSuperior(replacement));
            Delete();
        }

        protected virtual void ReplaceSuperior (Locality replacement) {
            Validate();
            MarkDirty();
            replacement.RegisterHierarchySubordinate(this);
        }

        public Transform3D GetAbsoluteTransform () {
            Validate();
            return Origin.TransformRelativeToOrigin(this);
        }

        public Transform3D GetRelativeTransform (Locality relativeTo) {
            Debug.Assert(null != relativeTo);
            Validate();
            if (relativeTo == this) {
                return Transform3D.Identity;
            } else {
                Transform3D transform3D = CalculateRelativeTransform(relativeTo); // TODO: REMOVE
                // TODO: REACTIVATE Transform3D transform3D = this.cachedRelativeMathematik.GetValue(relativeTo);
                if (Mathematik.InvalidPositional(transform3D)) {
                    throw new Exception();
                }
                return transform3D;
            }
        }

        protected Transform3D CalculateRelativeTransform (Locality relativeTo) {
            Debug.Assert(null != relativeTo);
            Validate();
            if (relativeTo == this) {
                throw new Exception(this + " == " + relativeTo);
            }

            Locality commonRoot = StaticHierarchicalLowestCommonSuperior(this, relativeTo);

            if (commonRoot == null) {
                throw new Exception("No common root between " + this + " and " + relativeTo + ".");
            }

            Transform3D absoluteSelf = CalculateRelativeTransformUpToRoot(commonRoot);
            Transform3D absoluteOther = relativeTo.CalculateRelativeTransformUpToRoot(commonRoot);

            Transform3D result = absoluteOther.Inverse() * absoluteSelf;
            result.Basis = result.Basis.Orthonormalized();

            if (Mathematik.InvalidPositional(result)) {
                throw new Exception("Invalid relative transform of " + this + " to " + relativeTo + ": " + result);
            }

            return result;
        }

        private List<Locality> GetDescendingInheritance (Locality root) {
            Validate();
            List<Locality> members = new List<Locality>();
            members.Add(this);
            Locality superior = HierarchySuperior();
            while (superior != null && superior.hierarchyDepth > root.hierarchyDepth) {
                members.Add(superior);
                Locality nextSuperior = superior.HierarchySuperior();
                superior = nextSuperior;
            }
            members.Reverse();
            return members;
        }

        private Transform3D CalculateRelativeTransformUpToRoot (Locality root) {
            Validate();

            if (this == root) {
                return Transform3D.Identity;
            }

            Transform3D total = default;
            bool initialized = false;

            List<Locality> descendingInheritance = GetDescendingInheritance(root);

            foreach (Locality child in descendingInheritance) {
                Transform3D local = child.GetLocalTransform();
                if (Mathematik.InvalidPositional(local)) {
                    throw new Exception();
                }
                if (!initialized) {
                    total = local;
                    initialized = true;
                } else {
                    bool inheritRotation = child.InheritsRotation();
                    Transform3D result;
                    if (inheritRotation) {
                        result = total * local;
                    } else {
                        result = local.Translated(total.Origin);
                    }
                    result.Basis = result.Basis.Orthonormalized();
                    if (Mathematik.InvalidPositional(result)) {
                        throw new Exception("Scale " + result.Basis.Scale + ". Whole Transform: " + result);
                    }
                    total = result;
                }
            }

            return total;
        }

        public abstract Transform3D GetLocalTransform ();

        protected Transform3D GetTransformRelativeToParent () {
            Validate();
            return GetLocalTransform();
        }

        protected void UncacheTransform () {
            cachedRelativeGeometry.MarkAsDirty();
        }

        public Locality HierarchyMember () {
            Validate();
            return this;
        }

        public abstract Locality HierarchySuperior ();

        public List<Locality> HierarchySubordinates () {
            Validate();
            return hierarchySubordinates;
        }

        public void RegisterHierarchySubordinate (Locality subordinate) {
            Validate();
            if (!HierarchySubordinates().Contains(subordinate)) {
                hierarchySubordinates.Add(subordinate);
                subordinate.MarkDirty();
            }
        }

        private void DeregisterSubordinate (Locality subordinate) {
            Validate();
            if (!HierarchySubordinates().Contains(subordinate)) {
                hierarchySubordinates.Remove(subordinate);
            }
        }


        public Hierarchical<Locality> HierarchyRoot () {
            Validate();
            if (HierarchySuperior() != null) {
                return HierarchySuperior().HierarchyRoot();
            } else {
                return this;
            }
        }

        public Locality HierarchicalLowestCommonSuperior (Locality other) {
            return StaticHierarchicalLowestCommonSuperior(this, other);
        }
        public static Locality StaticHierarchicalLowestCommonSuperior (Locality realNode1, Locality realNode2) {
            Debug.Assert(null != realNode1);
            Debug.Assert(null != realNode2);
            realNode1.Validate();
            realNode2.Validate();
            Locality node1 = realNode1;
            Locality node2 = realNode2;
            int deepest = Math.Max(node1.hierarchyDepth, node2.hierarchyDepth);
            for (int i = deepest; i >= 0; i--) {
                if (node1 == node2) {
                    return node1;
                }
                if (node1.hierarchyDepth == i) {
                    Locality superior1 = node1.HierarchySuperior();
                    Debug.Assert(null != superior1);
                    Debug.Assert(i > superior1.hierarchyDepth);
                    node1 = superior1;
                }
                if (node2.hierarchyDepth == i) {
                    Locality superior2 = node2.HierarchySuperior();
                    Debug.Assert(null != superior2);
                    Debug.Assert(i > superior2.hierarchyDepth);
                    node2 = superior2;
                }
            }

            if (realNode1.SharesHierarchy(realNode2)) {
                throw new Exception(realNode1 + " and " + realNode2 + " share the same root (" + realNode2.HierarchyRoot() + ") but cannot find a common superior.");
            } else {
                throw new Exception("Apparently " + realNode1 + " (root " + realNode1.HierarchyRoot() + ") and " + realNode2 + " (root " + realNode2.HierarchyRoot() + ") do not exist in the same hierarchy.");
            }
        }

        public bool SharesHierarchy (Locality other) {
            return HierarchyRoot() == other.HierarchyRoot();
        }

        public double DistanceTo (Locality locality) {
            Validate();
            Vector3d relativePosition = GetRelativeTransform(locality).Origin;
            double distance = relativePosition.Length();

            if (double.IsFinite(distance)) {
                return distance;
            } else {
                throw new Exception("Nonfinite distance between " + this + " and " + locality + " = " + distance);
            }
        }

        public void Delete () {
            Validate();
            Obsolete = true;
            physicalEntity = null;
            HierarchySuperior().DeregisterSubordinate(this);
            hierarchySubordinates.Clear();
            hierarchySubordinates = null;
            hierarchyDepthBackingField = -1;
        }
        protected void MarkDirty () {
            hierarchyDepthBackingField = -1;
        }

        public virtual void SetTransform (Transform3D value) {
            throw new NotImplementedException("Not implemented for type " + GetType().Name + ".");
        }

        public virtual void Translate (Vector3 relativeOffset) {
            throw new NotImplementedException("Not implemented for type " + GetType().Name + ".");
        }

        public virtual void Rotate (float yaw, float pitch, float roll = 0) {
            throw new NotImplementedException("Not implemented for type " + GetType().Name + ".");
        }

        public virtual void RotateEcliptic (float yaw, float pitch, float roll = 0) {
            throw new NotImplementedException("Not implemented for type " + GetType().Name + ".");
        }

        public virtual void AddLocalVelocity (Vector3 forceVector) {
            throw new NotImplementedException("Not implemented for type " + GetType().Name + ".");
        }

        public virtual void SetLocalVelocity (Vector3 forceVector) {
            throw new NotImplementedException("Not implemented for type " + GetType().Name + ".");
        }

        public virtual Vector3 GetLocalVelocity () {
            throw new NotImplementedException("Not implemented for type " + GetType().Name + ".");
        }

        public bool HasPhysicalEntity () {
            return physicalEntity != null;
        }

        public double DistanceFromSuperior () {
            if (HierarchySuperior() == null) {
                return 0;
            }
            return DistanceTo(HierarchySuperior());
        }
    }
}