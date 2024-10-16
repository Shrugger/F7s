using F7s.Modell.Conceptual.Agents;
using F7s.Modell.Economics.Institutions;
using F7s.Modell.Economics.Operations;

namespace F7s.Modell.Economics.Agriculture;

public class SubsistenceFarming : SubsistenceWorkerTraderInstitution {
    protected override ResourceProductionOperatorRole WorkRole () {
        return new FarmerRole();
    }
}
