using System;
using F7s.Modell.Conceptual.Agents;
using F7s.Modell.Economics.Facilities;

namespace F7s.Modell.Economics.Agents {
    
    /// <summary>
    /// Ensures the facility has what it needs and operates as much as desired. Buys supply and sells off surplus, and runs production at a rate aimed to reach a given target.
    /// </summary>
    public class FacilityOperationsRole : TradeRole
    {
        private Facility facility;

        public override void RunRole(double deltaTime)
        {
            
            throw new NotImplementedException();
        }
    }
}
