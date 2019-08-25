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

                    case "WR":
                        player.Receptions = decimal.Parse(row.SelectSingleNode("./td[5]").InnerText) / 16;
                        player.ReceivingYards = decimal.Parse(row.SelectSingleNode("./td[6]").InnerText) / 16;
                        player.ReceivingTouchdowns = decimal.Parse(row.SelectSingleNode("./td[7]").InnerText) / 16;
                        break;

                    case "TE":
                        player.Receptions = decimal.Parse(row.SelectSingleNode("./td[2]").InnerText) / 16;
                        player.ReceivingYards = decimal.Parse(row.SelectSingleNode("./td[3]").InnerText) / 16;
                        player.ReceivingTouchdowns = decimal.Parse(row.SelectSingleNode("./td[4]").InnerText) / 16;
                        break;

                    case "K":
                        player.FantasyPoints = decimal.Parse(row.SelectSingleNode("./td[5]").InnerText) / 16;
                        break;

                    case "DST":
                        player.FantasyPoints = decimal.Parse(row.SelectSingleNode("./td[10]").InnerText) / 16;

                        if (player.Name == "Arizona Cardinals") { player.Team = "ARI"; }
                        else if (player.Name == "Atlanta Falcons") { player.Team = "ATL"; }
                        else if (player.Name == "Baltimore Ravens") { player.Team = "BAL"; }
                        else if (player.Name == "Buffalo Bills") { player.Team = "BUF"; }
                        else if (player.Name == "Carolina Panthers") { player.Team = "CAR"; }
                        else if (player.Name == "Chicago Bears") { player.Team = "CHI"; }
                        else if (player.Name == "Cincinnati Bengals") { player.Team = "CIN"; }
                        else if (player.Name == "Cleveland Browns") { player.Team = "CLE"; }
                        else if (player.Name == "Dallas Cowboys") { player.Team = "DAL"; }
                        else if (player.Name == "Denver Broncos") { player.Team = "DEN"; }
                        else if (player.Name == "Detroit Lions") { player.Team = "DET"; }
                        else if (player.Name == "Green Bay Packers") { player.Team = "GB"; }
                        else if (player.Name == "Houston Texans") { player.Team = "HOU"; }
                        else if (player.Name == "Indianapolis Colts") { player.Team = "IND"; }
                        else if (player.Name == "Jacksonville Jaguars") { player.Team = "JAC"; }
                        else if (player.Name == "Kansas City Chiefs") { player.Team = "KC"; }
                        else if (player.Name == "Los Angeles Chargers") { player.Team = "LAC"; }
                        else if (player.Name == "Los Angeles Rams") { player.Team = "LAR"; }
                        else if (player.Name == "Miami Dolphins") { player.Team = "MIA"; }
                        else if (player.Name == "Minnesota Vikings") { player.Team = "MIN"; }
                        else if (player.Name == "New England Patriots") { player.Team = "NE"; }
                        else if (player.Name == "New Orleans Saints") { player.Team = "NO"; }
                        else if (player.Name == "New York Giants") { player.Team = "NYG"; }
                        else if (player.Name == "New York Jets") { player.Team = "NYJ"; }
                        else if (player.Name == "Oakland Raiders") { player.Team = "OAK"; }
                        else if (player.Name == "Philadelphia Eagles") { player.Team = "PHI"; }
                        else if (player.Name == "Pittsburgh Steelers") { player.Team = "PIT"; }
                        else if (player.Name == "Seattle Seahawks") { player.Team = "SEA"; }
                        else if (player.Name == "San Francisco 49ers") { player.Team = "SF"; }
                        else if (player.Name == "Tampa Bay Buccaneers") { player.Team = "TB"; }
                        else if (player.Name == "Tennessee Titans") { player.Team = "TEN"; }
                        else if (player.Name == "Washington Redskins") { player.Team = "WAS"; }
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
