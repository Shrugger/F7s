using F7s.Modell.Abstract;
using F7s.Modell.Conceptual.Agents.GroupDistributions;
using F7s.Modell.Physical;
using F7s.Modell.Physical.Localities;
using System;
using System.Collections.Generic;

namespace F7s.Modell.Conceptual.Agents {

    public class Group : Agent, Hierarchical<Group> {

        // Groups are agents so that one needn't constantly generate individual agents to make group behavior happen. It's an abstraction. It may need to be handled with some care. The group may carry out its own roles until a suitable lower-level agent is found.

        private readonly List<Agent> members = new List<Agent>();
        private readonly GroupComposition composition;
        private GameValue<long> abstractMembersCount {
            get { return composition.abstractMembersCount; }
            set { composition.SetMemberCount(value); }
        }

        public override float WorkingPower () {
            throw new NotImplementedException();
        }

        public Group (string name, GroupComposition composition) : base(name) {
            this.composition = composition;
            GroupRoleManager roleManager = new GroupRoleManager(this);
            roleManager.SetAgent(this);
        }

        public Group EstablishSubgroup (string name, long memberCount) {
            if (abstractMembersCount <= memberCount) {
                throw new Exception();
            }
            GroupComposition groupComposition = composition.Copy();
            groupComposition.SetMemberCount(memberCount);
            return EstablishSubgroup(name, groupComposition);
        }
        public Group EstablishSubgroup (string name, GroupComposition composition) {
            abstractMembersCount -= composition.abstractMembersCount;
            Group subgroup = new Group(name, composition);
            subgroup.AssignToGroup(this);
            AddMember(subgroup);
            return subgroup;
        }

        public Agent ManifestMember (string name, Locality locality) {
            if (abstractMembersCount <= 0) {
                throw new Exception();
            }
            abstractMembersCount -= 1;
            Agent individualMember;
            if (composition.humans) {
                individualMember = new Human(name, locality);
            } else {
                throw new NotImplementedException();
            }
            AddMember(individualMember);
            individualMember.AddToGroup(this);
            return individualMember;
        }

        public override Locality GetLocality () {
            return composition.GetRepresentativeLocality();
        }

        public List<Role> UnassignedRoles () {
            throw new NotImplementedException();
        }

        public List<Role> RolesAssignedToGroup () {
            throw new NotImplementedException();
        }

        public List<Role> RolesAssignedToMember () {
            throw new NotImplementedException();
        }

        public void AddMember (Agent agent) {
            if (members.Contains(agent)) {
                throw new Exception();
            } else {
                members.Add(agent);
            }
        }

        public bool RemoveMember (Agent agent) {
            return members.Remove(agent);
        }

        public override void FoldIntoSupergroup () {
            throw new NotImplementedException();
        }

        public Group HierarchicalLowestCommonSuperior (Group other) {
            throw new NotImplementedException();
        }

        public bool HierarchyContains (Hierarchical<Group> item) {
            throw new NotImplementedException();
        }

        public Group HierarchyMember () {
            throw new NotImplementedException();
        }

        public Group HierarchyRoot () {
            throw new NotImplementedException();
        }

        public List<Group> HierarchySubordinates () {
            throw new NotImplementedException();
        }

        public Group HierarchySuperior () {
            throw new NotImplementedException();
        }

        public override void UnfoldAgent () {
            throw new NotImplementedException();
        }

        public override void UnfoldAllAgents () {
            throw new NotImplementedException();
        }

        Hierarchical<Group> Hierarchical<Group>.HierarchyRoot () {
            throw new NotImplementedException();
        }

        public override List<GameEntity> SubEntities () {
            return members.ConvertAll<GameEntity>(a => a);
        }
    }
}
