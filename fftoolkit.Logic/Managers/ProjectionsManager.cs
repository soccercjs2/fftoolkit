using fftoolkit.DB.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fftoolkit.Logic.Managers
{
    public class ProjectionsManager
    {
        public ProjectionsManager() { }

        public List<Player> GetProjections()
        {
            ScraperManager scraperManager = new ScraperManager();

            int week = GetCurrentWeek();
            if (week == 0) { return scraperManager.ScrapeFantasyPros(0); }
            else { return CalculateProjections(week, scraperManager); }
        }

        private List<Player> CalculateProjections(int nextWeek, ScraperManager scraperManager)
        {
            //assemble weekly projections
            List<List<Player>> projections = new List<List<Player>>();
            List<Player> beginSeasonProjections = null;
            List<Player> nextWeekProjections = null;

            int year = GetCurrentYear();
            int currentWeek = GetCurrentWeek();
            int startWeek = currentWeek - 5;

            if (startWeek <= 0) { beginSeasonProjections = scraperManager.ScrapeFantasyPros(0); }

            for (int week = startWeek; week < currentWeek; week++)
            {
                if (week <= 0) { projections.Add(beginSeasonProjections); }
                else { projections.Add(scraperManager.ScrapeNfl(year, week)); }
            }

            //nextWeekProjections = scraperManager.ScrapeFantasyPros(nextWeek);

            return MergeProjections(projections);
        }

        private int GetCurrentYear()
        {
            return 2017;
        }

        private int GetCurrentWeek()
        {
            return 2;
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

            //not adding up right!!! rodgers has 2000 yards when should have 1300

            projections = projections.Where(p => p.GamesPlayed >= 3).ToList();

            foreach (Player player in projections)
            {
                player.PassingYards /= player.PassingYards;
                player.PassingTouchdowns /= player.PassingTouchdowns;
                player.Interceptions /= player.Interceptions;
                player.RushingYards /= player.RushingYards;
                player.RushingTouchdowns /= player.RushingTouchdowns;
                player.Receptions /= player.Receptions;
                player.ReceivingYards /= player.ReceivingYards;
                player.ReceivingYards /= player.ReceivingTouchdowns;
            }

            return projections;
        }

        private Player MergePlayers(Player master, Player other)
        {
            Player mergedPlayer = new Player();
            mergedPlayer.PassingYards = master.PassingYards + other.PassingYards;
            mergedPlayer.PassingTouchdowns = master.PassingTouchdowns + other.PassingTouchdowns;
            mergedPlayer.Interceptions = master.Interceptions + other.Interceptions;
            mergedPlayer.RushingYards = master.RushingYards + other.RushingYards;
            mergedPlayer.RushingTouchdowns = master.RushingTouchdowns + other.RushingTouchdowns;
            mergedPlayer.Receptions = master.Receptions + other.Receptions;
            mergedPlayer.ReceivingYards = master.ReceivingYards + other.ReceivingYards;
            mergedPlayer.ReceivingYards = master.ReceivingTouchdowns + other.ReceivingTouchdowns;
            mergedPlayer.GamesPlayed = master.GamesPlayed + other.GamesPlayed;
            return mergedPlayer;
        }
    }
}
