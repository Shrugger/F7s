using F7s.Modell.Abstract;

namespace F7s.Modell.Physical.Bodies.Weapons.Abstract {
    public class FeedingMechanism : AbstractGameEntity {
        public enum Types { None, Manual, InternalMagazine, RotatingCylinder, ExternalMagazine }
        public GameValue<Types> Type { get; private set; }
    }
}
