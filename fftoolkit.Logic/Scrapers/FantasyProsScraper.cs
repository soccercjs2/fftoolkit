using fftoolkit.DB.Models;
using fftoolkit.Logic.Parsers;
using fftoolkit.Logic.Tools;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;

namespace fftoolkit.Logic.Scrapers
{
    public class FantasyProsScraper
    {
        private const string _dataTableName = "data";

        public FantasyProsScraper()
        {

        }

        public List<Player> Scrape(string position, int week)
        {
            if (position == null || position == "")
            {
                throw new Exception("Position cannot be empty.");
            }

            if (week < 0 || week > 17)
            {
                throw new Exception("Invalid week.");
            }

            WebScraper scraper = new WebScraper(null, null, null);
            HtmlDocument document = scraper.Scrape(GetUrl(position, week));
            List<Player> players = new List<Player>();

            //get projection-data table from html
            HtmlNode table = document.GetElementbyId(_dataTableName).Descendants().Where(t => t.Name == "tbody").FirstOrDefault<HtmlNode>();

            //loop through rows in projection table
            foreach (HtmlNode row in table.SelectNodes("./tr"))
            {
                //create new datarow
                Player player = new Player();

                //parse name and team out of player cell
                FantasyProsParser parser = new FantasyProsParser(row.SelectSingleNode("./td[1]"));
                
                //set row values
                player.Name = parser.Player;
                player.Position = position;
                player.Team = parser.Team;
                player.GamesPlayed = 1;

                switch (position) {
                    case "QB":
                        player.PassingYards = decimal.Parse(row.SelectSingleNode("./td[4]").InnerText) / 16;
                        player.PassingTouchdowns = decimal.Parse(row.SelectSingleNode("./td[5]").InnerText) / 16;
                        player.Interceptions = decimal.Parse(row.SelectSingleNode("./td[6]").InnerText) / 16;
                        player.RushingYards = decimal.Parse(row.SelectSingleNode("./td[8]").InnerText) / 16;
                        player.RushingTouchdowns = decimal.Parse(row.SelectSingleNode("./td[9]").InnerText) / 16;
                        break;

                    case "RB":
                        player.RushingYards = decimal.Parse(row.SelectSingleNode("./td[3]").InnerText) / 16;
                        player.RushingTouchdowns = decimal.Parse(row.SelectSingleNode("./td[4]").InnerText) / 16;
                        player.Receptions = decimal.Parse(row.SelectSingleNode("./td[5]").InnerText) / 16;
                        player.ReceivingYards = decimal.Parse(row.SelectSingleNode("./td[6]").InnerText) / 16;
                        player.ReceivingTouchdowns = decimal.Parse(row.SelectSingleNode("./td[7]").InnerText) / 16;
                        break;

                    case "WR":;
                        player.Receptions = decimal.Parse(row.SelectSingleNode("./td[5]").InnerText) / 16;
                        player.ReceivingYards = decimal.Parse(row.SelectSingleNode("./td[6]").InnerText) / 16;
                        player.ReceivingTouchdowns = decimal.Parse(row.SelectSingleNode("./td[7]").InnerText) / 16;
                        break;

                    case "TE":
                        player.Receptions = decimal.Parse(row.SelectSingleNode("./td[2]").InnerText) / 16;
                        player.ReceivingYards = decimal.Parse(row.SelectSingleNode("./td[3]").InnerText) / 16;
                        player.ReceivingTouchdowns = decimal.Parse(row.SelectSingleNode("./td[4]").InnerText) / 16;
                        break;

                    default:
                        break;

                }
                

                //add datarow to datatable
                players.Add(player);
            }

            return players;
        }

        private string GetUrl(string position, int week)
        {
            return String.Format("https://www.fantasypros.com/nfl/projections/{0}.php?week={1}", 
                position.ToLower(),
                (week == 0) ? "draft" : week.ToString());
        }
    }
}
