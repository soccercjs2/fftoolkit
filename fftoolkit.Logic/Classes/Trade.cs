namespace fftoolkit.Logic.Classes
{
    public class Trade
    {
        public int TradeId { get; set; }
        public TradeSide MyTradeSide { get; set; }
        public TradeSide TheirTradeSide { get; set; }
    }
}
