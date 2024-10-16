using F7s.Modell.Economics.Operations;
using F7s.Modell.Economics.Resources;
using F7s.Modell.Physical;

namespace F7s.Modell.Economics.Mining
{

    public class ResourceExtractionOperation : ResourceProductionOperation
    {

        private PhysicalEntity deposit;

        public ResourceExtractionOperation(ResourceType product, float efficiency = 0) : base(product, efficiency)
        {
        }

        public override bool Operational()
        {
            return deposit;
        }

        protected override float Produce(float effectiveWorkAccomplished)
        {
            float extraction = DiminishDeposits(effectiveWorkAccomplished);
            return extraction;
        }

        private float DiminishDeposits(float quantity)
        {
            float extraction = (float)deposit.AlterMass(-quantity);
            return extraction;
        }
    }
}
