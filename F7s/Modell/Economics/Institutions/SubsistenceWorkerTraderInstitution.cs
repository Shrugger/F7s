using F7s.Modell.Conceptual.Agents.Institutions;
using F7s.Modell.Conceptual.Agents.Roles;
using F7s.Modell.Economics.Agents;
using F7s.Modell.Economics.Operations;
using System.Collections.Generic;

namespace F7s.Modell.Economics.Institutions;

public abstract class SubsistenceWorkerTraderInstitution : Institution
{
    protected override List<Role> GenerateRoles()
    {
        ResourceProductionOperatorRole workRole = WorkRole();
        TradeRole tradeRole = new TradeRole();
        // TODO: Configure the trader to buy what is needed to survive, and to sell what is not.
        return new List<Role>() { workRole, tradeRole };
    }

    protected abstract ResourceProductionOperatorRole WorkRole ();
}
