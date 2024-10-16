using F7s.Modell.Abstract;

namespace F7s.Modell.Physical.Bodies.Weapons.Abstract {
    public class Magazine : AbstractGameEntity {
        public enum Types { Box, Tube, Belt }
        public GameValue<Types> Type { get; private set; }
    }
}
