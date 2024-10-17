namespace F7s.Utility.Shapes {
    
    public class HollowSphere : Sphere {

        public readonly float internalRadius;
        public HollowSphere(float externalRadius, float internalRadius) : base(externalRadius) {
            this.internalRadius = internalRadius;
        }

        public override Shape3Dim GetInternalNegativeSpace() {
            return new Sphere(this.internalRadius);
        }
    }

}