using F7s.Geometry;
using System;

namespace F7s.Modell.Physical.Localities {
    public class RootLocality : Locality {

        public static RootLocality Instance = new RootLocality();

        public RootLocality () : base(null, null) {
        }

        public override Transform3D GetLocalTransform () {
            return Transform3D.Identity;
        }

        public override Locality HierarchySuperior () {
            return null;
        }

        protected override void ReplaceSuperior (Locality replacement) {
            throw new Exception("Root Locality must not have a superior.");
        }

        public override Visualizabilities Visualizability () {
            return Visualizabilities.Static;
        }
    }
}
