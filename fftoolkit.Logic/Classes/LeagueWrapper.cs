using fftoolkit.DB.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fftoolkit.Logic.Classes
{
    public class LeagueWrapper
    {
        public League League { get; set; }
        public List<Team> Teams { get; set; }
        public List<Player> Waivers { get; set; }

        public LeagueWrapper(League league)
        {
            League = league;
        }
    }
}
