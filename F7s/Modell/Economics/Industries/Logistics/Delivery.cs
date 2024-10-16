using F7s.Modell.Economics.Trade;
using F7s.Modell.Physical.Localities;

namespace F7s.Modell.Economics
{
    public class Delivery : TradeItem
    {
        float volume;
        float mass;
        Locality origin;
        Locality destination;
        double latestDeliveryByDate;
    }

}
