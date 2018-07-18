using fftoolkit.DB.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fftoolkit.Logic.Classes
{
    public class Trade
    {
        public List<Player> MyPlayers { get; set; }
        public Roster MyRoster { get; set; }
        public decimal MyDifference { get; set; }

        public List<Player> TheirPlayers { get; set; }
        public Roster TheirRoster { get; set; }
        public decimal TheirDifference { get; set; }        
    }
}
