using F7s.Modell.Economics.Markets;
using System; using F7s.Utility.Geometry.Double; using Stride.Core.Mathematics;

namespace F7s.Modell.Economics.Trade
{
    public abstract class AnyTradeObject : TradeItem
    {
        float marketValue;

        public override TradeItemMatch Matches(TradeItem other)
        {
            throw new NotImplementedException();
        }
    }

}
