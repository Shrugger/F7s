using F7s.Modell.Abstract;
using F7s.Modell.Economics.Facilities;
using System;

namespace F7s.Modell.Conceptual.Agents
{


    public abstract class Role : GameEntity
    {
        public Agent Agent { get; private set; }
        public void SetAgent (Agent agent) {
            if (this.Agent == agent) {
                return;
            }
            if (this.Agent == null) {
                this.Agent = agent;
            }
            if (this.Agent != agent) {
                throw new Exception();
            }
        }
        public void TransferRoleToAgent (Agent agent) {
            if (agent == this.Agent) {
                throw new Exception();
            }
            if (agent == null) {
                throw new NullReferenceException();
            }
            if (this.Agent == null) {
                throw new NullReferenceException();
            }
            this.Agent.RemoveRole(this);
            this.Agent = agent;
            agent.AddRole(this);
        }
        public abstract void RunRole(double deltaTime);
    }
}
