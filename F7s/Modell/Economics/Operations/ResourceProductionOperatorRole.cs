using F7s.Modell.Conceptual.Agents;
using F7s.Modell.Physical.Localities;

namespace F7s.Modell.Economics.Operations {
    public abstract class ResourceProductionOperatorRole : Role {
        public ResourceProductionOperation ProductiveOperation { get; private set; }

        protected virtual ResourceProductionOperation BootstrapOperationAndFacilities (Locality locality) {
            return null;
        }

        public void SetOperation (ResourceProductionOperation operation) {
            this.ProductiveOperation = operation;
        }

        public override void RunRole (double deltaTime) {
            if (this.ProductiveOperation == null) {
                this.ProductiveOperation = BootstrapOperationAndFacilities(this.Agent.GetLocality());
            }
            ProduceResource(deltaTime);
        }

        protected virtual void ProduceResource (double deltaTime) {
            ProductiveOperation.Operate(deltaTime);

        }
    }
}
