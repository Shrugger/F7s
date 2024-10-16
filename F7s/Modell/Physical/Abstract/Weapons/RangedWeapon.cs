using F7s.Modell.Abstract;

namespace F7s.Modell.Physical.Bodies.Weapons.Abstract {
    public class RangedWeapon {
        public GameValue<Projector> Projector { get; private set; }
        public GameValue<MountingProvision> Mount { get; private set; }
        public GameValue<LoadingMechanism> LoadingMechanism { get; private set; }
    }
}
