using F7s.Modell.Abstract;

namespace F7s.Modell.Physical.Bodies.Weapons.Abstract {

    public class ChemicalGun : Projector {
        public ChemicalGun () : base() {
        }
        public GameValue<FiringMechanism> FiringMechanism { get; private set; }
        public GameFloat ChamberLength { get; private set; }
        public GameFloat BarrelLength { get; private set; }
        public GameFloat Bore { get; private set; }
    }
}
