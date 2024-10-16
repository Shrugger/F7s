using F7s.Modell.Abstract;

namespace F7s.Modell.Physical.Bodies.Weapons.Abstract {
    public class CyclingMechanism : AbstractGameEntity {
        public enum Types { None, Manual, SemiAutomatic, FullyAutomatic }
        public GameValue<Types> Type { get; private set; }
    }
}
