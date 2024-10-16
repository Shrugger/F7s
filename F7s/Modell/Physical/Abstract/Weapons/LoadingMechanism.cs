using F7s.Modell.Abstract;

namespace F7s.Modell.Physical.Bodies.Weapons.Abstract {
    public class LoadingMechanism : AbstractGameEntity {
        public enum Types { None, Manual, MuzzleLoading, BreechLoading }
        public GameValue<Types> Type { get; private set; }
    }
}
