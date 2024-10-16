using F7s.Modell.Abstract;

namespace F7s.Modell.Physical.Bodies.Weapons.Abstract {

    public class Gun : Weapon {
        public Gun () : base() {
        }

        public GameFloat ShotsPerSecond { get; private set; }
        public GameFloat MuzzleVelocity { get; private set; }
        public GameFloat Bore { get; private set; }
        public GameFloat ChamberLength { get; private set; }
    }
}
