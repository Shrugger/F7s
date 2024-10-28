using F7s.Modell.Abstract;
using System;

namespace F7s.Modell.Conceptual.Agents.Roles {

    public abstract class Role : GameEntity {
        public Agent Agent { get; private set; }
        public void SetAgent (Agent agent) {
            if (Agent == agent) {
                return;
            }
            if (Agent == null) {
                Agent = agent;
            }
            if (Agent != agent) {
                throw new Exception();
            }
        }
        public void TransferRoleToAgent (Agent agent) {
            if (agent == Agent) {
                throw new Exception();
            }
            if (agent == null) {
                throw new NullReferenceException();
            }
            if (Agent == null) {
                throw new NullReferenceException();
            }
            Agent.RemoveRole(this);
            Agent = agent;
            agent.AddRole(this);
        }
        public abstract void RunRole (double deltaTime);
    }
}
