using F7s.Modell.Economics.Agents;
using F7s.Modell.Handling;

namespace F7s.Modell.Economics.Facilities {

    public abstract class Facility : Structure {

        public FacilityOperationsRole FacilityOperationsManager { get; private set; }

        public bool Valid { get; protected set; }

        public void InstallManager (FacilityOperationsRole manager) {
            this.FacilityOperationsManager = manager;
        }

        public override void ConfigureInfoblock (Infoblock infoblock) {
            base.ConfigureInfoblock(infoblock);

            infoblock.AddInformation(FacilityOperationsManager, "Manager");
        }
    }
}
