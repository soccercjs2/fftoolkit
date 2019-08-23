using fftoolkit.DB.Models;
using fftoolkit.Logic.Classes;
using HtmlAgilityPack;
using System.Collections.Generic;
using System.Linq;

namespace fftoolkit.Logic.HostParsers
{
    public class YahooParser : IHostParser
    {
        private const string LoginUrl = "https://www.yahoo.com";
        private const string LeagueTableId = "standingstable";
        private const string TeamLinkClass = "Linkable";
        private const string TeamTableId = "statTable0";
        private const string PlayerClass = "ysf-player-name";

        public YahooParser() { }

        public List<Team> ParseLeague(HtmlDocument document, League league)
        {
            //get table containing team names and team urls
            HtmlNode leagueTable = document.GetElementbyId(LeagueTableId);

            //get all the rows that contain the team links
            List<HtmlNode> rows = leagueTable.Descendants().Where(
                        row => row.Attributes.Count > 0 &&
                        row.Name == "tr" &&
                        row.Attributes["class"] != null &&
                        row.Attributes["class"].Value.Contains(TeamLinkClass)
            ).ToList<HtmlNode>();

            List<Team> teams = new List<Team>();
            
            //loop through rows and get anchors with url and team name
            foreach (HtmlNode row in rows)
            {
                List<HtmlNode> anchors = row.Descendants().Where(a => a.Name == "a").ToList<HtmlNode>();
                HtmlNode anchor = anchors[1];

                teams.Add(new Team()
                {
                    Name = anchor.InnerHtml.Replace("&#39;", "'"),
                    Url = "https://football.fantasysports.yahoo.com" + anchor.Attributes["href"].Value
                });
            }

            return teams;
        }

        public List<Player> ParseTeam(HtmlDocument document)
        {
            List<Player> players = new List<Player>();

            //get table containing players of teams
            HtmlNode teamTable = document.GetElementbyId(TeamTableId);

            //get all rows with id's because they are the rows that have players
            List<HtmlNode> playerContainers = teamTable.Descendants().Where(row => row.Name == "div" && 
                                                                row.Attributes["class"] != null &&
                                                                row.Attributes["class"].Value.Contains(PlayerClass)
            ).ToList<HtmlNode>();

            foreach (HtmlNode playerContainer in playerContainers)
            {
                if (playerContainer != null && playerContainer.Id != null)
                {
                    HtmlNode nameAnchor = playerContainer.Descendants().Where(a => a.Name == "a" &&
                                                                              a.Attributes["class"] != null &&
                                                                              a.Attributes["class"].Value.Contains("name")).FirstOrDefault<HtmlNode>();

                    //if nameanchor is null, then roster slot is empty
                    if (nameAnchor != null)
                    {
                        HtmlNode teamPositionSpan = playerContainer.Descendants().Where(a => a.Name == "span").FirstOrDefault<HtmlNode>();

                        //find indecies of start of team and position
                        int positionStart = teamPositionSpan.InnerText.LastIndexOf(" ") + 1;
                        int teamLength = teamPositionSpan.InnerText.IndexOf(" ");

                        //get player attributes
                        players.Add(new Player()
                        {
                            Name = nameAnchor.InnerText,
                            Position = teamPositionSpan.InnerText.Substring(positionStart),
                            Team = teamPositionSpan.InnerText.Substring(0, teamLength).ToUpper()
                        });
                    }
                }
            }

            return players;
        }
    }
}