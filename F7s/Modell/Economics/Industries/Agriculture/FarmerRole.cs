using F7s.Modell.Economics.Operations;
using F7s.Modell.Physical.Bodies;
using F7s.Modell.Physical.Localities;

namespace F7s.Modell.Economics.Agriculture;

public class FarmerRole : ResourceProductionOperatorRole {
    protected override ResourceProductionOperation BootstrapOperationAndFacilities (Locality locality) {
        FarmingOperations farmingOperations = new FarmingOperations(1, 0.5f);
        Farm farm = new Farm();
        Body farmBody = new Body("Farm", new Godot.Vector3(100, 0.5f, 100), new Godot.Color(0.5f, 1.0f, 0.0f, 1.0f));
        farm.SetPhysicalEntity(farmBody);

        Fixed.FixedSibling(farmBody, locality);

        return farmingOperations;
    }

}
