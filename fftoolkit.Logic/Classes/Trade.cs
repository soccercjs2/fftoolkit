using fftoolkit.DB.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fftoolkit.Logic.Classes
{
    public class Trade
    {
        public int TradeId { get; set; }
        public TradeSide MyTradeSide { get; set; }
        public TradeSide TheirTradeSide { get; set; }
    }
}
