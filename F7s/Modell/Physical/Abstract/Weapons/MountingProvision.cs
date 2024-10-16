using F7s.Modell.Abstract;

namespace F7s.Modell.Physical.Bodies.Weapons.Abstract
{
    public class MountingProvision : GameEntity
    {
        public MountingProvision() : base()
        {
        }

        public GameFloat DegreesOfFreedom { get; private set; }
        public GameFloat TrackingSpeed { get; private set; }
    }

    public class Handheld : MountingProvision { }
}