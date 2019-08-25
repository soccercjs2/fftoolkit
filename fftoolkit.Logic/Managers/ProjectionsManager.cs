using fftoolkit.DB.Context;
using fftoolkit.DB.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace fftoolkit.Logic.Managers
{
    public class ProjectionManager
    {
        private DataContext _context;

        public ProjectionManager(DataContext context)
        {
            _context = context ?? throw new Exception("The context cannot be null.");
        }

        public List<Player> GetProjections()
        {
            ScraperManager scraperManager = new ScraperManager(_context);
            
            //assemble weekly projections
            List<List<Player>> projections = new List<List<Player>>();
            List<Player> beginSeasonProjections = null;
            List<Player> nextWeekProjections = null;

            int year = GetCurrentYear();
            int currentWeek = GetCurrentWeek();
            int startWeek = currentWeek - 5;

            if (currentWeek == 0) { return scraperManager.ScrapeFantasyPros(0); }

            if (startWeek <= 0) { beginSeasonProjections = scraperManager.ScrapeFantasyPros(0); }

            for (int week = startWeek; week < currentWeek; week++)
            {
                if (week <= 0) { projections.Add(beginSeasonProjections); }
                else
                {
                    List<Player> nflPlayers = scraperManager.ScrapeNfl(year, week);
                    projections.Add(nflPlayers);
                }
            }

            nextWeekProjections = scraperManager.ScrapeFantasyPros(currentWeek);

            return MergeProjections(projections);
        }

        private int GetCurrentYear()
        {
            return DateTime.Today.Year;
        }

        private int GetCurrentWeek()
        {
            return 0;
        }

        private List<Player> MergeProjections(List<List<Player>> projectionsByWeek)
        {
            List<Player> projections = new List<Player>();

            foreach (List<Player> weeklyProjections in projectionsByWeek)
            {
                foreach (Player player in weeklyProjections)
                {
                    Player matchedPlayer = projections.Where(p => p.Equals(player)).FirstOrDefault();

                    if (matchedPlayer != null && matchedPlayer.Name == "Aaron Rodgers")
                    {
                        var asdf = "asdf";
                    }

                    if (matchedPlayer == null) { projections.Add(new Player(player)); }
                    else
                    {
                        matchedPlayer.PassingYards += player.PassingYards;
                        matchedPlayer.PassingTouchdowns += player.PassingTouchdowns;
                        matchedPlayer.Interceptions += player.Interceptions;
                        matchedPlayer.RushingYards += player.RushingYards;
                        matchedPlayer.RushingTouchdowns += player.RushingTouchdowns;
                        matchedPlayer.Receptions += player.Receptions;
                        matchedPlayer.ReceivingYards += player.ReceivingYards;
                        matchedPlayer.ReceivingYards += player.ReceivingTouchdowns;
                        matchedPlayer.GamesPlayed += player.GamesPlayed;
                    }
                }
            }

            projections = projections.Where(p => p.GamesPlayed >= 3).ToList();

            foreach (Player player in projections)
            {
                player.PassingYards /= player.GamesPlayed;
                player.PassingTouchdowns /= player.GamesPlayed;
                player.Interceptions /= player.GamesPlayed;
                player.RushingYards /= player.GamesPlayed;
                player.RushingTouchdowns /= player.GamesPlayed;
                player.Receptions /= player.GamesPlayed;
                player.ReceivingYards /= player.GamesPlayed;
                player.ReceivingYards /= player.GamesPlayed;
            }

            return projections;
        }
    }
}
