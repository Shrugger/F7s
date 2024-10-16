using F7s.Modell.Economics.Resources;

namespace F7s.Modell.Economics.Operations
{
    public class SustainableProductionOperation : ResourceProductionOperation
    {
        public SustainableProductionOperation(ResourceType product, float efficiency = 0) : base(product, efficiency)
        {
        }

        protected override float Produce(float effectiveWorkAccomplished)
        {
            return effectiveWorkAccomplished;
        }
    }
}
