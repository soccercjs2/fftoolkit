using fftoolkit.DB.Context;
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
        private DataContext _context;

        public TradeManager(DataContext context)
        {
            _context = context ?? throw new Exception("The context cannot be null.");
        }

        public List<Player> GetTradeValues(List<Player> players, League league)
        {
            ScraperManager scraperManager = new ScraperManager(_context);
            List<Team> teams = scraperManager.ScrapeLeague(league);

            List<Player> waivers = new List<Player>(players);
            players = new List<Player>();

            foreach (Team team in teams)
            {
                for (int i = 0; i < team.Players.Count; i++)
                {
                    //get player with statistics that matches team's player
                    Player match = waivers.Where(p => p.Equals(team.Players[i])).FirstOrDefault();

                    if (match != null)
                    {
                        players.Add(match);

                        //remove match from players so that resulting players are waiver players
                        waivers.Remove(match);
                    }
                }
            }

            Player waiverQb = waivers.Where(p => p.Position == "QB").OrderByDescending(p => p.FantasyPoints).FirstOrDefault();
            Player waiverRb = waivers.Where(p => p.Position == "RB").OrderByDescending(p => p.FantasyPoints).FirstOrDefault();
            Player waiverWr = waivers.Where(p => p.Position == "WR").OrderByDescending(p => p.FantasyPoints).FirstOrDefault();
            Player waiverTe = waivers.Where(p => p.Position == "TE").OrderByDescending(p => p.FantasyPoints).FirstOrDefault();

            foreach (Player player in players)
            {
                if (player.Position == "QB") { player.TradeValue = player.FantasyPoints - waiverQb.FantasyPoints; }
                else if (player.Position == "RB") { player.TradeValue = player.FantasyPoints - waiverRb.FantasyPoints; }
                else if (player.Position == "WR") { player.TradeValue = player.FantasyPoints - waiverWr.FantasyPoints; }
                else if (player.Position == "TE") { player.TradeValue = player.FantasyPoints - waiverTe.FantasyPoints; }
            }

            return players;
        }
    }
}
