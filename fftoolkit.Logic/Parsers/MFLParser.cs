using fftoolkit.DB.Models;
using fftoolkit.Logic.Classes;
using HtmlAgilityPack;
using System.Collections.Generic;
using System.Linq;

namespace fftoolkit.Logic.HostParsers
{
    public class MFLParser : IHostParser
    {
        private const string LoginUrl = "www60.myfantasyleague.com";
        private const string LeagueTableId = "standings";
        private const string TeamLinkClass = "franchise_";
        private const string TeamTableId = "single_roster";

        public MFLParser() { }

        public List<Team> ParseLeague(HtmlDocument document, League league)
        {
            //get table containing team names and team urls
            HtmlNode leagueTable = document.GetElementbyId(LeagueTableId);

            //get all the rows that contain the team links
            List<HtmlNode> rows = leagueTable.Descendants().Where(
                        row => row.Attributes.Count > 0 &&
                        row.Name == "tr" &&
                        row.Attributes["class"] != null
            ).ToList<HtmlNode>();

            List<Team> teams = new List<Team>();

            //loop through rows and get anchors with url and team name
            foreach (HtmlNode row in rows)
            {
                HtmlNode anchor = row.Descendants().Where(a => a.Name == "a" &&
                                                        a.Attributes["class"] != null &&
                                                        a.Attributes["class"].Value.Contains(TeamLinkClass))
                .FirstOrDefault<HtmlNode>();

                teams.Add(new Team()
                {
                    Name = anchor.InnerHtml.Replace("&#39;", "'"),
                    Url = anchor.Attributes["href"].Value.Replace("O=01", "O=07")
                });
            }

            return teams;
        }

        public List<Player> ParseTeam(HtmlDocument document)
        {
            List<Player> players = new List<Player>();

            //get table containing players of teams
            HtmlNode teamTable = document.GetElementbyId(TeamTableId).Descendants().Where(t => t.Name == "table").FirstOrDefault<HtmlNode>();

            //get all rows with id's because they are the rows that have players
            List<HtmlNode> rows = teamTable.Descendants().Where(row => row.Name == "tr" && 
                                                                row.Attributes.Count > 0 && 
                                                                row.Attributes["class"] != null && 
                                                                row.Attributes["class"].Value.ToString() != "").ToList<HtmlNode>();

            foreach (HtmlNode row in rows)
            {
                HtmlNode playerCell = row.SelectSingleNode("./td[1]");

                if (playerCell != null && playerCell.Attributes["class"] != null && playerCell.Attributes["class"].Value == "player")
                {
                    string playerTeamPosition = playerCell.Descendants().Where(a => a.Name == "a").FirstOrDefault<HtmlNode>().InnerText;

                    //find indecies of start of team and position
                    int positionStart = playerTeamPosition.LastIndexOf(" ") + 1;
                    int teamStart = playerTeamPosition.Substring(0, positionStart - 1).LastIndexOf(" ");

                    //split player names into array to rearange them
                    string[] playerNames = playerTeamPosition.Substring(0, teamStart).Split(',');

                    //get player attributes
                    players.Add(new Player()
                    {
                        Name = playerNames[1].Trim() + " " + playerNames[0].Trim(),
                        Position = playerTeamPosition.Substring(positionStart, playerTeamPosition.Length - positionStart).Trim(),
                        Team = playerTeamPosition.Substring(teamStart, positionStart - teamStart - 1).Trim().ToUpper()
                    });
                }
            }

            return players;
        }
    }
}