using F7s.Modell.Abstract;

namespace F7s.Modell.Economics.Resources
{
    public abstract class ResourceType : GameEntity
    {

        public static readonly LifeSupportNecessities LifeSupportNecessities = new LifeSupportNecessities();
        public static readonly Machinery Machinery = new Machinery();
        public static readonly StructuralMaterials StructuralMaterials = new StructuralMaterials();
        public static readonly Fuel Fuel = new Fuel();
        public static readonly Energy Energy = new Energy();
        public static readonly Waste Waste = new Waste();


        public abstract float Density();

        protected ResourceType()
        {
        }
    }
}
