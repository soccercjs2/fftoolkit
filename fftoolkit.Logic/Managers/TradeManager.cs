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
                    //get player with statistics that matches team's player
                    Player match = players.Where(p => p.Equals(team.Players[i])).FirstOrDefault();
                    team.Players[i] = match;

                    //remove match from players so that resulting players are waiver players
                    players.Remove(match);
                }

                //remove players who don't have projections from team
                team.Players.RemoveAll(p => p == null);
            }

            //resulting list of players will be the waivers
            leagueWrapper.Waivers = players;

            Player waiverQb = players.Wher

            return players;
        }
    }
}
