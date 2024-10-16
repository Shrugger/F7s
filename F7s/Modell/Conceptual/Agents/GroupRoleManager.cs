using F7s.Modell.Conceptual.Agents;
using System;

namespace F7s.Modell.Conceptual {
    public class GroupRoleManager : Role {
        public Group Group { get; private set; }

        public GroupRoleManager (Group group) {
            Group = group;
        }

        public override void RunRole (double deltaTime) {
            System.Collections.Generic.List<Role> unassignedRoles = this.Group.UnassignedRoles();
            foreach (Role role in unassignedRoles) {
                role.SetAgent(this.Group);
            }
            // TODO: Determine when to assign unassigned roles to qualified group members instead of the group as a whole, and when to reassign roles.
        }
    }
}
