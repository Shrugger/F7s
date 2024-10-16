using F7s.Modell.Economics.Markets;

namespace F7s.Modell.Economics.Trade
{
    public class Money : TradeItem
    {
        public readonly float Value;

        public Money (float value)
        {
            this.Value = value;
        }

        public static implicit operator Money (float value)
        {
            return new Money(value);
        }

        public static implicit operator float (Money money)
        {
            return money.Value;
        }
    }

}
