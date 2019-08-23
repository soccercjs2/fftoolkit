using fftoolkit.DB.Models;
using System.Collections.Generic;

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
