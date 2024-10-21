using F7s.Modell.Physical;
using F7s.Modell.Physical.Localities;
using System; using F7s.Utility.Geometry.Double;

namespace F7s.Modell.Conceptual {
    public abstract class Individual : Agent {

        public PhysicalEntity Body { get; private set; }

        protected Individual (string name) : base(name) {

        }

        protected void SetBody (PhysicalEntity body) {
            this.Body = body;
        }

        public override Locality GetLocality () {
            return Body.Locality;
        }

        public override void FoldIntoSupergroup () {
            throw new NotImplementedException();
        }

        public override void UnfoldAgent () {
            throw new Exception("Not applicable.");
        }

        public override void UnfoldAllAgents () {
            throw new Exception("Not applicable.");
        }
    }
}
