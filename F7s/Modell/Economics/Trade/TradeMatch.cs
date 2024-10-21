using F7s.Modell.Abstract;
using F7s.Modell.Physical;
using System; using F7s.Utility.Geometry.Double; using Stride.Core.Mathematics;
using static F7s.Modell.Economics.Trade.TradeItemMatch;
using static F7s.Modell.Economics.Trade.TradeProposalMatch;

namespace F7s.Modell.Economics.Trade
{

    public class TradeItemMatch : GameValue<TradeItemMatchType> {
        public enum TradeItemMatchType { Undefined, WrongType, Match }

        public readonly TradeItemMatchType Type;
        public readonly TradeItem Item1;
        public readonly TradeItem Item2;

        public TradeItemMatch(TradeItem item1, TradeItem item2, TradeItemMatchType type) : base(type) {
            Type = type;
            this.Item1 = item1;
            this.Item2 = item2;
        }

        public static implicit operator TradeItemMatchType (TradeItemMatch match) {
            return match.Type;
        }

    }

    public class TradeProposalMatch : GameValue<Types> {
        // Whether a given trade is feasible at all. If yes, then the exact price and possibly the exact quantity still need to be determined through negotiation.

        public enum Types { Undefined, WrongType, IncompatibleAmount, ImmediateMatch, Negotiable, TooExpensive }
        public enum Quantities { Undefined, Compatible, IncompatibleAmount }
        public enum Prices { Undefined, ImmediateMatch, Negotiable, TooExpensive }

        public readonly Types Type;
        public readonly TradeProposal Request;
        public readonly TradeProposal Offer;

        public TradeProposalMatch(TradeProposal request, TradeProposal offer, Types type) : base(type) {
            Type = type;
            this.Request = request;
            this.Offer = offer;
        }

        public static implicit operator Types (TradeProposalMatch match) {
            return match.Type;
        }

    }

}
