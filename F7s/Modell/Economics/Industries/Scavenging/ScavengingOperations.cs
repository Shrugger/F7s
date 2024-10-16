using F7s.Modell.Economics.Operations;
using F7s.Modell.Economics.Resources;
using F7s.Utility;

namespace F7s.Modell.Economics.Agriculture;

public class ScavengingOperations : SustainableProductionOperation {
    public float Volatility { get; private set; }
    public ScavengingOperations (float efficiency = 0,
                                 float volatility = 1) : base(ResourceType.LifeSupportNecessities, efficiency) {
        this.Volatility = volatility;
    }

    protected override float Produce (float effectiveWorkAccomplished) {
        return base.Produce(effectiveWorkAccomplished) * Alea.Float(1 - Volatility, 1 + Volatility);
    }
}
