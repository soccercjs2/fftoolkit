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

            //cuttoffs to determine baseline player
            int qbCuttoff = teams.Count * league.Quarterbacks;
            int rbCuttoff = teams.Count * (league.RunningBacks + league.Flexes);
            int wrCuttoff = teams.Count * (league.WideReceivers + league.Flexes);
            int teCuttoff = teams.Count * league.TightEnds;

            List<Player> quarterbacks = players.Where(p => p.Position == "QB").ToList();
            List<Player> runningBacks = players.Where(p => p.Position == "RB").ToList();
            List<Player> wideReceivers = players.Where(p => p.Position == "WR").ToList();
            List<Player> tightEnds = players.Where(p => p.Position == "TE").ToList();

            int qbCuttoffIndex = Math.Min(qbCuttoff, quarterbacks.Count() - 1);
            int rbCuttoffIndex = Math.Min(rbCuttoff, runningBacks.Count() - 1);
            int wrCuttoffIndex = Math.Min(wrCuttoff, wideReceivers.Count() - 1);
            int teCuttoffIndex = Math.Min(teCuttoff, tightEnds.Count() - 1);

            //get baseline players
            Player qbBaseline = players.Where(p => p.Position == "QB").OrderByDescending(p => p.FantasyPoints).ElementAt(qbCuttoffIndex);
            Player rbBaseline = players.Where(p => p.Position == "RB").OrderByDescending(p => p.FantasyPoints).ElementAt(rbCuttoffIndex);
            Player wrBaseline = players.Where(p => p.Position == "WR").OrderByDescending(p => p.FantasyPoints).ElementAt(wrCuttoffIndex);
            Player teBaseline = players.Where(p => p.Position == "TE").OrderByDescending(p => p.FantasyPoints).ElementAt(teCuttoffIndex);

            foreach (Player player in players)
            {
                if (player.Position == "QB") { player.TradeValue = player.FantasyPoints - qbBaseline.FantasyPoints; }
                else if (player.Position == "RB") { player.TradeValue = player.FantasyPoints - rbBaseline.FantasyPoints; }
                else if (player.Position == "WR") { player.TradeValue = player.FantasyPoints - wrBaseline.FantasyPoints; }
                else if (player.Position == "TE") { player.TradeValue = player.FantasyPoints - teBaseline.FantasyPoints; }
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

                    if (team.Players[i].Position == "QB" || team.Players[i].Position == "RB" || team.Players[i].Position == "WR" || team.Players[i].Position == "TE")
                    {
                        playerManager.AddUnmatchedPlayer(new UnmatchedPlayer(team.Players[i]));
                    }

                    team.Players[i] = match;
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

                    bool allTradedPlayersAreStarting = true;

                    foreach (Player theirPlayer in theirPlayers)
                    {
                        if ((theirPlayer.Position == "QB" && !myNewStartingRoster.Quarterbacks.Contains(theirPlayer) && !myNewStartingRoster.Flexes.Contains(theirPlayer)) ||
                            (theirPlayer.Position == "RB" && !myNewStartingRoster.RunningBacks.Contains(theirPlayer) && !myNewStartingRoster.Flexes.Contains(theirPlayer)) ||
                            (theirPlayer.Position == "WR" && !myNewStartingRoster.WideReceivers.Contains(theirPlayer) && !myNewStartingRoster.Flexes.Contains(theirPlayer)) ||
                            (theirPlayer.Position == "TE" && !myNewStartingRoster.TightEnds.Contains(theirPlayer) && !myNewStartingRoster.Flexes.Contains(theirPlayer)))
                        {
                            allTradedPlayersAreStarting = false;
                        }
                    }

                    foreach (Player myPlayer in myPlayers)
                    {
                        if ((myPlayer.Position == "QB" && !theirNewStartingRoster.Quarterbacks.Contains(myPlayer) && !theirNewStartingRoster.Flexes.Contains(myPlayer)) ||
                            (myPlayer.Position == "RB" && !theirNewStartingRoster.RunningBacks.Contains(myPlayer) && !theirNewStartingRoster.Flexes.Contains(myPlayer)) ||
                            (myPlayer.Position == "WR" && !theirNewStartingRoster.WideReceivers.Contains(myPlayer) && !theirNewStartingRoster.Flexes.Contains(myPlayer)) ||
                            (myPlayer.Position == "TE" && !theirNewStartingRoster.TightEnds.Contains(myPlayer) && !theirNewStartingRoster.Flexes.Contains(myPlayer)))
                        {
                            allTradedPlayersAreStarting = false;
                        }
                    }

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

                    decimal myTradeValue = myPlayers.Sum(p => p.TradeValue);
                    decimal theirTradeValue = theirPlayers.Sum(p => p.TradeValue);
                    decimal fairness = Math.Abs(myTradeValue - theirTradeValue);
                    int maxPlayerCount = Math.Max(myPlayers.Count, theirPlayers.Count);

                    if (allTradedPlayersAreStarting && myDifference > 0 && theirDifference > 0 && fairness <= 3)
                    {
                        Trade trade = new Trade
                        {
                            TradeId = trades.Count,
                            MyTradeSide = new TradeSide()
                            {
                                TeamName = myTeam.Name,
                                Players = myPlayers.OrderByDescending(p => p.TradeValue).ToList(),
                                Roster = myNewStartingRoster,
                                Difference = myDifference,
                                TradeValue = myTradeValue,
                                MaxPlayerCount = maxPlayerCount
                            },
                            
                            TheirTradeSide = new TradeSide()
                            {
                                TeamName = theirTeam.Name,
                                Players = theirPlayers.OrderByDescending(p => p.TradeValue).ToList(),
                                Roster = theirNewStartingRoster,
                                Difference = theirDifference,
                                TradeValue = theirTradeValue,
                                MaxPlayerCount = maxPlayerCount
                            }
                        };

                        trades.Add(trade);
                    }
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

                                    if (team.Players.Count > 3)
                                    {
                                        for (int l = k + 1; l < team.Players.Count; l++)
                                        {
                                            playerCombinations.Add(
                                                new List<Player>()
                                                {
                                                    team.Players[i],
                                                    team.Players[j],
                                                    team.Players[k],
                                                    team.Players[l]
                                                });
                                        }
                                    }
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

        private Roster GetStartingRoster(List<Player> originalPlayers, League league, List<Player> newPlayers, List<Player> oldPlayers)
        {
            List<Player> players = new List<Player>(originalPlayers);

            if (oldPlayers != null) { foreach (Player oldPlayer in oldPlayers) { players.Remove(oldPlayer); } }
            if (newPlayers != null) { foreach (Player newPlayer in newPlayers) { players.Add(newPlayer); } }

            List<Player> quarterbacks = players
                .Where(p => p.Position == "QB")
                .OrderByDescending(p => p.FantasyPoints)
                .Take(league.Quarterbacks)
                .ToList();

            List<Player> runningBacks = players
                .Where(p => p.Position == "RB")
                .OrderByDescending(p => p.FantasyPoints)
                .Take(league.RunningBacks)
                .ToList();

            List<Player> wideReceivers = players
                .Where(p => p.Position == "WR")
                .OrderByDescending(p => p.FantasyPoints)
                .Take(league.WideReceivers)
                .ToList();

            List<Player> tightEnds = players
                .Where(p => p.Position == "TE")
                .OrderByDescending(p => p.FantasyPoints)
                .Take(league.TightEnds)
                .ToList();

            foreach (Player quarterback in quarterbacks) { players.Remove(quarterback); }
            foreach (Player runningBack in runningBacks) { players.Remove(runningBack); }
            foreach (Player wideReceiver in wideReceivers) { players.Remove(wideReceiver); }
            foreach (Player tightEnd in tightEnds) { players.Remove(tightEnd); }

            List<Player> flexes = players
                .Where(p => p.Position == "RB" || p.Position == "WR" || p.Position == "TE")
                .OrderByDescending(p => p.FantasyPoints)
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
