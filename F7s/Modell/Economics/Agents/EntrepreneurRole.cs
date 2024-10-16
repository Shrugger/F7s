using System.Collections.Generic;
using F7s.Modell.Conceptual.Agents;
using F7s.Modell.Economics.Enterprises;

namespace F7s.Modell.Economics.Agents {
    public class EntrepreneurRole : Role
    {
        // Responsible for handling enterprises.
        // Founds, merges, splits, coordinates, adjusts and closes down enterprises depending on profitability and other ideas.
        // TODO: But for now, simply has the job of instating manager agents.
        private List<Enterprise> enterprises = new List<Enterprise>();

        public override void RunRole(double deltaTime)
        {
            foreach (Enterprise enterprise in enterprises)
            {
                if (enterprise.EnterpriseManagementRole == null)
                {
                    EnterpriseManagementRole manager = new EnterpriseManagementRole();
                    enterprise.InstallManager(manager);
                }
            }
        }
    }
}
