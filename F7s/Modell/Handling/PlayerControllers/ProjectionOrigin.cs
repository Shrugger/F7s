using F7s.Modell.Handling.PlayerControllers;
using F7s.Modell.Physical.Localities;
using F7s.Utility.Geometry;
using F7s.Utility.Lazies;
using Stride.Core.Mathematics;

namespace F7s.Engine.PlayerControllers {
    public class ProjectionOrigin : Locality {

        private FrameFaul<Transform3D> currentTransform;

        public ProjectionOrigin () : base(null, GetOriginSuperior()) {
            this.currentTransform = new FrameFaul<Transform3D>(this.CalculateNewTransform);
        }

        private Transform3D CalculateNewTransform () {
            Locality anchor = Origin.GetFloatingOriginFloatingAnchor();
            Transform3D anchorTransform = anchor.GetLocalTransform();
            Vector3 origin = anchorTransform.Origin.ToVector3();
            return new Transform3D(Matrix3x3d.Identity, origin);
        }

        public override Transform3D GetLocalTransform () {
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
