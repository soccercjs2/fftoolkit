using fftoolkit.DB.Models;
using fftoolkit.Logic.Classes;
using HtmlAgilityPack;
using System.Collections.Generic;
using System.Linq;

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
            //HtmlNode leagueTable = document.GetElementbyId(LeagueTableId);
            //leagueTable = document.DocumentNode

            //get all the rows that contain the team links
            List<HtmlNode> rows = document.DocumentNode.Descendants().Where(
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

            //get all rows with id's because they are the rows that have players
            List<HtmlNode> playerNodes = document.DocumentNode.Descendants().Where(row => row.Name == "div" && row.HasClass("player")).ToList<HtmlNode>();

            foreach (HtmlNode playerNode in playerNodes)
            {
                HtmlNode nameNode = playerNode.Descendants().Where(n => n.HasClass("player-text")).FirstOrDefault();
                HtmlNode teamNode = playerNode.Descendants().Where(n => n.HasClass("player-team")).FirstOrDefault();
                HtmlNode positionNode = playerNode.Descendants().Where(n => n.HasClass("position")).FirstOrDefault();

                players.Add(new Player()
                {
                    Name = nameNode.InnerText,
                    Position = positionNode.InnerText,
                    Team = teamNode.InnerText
                });
            }

            return players;
        }
    }
}