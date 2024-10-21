using F7s.Modell.Conceptual;
using F7s.Modell.Economics.Resources;
using F7s.Modell.Physical.Bodies;
using F7s.Modell.Physical.Localities;
using F7s.Utility;
using System; using F7s.Utility.Geometry.Double;
using Stride.Core.Mathematics; using F7s.Utility.Geometry.Double;

namespace F7s.Modell.Physical {

    public class Human : Individual {

        private HumanBody humanBody;

        public Human (string name, Locality locality) : base(name) {
            this.constantDemands.Add(new Resource(ResourceType.LifeSupportNecessities, 10));

            HumanBody body = new HumanBody(name, new Vector3(0.4f, 1.75f, 0.2f), Farbe.white);
            locality.SetPhysicalEntity(body);

            this.humanBody = body;
            base.SetBody(body);
        }

        public override float WorkingPower () {
            throw new NotImplementedException();
        }
    }


    public class HumanBody : Body {
        // TODO: Make HumanBody a subclass of Structure rather than of Body.
        public HumanBody (string name, Vector3 scale, Farbe color) : base(name, scale, color) {
        }

        public float Age { get; private set; }
        public float Nourishment { get; private set; }
        public float Health { get; private set; }
        public bool Dead { get; private set; }
    }

}
