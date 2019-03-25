using fftoolkit.DB.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fftoolkit.Logic.Classes
{
    public class TradeSide
    {
        public string TeamName { get; set; }
        public List<Player> Players { get; set; }
        public Roster Roster { get; set; }
        public int MaxPlayerCount { get; set; }

        [DisplayFormat(DataFormatString = "{0:0.##}")]
        public decimal Difference { get; set; }

        [DisplayFormat(DataFormatString = "{0:0.##}")]
        public decimal TradeValue { get; set; }
    }
}
