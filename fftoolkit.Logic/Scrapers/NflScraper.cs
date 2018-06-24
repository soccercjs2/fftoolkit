using fftoolkit.DB.Model;
using fftoolkit.Logic.Tools;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fftoolkit.Logic.Scrapers
{
    public class NflScraper
    {
        private const string GamesPlayed = "1";
        private const string PassingYards = "5";
        private const string PassingTouchdowns = "6";
        private const string Interceptions = "7";
        private const string RushingYards = "14";
        private const string RushingTouchdowns = "15";
        private const string Receptions = "20";
        private const string ReceivingYards = "21";
        private const string ReceivingTouchdowns = "22";

        public NflScraper() { }

        public List<Player> Scrape(int year, int week)
        {
            WebScraper scraper = new WebScraper(null, null, null);
            JObject json = scraper.ScrapeJson(String.Format("http://api.fantasy.nfl.com/v1/players/stats?statType=weekStats&season={0}&week={1}&format=json", year, week));
            List<Player> statistics = new List<Player>();

            var players =
                from player in json["players"]
                select player;

            //loop through rows in projection table
            foreach (JObject jPlayer in players)
            {
                //create new datarow
                Player player = new Player();

                //create fantasy pro converter
                PlayerConverter converter = new PlayerConverter(jPlayer["name"].ToString(), jPlayer["teamAbbr"].ToString());

                //set row values
                player.Name = converter.Name;
                player.Team = converter.Team;
                player.Position = jPlayer["position"].ToString().ToUpper();
                player.GamesPlayed = 1;

                if (player.Position == "QB")
                {
                    player.PassingYards = decimal.Parse(IsNullStat(jPlayer["stats"][PassingYards]));
                    player.PassingTouchdowns = decimal.Parse(IsNullStat(jPlayer["stats"][PassingTouchdowns]));
                    player.Interceptions = decimal.Parse(IsNullStat(jPlayer["stats"][Interceptions]));
                    player.RushingYards = decimal.Parse(IsNullStat(jPlayer["stats"][RushingYards]));
                    player.RushingTouchdowns = decimal.Parse(IsNullStat(jPlayer["stats"][RushingTouchdowns]));
                    statistics.Add(player);
                }
                if (player.Position == "RB")
                {
                    player.RushingYards = decimal.Parse(IsNullStat(jPlayer["stats"][RushingYards]));
                    player.RushingTouchdowns = decimal.Parse(IsNullStat(jPlayer["stats"][RushingTouchdowns]));
                    player.Receptions = decimal.Parse(IsNullStat(jPlayer["stats"][Receptions]));
                    player.ReceivingYards = decimal.Parse(IsNullStat(jPlayer["stats"][ReceivingYards]));
                    player.ReceivingTouchdowns = decimal.Parse(IsNullStat(jPlayer["stats"][ReceivingTouchdowns]));
                    statistics.Add(player);
                }
                if (player.Position == "WR")
                {
                    player.RushingYards = decimal.Parse(IsNullStat(jPlayer["stats"][RushingYards]));
                    player.RushingTouchdowns = decimal.Parse(IsNullStat(jPlayer["stats"][RushingTouchdowns]));
                    player.Receptions = decimal.Parse(IsNullStat(jPlayer["stats"][Receptions]));
                    player.ReceivingYards = decimal.Parse(IsNullStat(jPlayer["stats"][ReceivingYards]));
                    player.ReceivingTouchdowns = decimal.Parse(IsNullStat(jPlayer["stats"][ReceivingTouchdowns]));
                    statistics.Add(player);
                }
                if (player.Position == "TE")
                {
                    player.Receptions = decimal.Parse(IsNullStat(jPlayer["stats"][Receptions]));
                    player.ReceivingYards = decimal.Parse(IsNullStat(jPlayer["stats"][ReceivingYards]));
                    player.ReceivingTouchdowns = decimal.Parse(IsNullStat(jPlayer["stats"][ReceivingTouchdowns]));
                    statistics.Add(player);
                }
            }

            return statistics;
        }

        private string IsNullStat(object o)
        {
            if (o == null) { return "0"; }
            else { return o.ToString(); }
        }
    }
}
