using System;
using F7s.Utility.Geometry.Double;
using Stride.Core.Mathematics;
using System.Collections.Generic;
using F7s.Modell.Conceptual;
using F7s.Modell.Economics.Markets;
using F7s.Modell.Economics.Trade;
using F7s.Modell.Conceptual.Agents.Roles;

namespace F7s.Modell.Economics.Agents
{

    public class TradeRole : Role
    {
        // Buy what's needed, sell what isn't, try to find business opportunities.

        private Market market;
        private List<TradeItem> demand = new List<TradeItem>();
        private List<TradeItem> supply = new List<TradeItem>();
        private List<TradeProposal> openTradeProposals = new List<TradeProposal>();

        public override void RunRole(double deltaTime)
        {
            UpdateOpenTradeProposals();
            MatchProposalWithMarket();
        }

        public void SetMarket(Market market)
        {
            this.market = market ?? Market.UniversalMarket;
        }

        private Money CheckMarketValue(TradeItem item, Market market = null)
        {
            market ??= this.market;
            return market.MarketValue(item);
        }

        private void MatchProposalWithMarket()
        {
            if (this.openTradeProposals.Count > 0) {
                throw new NotImplementedException("Check all available markets for proposals that match my proposals.");
            }
        }

        private void UpdateOpenTradeProposals()
        {
            if (this.demand.Count > 0 && this.supply.Count > 0) {
                throw new NotImplementedException(
                    "Try to adjust my trade proposals so that I manage to satisfy as much demand as possible while expending as little supply as possible."
                    );
            } else if (this.openTradeProposals.Count > 0 && this.demand.Count == 0 && this.supply.Count == 0) {
                throw new NotImplementedException("Cancel all proposals since I have nothing to trade with.");
            }
        }

        public void DefineDemand(List<TradeItem> requiredTradeItems)
        {
            throw new NotImplementedException("Define trade items I am to acquire.");
        }

        public void DefineSupply(List<TradeItem> disposableTradeItems)
        {
            throw new NotImplementedException("Define trade items I can dispose of.");
        }
    }
}
