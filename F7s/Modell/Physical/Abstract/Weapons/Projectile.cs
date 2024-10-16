using F7s.Modell.Abstract;

namespace F7s.Modell.Physical.Bodies.Weapons.Abstract {
    public class Projectile : AbstractGameEntity {

        public enum Payloads { Heavy, Hard, Explosive, Incendiary }
        public enum Fuzes { None, Impact, ProximityRadar }

        public GameFloat Diameter { get; private set; }
        public GameFloat Length { get; private set; }
        public GameValue<Payloads> Payload { get; private set; }
        public GameValue<Fuzes> Fuze { get; private set; }
    }
}
