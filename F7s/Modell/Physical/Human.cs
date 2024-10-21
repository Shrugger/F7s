using F7s.Modell.Conceptual;
using F7s.Modell.Economics.Resources;
using F7s.Modell.Physical.Bodies;
using F7s.Modell.Physical.Localities;
using F7s.Utility;
using F7s.Utility.Geometry.Double;
using System;

namespace F7s.Modell.Physical {

    public class Human : Individual {

        private readonly HumanBody humanBody;

        public Human (string name, Locality locality) : base(name) {
            constantDemands.Add(new Resource(ResourceType.LifeSupportNecessities, 10));

            HumanBody body = new HumanBody(name, new Vector3d(0.4, 1.75, 0.2), Farbe.white);
            locality.SetPhysicalEntity(body);

            humanBody = body;
            base.SetBody(body);
        }

        public override float WorkingPower () {
            throw new NotImplementedException();
        }
    }


    public class HumanBody : Body {
        // TODO: Make HumanBody a subclass of Structure rather than of Body.
        public HumanBody (string name, Vector3d scale, Farbe color) : base(name, scale, color) {
        }

        public float Age { get; private set; }
        public float Nourishment { get; private set; }
        public float Health { get; private set; }
        public bool Dead { get; private set; }
    }

}
