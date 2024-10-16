using System.Collections.Generic;
using F7s.Modell.Conceptual.Agents;
using F7s.Modell.Economics.Facilities;

namespace F7s.Modell.Economics.Agents {
    public class EnterpriseManagementRole : Role
    {
        // Responsible for handling a single enterprise. Coordinates facilities and trade to maximize profits.
        private List<Facility> facilities = new List<Facility>();

        public override void RunRole(double deltaTime)
        {
            foreach (Facility facility in facilities)
            {
                if (facility.FacilityOperationsManager == null)
                {
                    FacilityOperationsRole manager = new FacilityOperationsRole();
                    facility.InstallManager(manager);
                }
            }
        }
    }
}
