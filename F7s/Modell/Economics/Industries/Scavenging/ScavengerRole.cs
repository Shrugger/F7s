using F7s.Modell.Economics.Agriculture;
using F7s.Modell.Economics.Operations;
using F7s.Modell.Physical.Localities;

namespace F7s.Modell.Economics.Scavenging;


public class ScavengerRole : ResourceProductionOperatorRole {
    protected override ResourceProductionOperation BootstrapOperationAndFacilities (Locality locality) {
        return new ScavengingOperations(1, 1);
    }
}
