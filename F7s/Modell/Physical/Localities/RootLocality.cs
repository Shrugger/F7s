using F7s.Utility.Geometry.Double;
using System;
namespace F7s.Modell.Physical.Localities {
    public class RootLocality : Locality {

        public static RootLocality Instance = new RootLocality();

        public RootLocality () : base(null, null) {
            Name = "Root of all Places";
        }

        public override MatrixD GetLocalTransform () {
            return MatrixD.Identity;
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
