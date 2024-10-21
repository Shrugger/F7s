using F7s.Modell.Abstract;
using F7s.Modell.Conceptual.Agents;
using F7s.Modell.Conceptual.Cultures;
using F7s.Modell.Economics.Resources;
using F7s.Modell.Handling;
using System;
using F7s.Utility.Geometry.Double;
using Stride.Core.Mathematics;
using System.Collections.Generic;

namespace F7s.Modell.Conceptual {

    public struct SkillValue : IGameValue {

        public readonly SkillType SkillType;
        public readonly float Value;

        public SkillValue (SkillType skillType, float value) {
            this.SkillType = skillType;
            Value = value;
        }

        public static implicit operator SkillValue ((SkillType, float) typeAndValue) {
            return new SkillValue(typeAndValue.Item1, typeAndValue.Item2);
        }

        public void ConfigureContextMenu (Handling.ContextMenu contextMenu) {
            throw new NotImplementedException();
        }

        public void ConfigureInfoblock (Infoblock infoblock) {
            throw new NotImplementedException();
        }
    }

    public class SkillType : AbstractGameValue {

        public static readonly SkillType Manual = new SkillType("Manual");
        public static readonly SkillType Mental = new SkillType("Mental");
        public static readonly SkillType Martial = new SkillType("Martial");

        public readonly string name;

        public SkillType (string name) {
            this.name = name;
        }
    }

    public class WorkType : AbstractGameValue {


        public static readonly WorkType Farmhand = new WorkType("Farmhand", (SkillType.Manual, 1.0f));
        public static readonly WorkType Trader = new WorkType("Trader", (SkillType.Mental, 1.0f));
        public static readonly WorkType Fighter = new WorkType("Fighter", (SkillType.Martial, 1.0f));

        public readonly string name;

        public enum WorkBaseTypes { Undefined, Manual, Mental, Martial }
        private readonly List<SkillValue> skills = new List<SkillValue>();

        public WorkType (string name, params SkillValue[] skills) {
            this.name = name;
            this.skills.AddRange(skills);
        }

        public override string ToString () {
            return this.name;
        }
    }

    public abstract class Agent : GameEntity {
        private readonly List<Role> assignedRoles = new List<Role>();
        private Culture culture;
        private readonly List<Group> groupMemberships = new List<Group>();
        private readonly List<Institution> institutions = new List<Institution>();

        protected readonly ResourcePool possessions = new ResourcePool();
        protected readonly ResourcePool constantDemands = new ResourcePool();

        protected Agent (string name) : base(name) {
            this.GenerateRoles().ForEach(r => this.AddRole(r));
        }

        public abstract float WorkingPower ();

        public virtual ResourcePool AllDemands () {
            return UnmetConstantDemands();
        }

        public virtual ResourcePool UnmetConstantDemands () {
            return ConstantDemands() - Possessions();
        }

        public virtual ResourcePool ConstantDemands () {
            return this.constantDemands;
        }

        public virtual ResourcePool Possessions () {
            return this.possessions;
        }

        public bool Possesses (Resource other) {
            return this.possessions.Matches(other);
        }

        public void AddToGroup (Group group) {
            this.groupMemberships.Add(group);
        }

        public override string ToString () {
            return base.ToString();
        }

        public void SetCulture (Culture culture) {
            this.culture = culture;
        }

        protected virtual List<Role> GenerateRoles () {
            return new List<Role>();
        }

        public void InstituteInstitution (Institution institution) {
            this.institutions.Add(institution);
            institution.Roles.ForEach(r => this.AddRole(r));
        }

        public void AssignToGroup (Group group) {
            if (this.groupMemberships.Contains(group)) {
                throw new Exception();
            }
            this.groupMemberships.Add(group);
        }
        public bool RemoveFromGroup (Group group) {
            return this.groupMemberships.Remove(group);
        }

        public void AddRole (Role role) {
            if (this.assignedRoles.Contains(role)) {
                throw new Exception();
            }
            this.assignedRoles.Add(role);
            if (role.Agent != this) {
                if (role.Agent == null) {
                    role.SetAgent(this);
                } else {
                    throw new Exception();
                }
            }
        }
        public bool RemoveRole (Role role) {
            return this.assignedRoles.Remove(role);
        }
        public abstract void FoldIntoSupergroup ();
        public abstract void UnfoldAgent ();
        public abstract void UnfoldAllAgents ();

        public override bool Pop () {
            this.UnfoldAllAgents();
            this.Delete();
            return base.Pop();
        }

        public override bool Zip () {
            this.FoldIntoSupergroup();
            this.Delete();
            return base.Zip();
        }

        protected override void Update (double deltaTime) {
            base.Update(deltaTime);

            this.assignedRoles.ForEach(r => r.RunRole(deltaTime));
        }
    }
}
