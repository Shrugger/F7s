using F7s.Modell.Economics.Operations;
using F7s.Modell.Economics.Resources;
using F7s.Utility;
using System;

namespace F7s.Modell.Economics.Agriculture;

public class FarmingOperations : SustainableProductionOperation {
    public float Reliability { get; private set; }

    public FarmingOperations ( float efficiency = 0, float reliability = 1) : base(ResourceType.LifeSupportNecessities, efficiency) {
        this.Reliability = reliability;
    }

    protected override float Produce (float effectiveWorkAccomplished) {
        return base.Produce(effectiveWorkAccomplished) * Alea.Maybe(Reliability);
    }
}
