using fftoolkit.DB.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace fftoolkit.Logic.Classes
{
    public class TradeSide
    {
        public string TeamName { get; set; }
        public List<Player> NewPlayers { get; set; }
        public List<Player> OldPlayers { get; set; }
        public Roster OldRoster { get; set; }
        public Roster NewRoster { get; set; }
        public int MaxPlayerCount { get; set; }

        [DisplayFormat(DataFormatString = "{0:0.##}")]
        public decimal Difference { get; set; }

        [DisplayFormat(DataFormatString = "{0:0.##}")]
        public decimal TradeValue { get; set; }
    }
}
