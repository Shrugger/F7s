using System; using F7s.Utility.Geometry.Double; using Stride.Core.Mathematics;
using System.Collections.Generic;
using F7s.Modell.Abstract;
using F7s.Modell.Economics.Trade;

namespace F7s.Modell.Economics.Markets
{

    public class Market : GameEntity, Hierarchical<Market>
    {

        // TODO: Track Transactions
        public static Market UniversalMarket { get; private set; }

        private readonly List<TradeProposal> proposals = new List<TradeProposal>();

        static Market ()
        {
            UniversalMarket = new Market();
        }



        public void RegisterProposal (TradeProposal proposal)
        {
            this.proposals.Add(proposal);
        }

        public void DeregisterProposal (TradeProposal proposal)
        {
            this.proposals.Remove(proposal);
        }

        public Money MarketValue (TradeItem item)
        {
            throw new NotImplementedException();
        }

        public Market HierarchicalLowestCommonSuperior(Market other)
        {
            throw new NotImplementedException();
        }

        public Market HierarchyMember()
        {
            throw new NotImplementedException();
        }

        public Hierarchical<Market> HierarchyRoot()
        {
            throw new NotImplementedException();
        }

        public List<Market> HierarchySubordinates()
        {
            throw new NotImplementedException();
        }

        public Market HierarchySuperior()
        {
            throw new NotImplementedException();
        }
    }
}
