using fftoolkit.DB.Models;

namespace fftoolkit.Web.Models
{
    public class TradeFilterViewModel
    {
        public TradeFilterType Type { get; set; }
        public Player Player { get; set; }
        public string Position { get; set; }
    }

    public enum TradeFilterType
    {
        Player,
        Position
    }
}