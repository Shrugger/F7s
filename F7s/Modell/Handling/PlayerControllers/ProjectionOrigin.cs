using F7s.Modell.Handling.PlayerControllers;
using F7s.Modell.Physical.Localities;
using F7s.Geometry;
using F7s.Utility.Lazies;
using Stride.Core.Mathematics;
using F7s.Utility.Geometry;

namespace F7s.Engine.PlayerControllers {
    public class ProjectionOrigin : Locality {

        private FrameFaul<MatrixD> currentTransform;

        public ProjectionOrigin () : base(null, GetOriginSuperior()) {
            this.currentTransform = new FrameFaul<MatrixD>(this.CalculateNewTransform);
        }

        private MatrixD CalculateNewTransform () {
            Locality anchor = Origin.GetFloatingOriginFloatingAnchor();
            MatrixD anchorTransform = anchor.GetLocalTransform();
            Vector3 origin = anchorTransform.Origin.ToVector3();
            return MatrixD.Transformation(origin, Matrix3x3d.Identity);
        }

        public override MatrixD GetLocalTransform () {
            return this.currentTransform.Value;
        }

        public override Locality HierarchySuperior () {
            return GetOriginSuperior();
        }

        private static Locality GetOriginSuperior () {
            return Origin.GetOriginLocality().HierarchySuperior();
        }
    }
}
