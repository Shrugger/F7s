using F7s.Modell.Abstract;

namespace F7s.Modell.Physical.Bodies.Weapons.Abstract {
    public class FiringMechanism : AbstractGameEntity {
        public enum Types { Centrefire, Electric }
        public GameValue<Types> Type { get; private set; }
    }
}
