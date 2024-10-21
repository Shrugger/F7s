using F7s.Modell.Abstract;
using F7s.Modell.Economics.Industry;
using F7s.Modell.Economics.Markets;
using System; using F7s.Utility.Geometry.Double; using Stride.Core.Mathematics;
using System.Reflection.Metadata.Ecma335;

namespace F7s.Modell.Economics.Trade
{
    public abstract class TradeItem : GameEntity
    {

        public virtual TradeItemMatch Matches (TradeItem other) {

            if (!this.GetType().IsAssignableFrom(other.GetType()) && !this.GetType().IsAssignableTo(other.GetType())) {
                return new TradeItemMatch(this , other , TradeItemMatch.TradeItemMatchType.WrongType);
            }

            return MatchesTypeSpecific(other);
        }

        protected virtual TradeItemMatch MatchesTypeSpecific (TradeItem other) {
            return new TradeItemMatch(this, other, TradeItemMatch.TradeItemMatchType.Match);
        }
    }

}
