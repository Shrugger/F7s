using F7s.Modell.Economics.Resources;
using F7s.Modell.Handling;
using System; using F7s.Utility.Geometry.Double;

namespace F7s.Modell.Economics.Operations {

    public abstract class ResourceProductionOperation : Operations {

        public float ProductOnSite { get; private set; }

        public ResourceProductionOperation (ResourceType product, float efficiency = DefaultEfficiency) : base(efficiency) {
            ProductType = product;
        }

        public override void ConfigureInfoblock (Infoblock infoblock) {
            base.ConfigureInfoblock(infoblock);
            infoblock.AddInformation(ProductOnSite, "Product on site");
        }

        public ResourceType ProductType { get; }

        public float RemoveProductFromSite (float quantity = float.MaxValue) {
            float result = Math.Clamp(quantity, 0, ProductOnSite);
            ProductOnSite -= result;
            return result;
        }

        protected override void OperateOperationally (float effectiveWorkAccomplished) {
            float production = Produce(effectiveWorkAccomplished);
            ProductOnSite += production;
        }

        protected abstract float Produce (float effectiveWorkAccomplished);
    }
}
