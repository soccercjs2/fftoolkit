using fftoolkit.DB.Model;
using fftoolkit.Logic.Tools;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fftoolkit.Logic.Scrapers
{
    public class FantasyProsScraper
    {
        public FantasyProsScraper()
        {

        }

        public List<Player> ScrapeQuarterbacks(string url)
        {
            WebScraper scraper = new WebScraper(null, null, null);
            HtmlDocument document = scraper.Scrape(url);

            //get projection-data table from html
            HtmlNode table = document.GetElementbyId(FantasyProsProjectionTable).Descendants().Where(t => t.Name == "tbody").FirstOrDefault<HtmlNode>();

            //loop through rows in projection table
            foreach (HtmlNode row in table.SelectNodes("./tr"))
            {
                //create new datarow
                Player player = new Player();

                //parse name and team out of player cell
                FantasyProsParser parser = new FantasyProsParser(row.SelectSingleNode("./td[1]"));

                //convert to nfl values
                NflConverter converter = new NflConverter(parser.Name, parser.Team);

                //set row values
                player.Id = projections.SeasonProjectionPlayers.Count + 1;
                player.Name = converter.Name;
                player.Position = "QB";
                player.NflTeam = converter.NflTeam;
                player.PassingYards = decimal.Parse(row.SelectSingleNode("./td[4]").InnerText) / 16;
                player.PassingTouchdowns = decimal.Parse(row.SelectSingleNode("./td[5]").InnerText) / 16;
                player.Interceptions = decimal.Parse(row.SelectSingleNode("./td[6]").InnerText) / 16;
                player.RushingYards = decimal.Parse(row.SelectSingleNode("./td[8]").InnerText) / 16;
                player.RushingTouchdowns = decimal.Parse(row.SelectSingleNode("./td[9]").InnerText) / 16;

                //add datarow to datatable
                projections.SeasonProjectionPlayers.Add(player);
            }
        }
    }
}
