using F7s.Modell.Abstract;
using F7s.Modell.Conceptual.Agents.Institutions;
using F7s.Modell.Conceptual.Agents.Roles;
using F7s.Modell.Conceptual.Cultures;
using F7s.Modell.Economics.Resources;
using System;
using System.Collections.Generic;

namespace F7s.Modell.Conceptual.Agents {

    public abstract class Agent : GameEntity {

        private readonly Role fallbackRole;
        private readonly List<Role> assignedRoles = new List<Role>();
        private Culture culture;
        private readonly List<Group> groupMemberships = new List<Group>();
        private readonly List<Institution> institutions = new List<Institution>();

        protected readonly ResourcePool possessions = new ResourcePool();
        protected readonly ResourcePool constantDemands = new ResourcePool();

        protected Agent (string name) : base(name) {
            GenerateRoles().ForEach(r => AddRole(r));
        }

        public abstract float WorkingPower ();

        public virtual ResourcePool AllDemands () {
            return UnmetConstantDemands();
        }

        public virtual ResourcePool UnmetConstantDemands () {
            return ConstantDemands() - Possessions();
        }

        public virtual ResourcePool ConstantDemands () {
            return constantDemands;
        }

        public virtual ResourcePool Possessions () {
            return possessions;
        }

        public bool Possesses (Resource other) {
            return possessions.Matches(other);
        }

        public void AddToGroup (Group group) {
            groupMemberships.Add(group);
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
            institutions.Add(institution);
            institution.Roles.ForEach(r => AddRole(r));
        }

        public void AssignToGroup (Group group) {
            if (groupMemberships.Contains(group)) {
                throw new Exception();
            }
            groupMemberships.Add(group);
        }
        public bool RemoveFromGroup (Group group) {
            return groupMemberships.Remove(group);
        }

        public void AddRole (Role role) {
            if (assignedRoles.Contains(role)) {
                throw new Exception();
            }
            assignedRoles.Add(role);
            if (role.Agent != this) {
                if (role.Agent == null) {
                    role.SetAgent(this);
                } else {
                    throw new Exception();
                }
            }
        }
        public bool RemoveRole (Role role) {
            return assignedRoles.Remove(role);
        }
        public abstract void FoldIntoSupergroup ();
        public abstract void UnfoldAgent ();
        public abstract void UnfoldAllAgents ();

        public override bool Pop () {
            UnfoldAllAgents();
            Delete();
            return base.Pop();
        }

        public override bool Zip () {
            FoldIntoSupergroup();
            Delete();
            return base.Zip();
        }

        protected override void Update (double deltaTime) {
            base.Update(deltaTime);

            if (assignedRoles.Count == 0) {
                fallbackRole?.RunRole(deltaTime);
            }

            assignedRoles.ForEach(r => r.RunRole(deltaTime));
        }
    }
}
