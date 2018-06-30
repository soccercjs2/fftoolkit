using fftoolkit.DB.Model;
using fftoolkit.Logic.Classes;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace fftoolkit.Logic.HostParsers
{
    public class FleaflickerLeagueParser : IHostParser
    {
        private const string LoginUrl = "http://www.fleaflicker.com/nfl/login";
        private const string LeagueTableId = "table_0";
        private const string TeamLinkClass = "league-name";

        public FleaflickerLeagueParser() { }

        public List<Team> ParseLeague(HtmlDocument document, League league)
        {
            //get table containing team names and team urls
            HtmlNode leagueTable = document.GetElementbyId(LeagueTableId);

            //get all the rows that contain the team links
            List<HtmlNode> rows = leagueTable.Descendants().Where(
                        row => row.Attributes.Count > 0 &&
                        row.Name == "div" &&
                        row.Attributes["class"] != null &&
                        row.Attributes["class"].Value.Contains(TeamLinkClass)
            ).ToList<HtmlNode>();

            List<Team> teams = new List<Team>();

            //loop through rows and get anchors with url and team name
            foreach (HtmlNode row in rows)
            {
                HtmlNode anchor = row.Descendants().Where(a => a.Name == "a").FirstOrDefault<HtmlNode>();

                teams.Add(new Team()
                {
                    Name = anchor.InnerHtml.Replace("&#39;", "'"),
                    Url = "http://www.fleaflicker.com" + anchor.Attributes["href"].Value
                });
            }

            return teams;
        }

        public List<Player> ParseTeam(HtmlDocument document)
        {
            List<Player> players = new List<Player>();
            
            //get table containing players of teams
            HtmlNode teamTable = document.GetElementbyId(LeagueTableId);

            //get all rows with id's because they are the rows that have players
            List<HtmlNode> rows = teamTable.Descendants().Where(row => row.Name == "tr" && row.Id != null && row.Id != "").ToList<HtmlNode>();

            foreach (HtmlNode row in rows)
            {
                HtmlNode nameCell = row.SelectSingleNode("./td[1]");
                HtmlNode positionCell = row.SelectSingleNode("./td[2]");
                HtmlNode teamCell = row.SelectSingleNode("./td[3]");

                players.Add(new Player()
                {
                    Name = nameCell.Descendants().Where(a => a.Name == "a").FirstOrDefault<HtmlNode>().InnerText,
                    Position = positionCell.Descendants().Where(s => s.Name == "span").FirstOrDefault<HtmlNode>().InnerText,
                    Team = teamCell.Descendants().Where(s => s.Name == "span").FirstOrDefault<HtmlNode>().InnerText.ToUpper()
                });
            }

            return players;
        }
    }
}