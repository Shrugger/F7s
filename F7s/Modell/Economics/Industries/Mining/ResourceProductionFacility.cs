using F7s.Modell.Economics.Facilities;
using F7s.Modell.Economics.Operations;
using F7s.Modell.Economics.Resources;

namespace F7s.Modell.Economics.Mining {

    /// <summary>
    /// A facility that produces resources of a given type.
    /// </summary>
    public abstract class ResourceProductionFacility : Facility {
        private ResourceProductionOperation operations;
        public ResourceType product;
        public float ProductOnSite { get; private set; }

        public void ProduceResource (double deltaTime) {
            operations.Operate(deltaTime);
            ProductOnSite += operations.RemoveProductFromSite();
        }
    }
}
