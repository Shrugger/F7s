using F7s.Modell.Abstract;

namespace F7s.Modell.Physical.Bodies.Weapons.Abstract {
    public class Cartridge : Ammunition {
        public GameFloat Diameter { get; private set; }
        public GameFloat Length { get; private set; }
        public Projectile Projectile { get; private set; }
        public GameFloat PropellantEnergy { get; private set; }
    }
}
