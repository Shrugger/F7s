using F7s.Modell.Abstract;
using F7s.Utility;
using F7s.Utility.Ranges;
using System;

namespace F7s.Modell.Economics.Trade {
    public abstract class TradeProposal : GameEntity {

        public enum ProposalTypes { Sell, Buy }
        protected ProposalTypes ProposalType { get; private set; }
        protected TradeItem Item { get; private set; }
        protected double MinPricePerUnit { get; private set; }
        protected double MaxPricePerUnit { get; private set; }
        protected long MinUnits { get; private set; }
        protected long MaxUnits { get; private set; }

        protected TradeProposal (ProposalTypes type, TradeItem item, double minPricePerUnit, double maxPricePerUnit, long minUnits, long maxUnits) {
            this.ProposalType = type;
            this.Item = item;
            this.MaxPricePerUnit = maxPricePerUnit;
            this.MinPricePerUnit = minPricePerUnit;
            this.MinUnits = minUnits;
            this.MaxUnits = maxUnits;
        }

        public override string ToString () {
            string result = "WT";
            switch (this.ProposalType) {
                case ProposalTypes.Sell:
                    result += "S";
                    break;
                case ProposalTypes.Buy:
                    result += "B";
                    break;
                default:
                    throw new NotImplementedException();
            }

            result += " "
                + this.Item.ToString()
                + " @"
                + Rounding.RoundToFirstInterestingDigit(this.MinPricePerUnit)
                + "-"
                + Rounding.RoundToFirstInterestingDigit(this.MaxPricePerUnit)
                + Chars.yen;

            return result;
        }

        public static TradeProposalMatch Match (TradeProposal buy, TradeProposal sell) {
            TradeItemMatch itemMatch = buy.Item.Matches(sell.Item);
            switch (itemMatch.Type) {
                case TradeItemMatch.TradeItemMatchType.Undefined:
                    throw new Exception();
                case TradeItemMatch.TradeItemMatchType.WrongType:
                    return new TradeProposalMatch(buy, sell, TradeProposalMatch.Types.WrongType);
                case TradeItemMatch.TradeItemMatchType.Match:
                    break;
                default:
                    throw new NotImplementedException();
            }

            if (buy.MinUnits > sell.MaxUnits || buy.MaxUnits < sell.MinUnits) {
                return new TradeProposalMatch(buy, sell, TradeProposalMatch.Types.IncompatibleAmount);
            } else {
                if (buy.MaxPricePerUnit < sell.MinPricePerUnit) {
                    return new TradeProposalMatch(buy, sell, TradeProposalMatch.Types.TooExpensive);
                } else if (buy.MinPricePerUnit >= sell.MaxPricePerUnit) {
                    return new TradeProposalMatch(buy, sell, TradeProposalMatch.Types.ImmediateMatch);
                } else if (buy.MaxPricePerUnit >= sell.MinPricePerUnit) {
                    return new TradeProposalMatch(buy, sell, TradeProposalMatch.Types.Negotiable);
                } else {
                    throw new Exception();
                }
            }
        }

        public TradeProposalMatch Matches (TradeProposal other) {
            if (this.ProposalType == ProposalTypes.Buy && other.ProposalType == ProposalTypes.Sell) {
                return Match(this, other);
            }
            if (this.ProposalType == ProposalTypes.Sell && other.ProposalType == ProposalTypes.Buy) {
                return Match(other, this);
            }
            throw new Exception();
        }
    }

}
