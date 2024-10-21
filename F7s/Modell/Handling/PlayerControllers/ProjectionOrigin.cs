using F7s.Modell.Physical.Localities;
using F7s.Utility.Geometry.Double;
using F7s.Utility.Lazies;

namespace F7s.Modell.Handling.PlayerControllers {
    public class ProjectionOrigin : Locality {

        private readonly FrameFaul<MatrixD> currentTransform;

        public ProjectionOrigin () : base(null, GetOriginSuperior()) {
            currentTransform = new FrameFaul<MatrixD>(CalculateNewTransform);
        }

        private MatrixD CalculateNewTransform () {
            Locality anchor = Origin.GetFloatingOriginFloatingAnchor();
            MatrixD anchorTransform = anchor.GetLocalTransform();
            Vector3d origin = anchorTransform.TranslationVector;
            return MatrixD.Transformation(origin, QuaternionD.Identity);
        }

        public override MatrixD GetLocalTransform () {
            return currentTransform.Value;
        }

        public override Locality HierarchySuperior () {
            return GetOriginSuperior();
        }

        private static Locality GetOriginSuperior () {
            return Origin.GetOriginLocality().HierarchySuperior();
        }
    }
}
