using F7s.Modell.Abstract;
using F7s.Modell.Physical;

namespace F7s.Modell.Economics.Facilities {
    public abstract class Structure : AbstractGameValue {

        public PhysicalEntity PhysicalEntity { get; private set; }

        public Structure () : base() {
        }

        public virtual bool SupportsOperations (Operations.Operations operations) {
            return false;
        }

        public void SetPhysicalEntity (PhysicalEntity physicalEntity) {
            physicalEntity.AddStructure(this);
        }
    }
}
