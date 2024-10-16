using F7s.Modell.Economics.Facilities;
using F7s.Modell.Economics.Resources;
using System.Collections.Generic;

namespace F7s.Modell.Economics.Industry
{
    public class LifeSupportFacility : Facility
    {
        private readonly ResourceConverter airScrubber;
        private readonly ResourceConverter waterPurifier;
        private readonly ResourceConverter hydroponics;

        public LifeSupportFacility()
        {
            airScrubber = new ResourceConverter(new Recipe(new List<Resource>() { new Resource(ResourceType.Waste, 1), new Resource(ResourceType.Energy, 1) },
                                                           new List<Resource>() { new Resource(ResourceType.LifeSupportNecessities, 1) }));
        }

    }
}
