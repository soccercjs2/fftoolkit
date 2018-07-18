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
                    //get player with attributes that matches team's player
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

        public List<Team> GetTeamsWithPlayers(List<Player> players, League league)
        {

            ScraperManager scraperManager = new ScraperManager(_context);
            List<Team> teams = scraperManager.ScrapeLeague(league);

            foreach (Team team in teams)
            {
                for (int i = 0; i < team.Players.Count; i++)
                {
                    //get player with attributes that matches team's player
                    Player match = players.Where(p => p.Equals(team.Players[i])).FirstOrDefault();
                    team.Players[i] = match;
                }

                team.Players.RemoveAll(p => p == null);
            }

            teams.RemoveAll(t => t == null || t.Players == null || t.Players.Count == 0);

            return teams;
        }

        public List<Trade> FindTrades(Team myTeam, Team theirTeam)
        {
            List<Trade> trades = new List<Trade>();

            List<List<Player>> myPlayerCombos = GetPlayerCombinations(myTeam);
            List<List<Player>> theirPlayerCombos = GetPlayerCombinations(theirTeam);

            foreach (List<Player> myPlayers in myPlayerCombos)
            {
                foreach (List<Player> theirPlayers in theirPlayerCombos)
                {
                    Roster myOldStartingRoster = null;
                    Roster myNewStartingRoster = null;
                    Roster theirOldStartingRoster = null;
                    Roster theirNewStartingRoster = null;

                    decimal myDifference =
                        (myNewStartingRoster.Quarterbacks.Sum(p => p.FantasyPoints) - myOldStartingRoster.Quarterbacks.Sum(p => p.FantasyPoints)) +
                        (myNewStartingRoster.RunningBacks.Sum(p => p.FantasyPoints) - myOldStartingRoster.RunningBacks.Sum(p => p.FantasyPoints)) +
                        (myNewStartingRoster.WideReceivers.Sum(p => p.FantasyPoints) - myOldStartingRoster.WideReceivers.Sum(p => p.FantasyPoints)) +
                        (myNewStartingRoster.TightEnds.Sum(p => p.FantasyPoints) - myOldStartingRoster.TightEnds.Sum(p => p.FantasyPoints)) +
                        (myNewStartingRoster.Flexes.Sum(p => p.FantasyPoints) - myOldStartingRoster.Flexes.Sum(p => p.FantasyPoints));

                    decimal theirDifference =
                        (theirNewStartingRoster.Quarterbacks.Sum(p => p.FantasyPoints) - theirOldStartingRoster.Quarterbacks.Sum(p => p.FantasyPoints)) +
                        (theirNewStartingRoster.RunningBacks.Sum(p => p.FantasyPoints) - theirOldStartingRoster.RunningBacks.Sum(p => p.FantasyPoints)) +
                        (theirNewStartingRoster.WideReceivers.Sum(p => p.FantasyPoints) - theirOldStartingRoster.WideReceivers.Sum(p => p.FantasyPoints)) +
                        (theirNewStartingRoster.TightEnds.Sum(p => p.FantasyPoints) - theirOldStartingRoster.TightEnds.Sum(p => p.FantasyPoints)) +
                        (theirNewStartingRoster.Flexes.Sum(p => p.FantasyPoints) - theirOldStartingRoster.Flexes.Sum(p => p.FantasyPoints));

                    Trade trade = new Trade
                    {
                        MyPlayers = myPlayers,
                        MyRoster = myNewStartingRoster,
                        MyDifference = myDifference,
                        TheirPlayers = theirPlayers,
                        TheirRoster = theirNewStartingRoster,
                        TheirDifference = theirDifference
                    };

                    trades.Add(trade);
                }
            }

            return trades;
        }

        private List<List<Player>> GetPlayerCombinations(Team team)
        {
            List<List<Player>> playerCombinations = new List<List<Player>>();

            if (team.Players.Count > 0)
            {
                for (int i = 0; i < team.Players.Count; i++)
                {
                    playerCombinations.Add(new List<Player>() { team.Players[i] });

                    if (team.Players.Count > 1)
                    {
                        for (int j = i + 1; j < team.Players.Count; j++)
                        {
                            playerCombinations.Add(
                                new List<Player>()
                                {
                                    team.Players[i],
                                    team.Players[j]
                                });

                            if (team.Players.Count > 2)
                            {
                                for (int k = j + 1; k < team.Players.Count; k++)
                                {
                                    playerCombinations.Add(
                                        new List<Player>()
                                        {
                                            team.Players[i],
                                            team.Players[j],
                                            team.Players[k]
                                        });
                                }
                            }
                        }
                    }
                }
            }

            return playerCombinations;
        }

        private Roster GetStartingRoster(List<Player> players, League league)
        {
            return new Roster()
            {
                Quarterbacks = players.Where(p => p.Position == "QB").OrderBy(p => p.FantasyPoints).Take(league.Quarterbacks)
            };
        }
    }
}
