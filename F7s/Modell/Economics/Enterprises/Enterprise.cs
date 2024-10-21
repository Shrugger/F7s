using F7s.Modell.Abstract;
using F7s.Modell.Economics.Agents;
using F7s.Modell.Economics.Facilities;
using System;
using F7s.Utility.Geometry.Double;
using Stride.Core.Mathematics;
using F7s.Modell.Conceptual.Agents;

namespace F7s.Modell.Economics.Enterprises {
    public class Enterprise : GameEntity
    {
        public EnterpriseManagementRole EnterpriseManagementRole { get; private set; }
        public Group Group { get; private set; }
        public Facility Facility { get; private set; }

        public void InstallManager(EnterpriseManagementRole manager)
        {
            EnterpriseManagementRole = manager;
        }
    }
}
