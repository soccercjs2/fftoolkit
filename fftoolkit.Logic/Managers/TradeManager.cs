using fftoolkit.DB.Model;
using fftoolkit.Logic.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fftoolkit.Logic.Managers
{
    public class TradeManager
    {
        public TradeManager() { }

        public List<Player> GetTradeValues(List<Player> players, League league)
        {
            ScraperManager scraperManager = new ScraperManager();
            LeagueWrapper leagueWrapper = scraperManager.ScrapeLeague(league);

            foreach (Team team in leagueWrapper.Teams)
            {
                for (int i = 0; i < team.Players.Count; i++)
                {
                    team.Players[i] = players.Where(p => p.Equals(team.Players[i])).FirstOrDefault();
                }
            }

            return players;
        }
    }
}
