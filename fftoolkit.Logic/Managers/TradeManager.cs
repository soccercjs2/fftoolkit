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
            PlayerManager playerManager = new PlayerManager(_context);
            List<Team> teams = scraperManager.ScrapeLeague(league);

            foreach (Team team in teams)
            {
                for (int i = 0; i < team.Players.Count; i++)
                {
                    //get player with attributes that matches team's player
                    Player match = players.Where(p => p.Equals(team.Players[i])).FirstOrDefault();

                    if (match != null) { team.Players[i] = match; }
                    else
                    {
                        playerManager.AddUnmatchedPlayer(team.Players[i]);
                    }
                }

                team.Players.RemoveAll(p => p == null);
            }

            teams.RemoveAll(t => t == null || t.Players == null || t.Players.Count == 0);

            return teams;
        }

        public List<Trade> FindTrades(Team myTeam, Team theirTeam, League league)
        {
            List<Trade> trades = new List<Trade>();
            Roster myOldStartingRoster = GetStartingRoster(myTeam.Players, league);
            Roster theirOldStartingRoster = GetStartingRoster(theirTeam.Players, league);

            List<List<Player>> myPlayerCombos = GetPlayerCombinations(myTeam);
            List<List<Player>> theirPlayerCombos = GetPlayerCombinations(theirTeam);

            foreach (List<Player> myPlayers in myPlayerCombos)
            {
                foreach (List<Player> theirPlayers in theirPlayerCombos)
                {
                    Roster myNewStartingRoster = GetStartingRoster(myTeam.Players, league, theirPlayers, myPlayers);
                    Roster theirNewStartingRoster = GetStartingRoster(theirTeam.Players, league, myPlayers, theirPlayers);

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

                    if (myPlayers.Count == 3 && theirPlayers.Count == 3 && myDifference > 3 && theirDifference > 1)
                    {
                        var asdf = "asdf";
                    }

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

            trades = trades
                .Where(t => t.MyDifference > 0 && t.TheirDifference > 0)
                .OrderBy(t => t.MyDifference * t.TheirDifference)
                .ToList();

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
            return GetStartingRoster(players, league, null, null);
        }

        private Roster GetStartingRoster(List<Player> players, League league, List<Player> newPlayers, List<Player> oldPlayers)
        {
            if (oldPlayers != null) { foreach (Player oldPlayer in oldPlayers) { players.Remove(oldPlayer); } }
            if (newPlayers != null) { foreach (Player newPlayer in newPlayers) { players.Add(newPlayer); } }

            List<Player> quarterbacks = players
                .Where(p => p.Position == "QB")
                .OrderBy(p => p.FantasyPoints)
                .Take(league.Quarterbacks)
                .ToList();

            List<Player> runningBacks = players
                .Where(p => p.Position == "RB")
                .OrderBy(p => p.FantasyPoints)
                .Take(league.Quarterbacks)
                .ToList();

            List<Player> wideReceivers = players
                .Where(p => p.Position == "WR")
                .OrderBy(p => p.FantasyPoints)
                .Take(league.Quarterbacks)
                .ToList();

            List<Player> tightEnds = players
                .Where(p => p.Position == "TE")
                .OrderBy(p => p.FantasyPoints)
                .Take(league.Quarterbacks)
                .ToList();

            foreach (Player quarterback in quarterbacks) { players.Remove(quarterback); }
            foreach (Player runningBack in runningBacks) { players.Remove(runningBack); }
            foreach (Player wideReceiver in wideReceivers) { players.Remove(wideReceiver); }
            foreach (Player tightEnd in tightEnds) { players.Remove(tightEnd); }

            List<Player> flexes = players
                .Where(p => p.Position == "RB" || p.Position == "WR" || p.Position == "TE")
                .OrderBy(p => p.FantasyPoints)
                .Take(league.Flexes)
                .ToList();

            return new Roster()
            {
                Quarterbacks = quarterbacks,
                RunningBacks = runningBacks,
                WideReceivers = wideReceivers,
                TightEnds = tightEnds,
                Flexes = flexes
            };
        }
    }
}
