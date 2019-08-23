using fftoolkit.DB.Models;
using fftoolkit.Logic.Classes;
using HtmlAgilityPack;
using System.Collections.Generic;
using System.Linq;

namespace fftoolkit.Logic.HostParsers
{
    public class EspnParser : IHostParser
    {
        private const string LoginUrl = "https://registerdisney.go.com/jgc/v2/client/ESPN-FANTASYLM-PROD/guest/login?langPref=en-US";
        //private const string LoginUrl = "https://espn.go.com/login/";
        private const string LeagueTableId = "xstandTbl_div";
        private const string TeamLinkClass = "sortableRow";
        private const string TeamTableId = "playertable_0";

        public EspnParser() { }

        public List<Team> ParseLeague(HtmlDocument document, League league)
        {
            //get table containing team names and team urls
            HtmlNodeCollection leagueTables = document.DocumentNode.SelectNodes("//table[contains(@id,'" + LeagueTableId + "')]");

            List<HtmlNode> rows = new List<HtmlNode>();

            //get all the rows that contain the team links
            foreach (HtmlNode leagueTable in leagueTables)
            {
                rows.AddRange(
                    leagueTable.Descendants().Where(
                        row => row.Attributes.Count > 0 &&
                        row.Attributes["class"] != null &&
                        row.Attributes["class"].Value.Contains(TeamLinkClass)
                    ).ToList<HtmlNode>()
                );
            }

            List<Team> teams = new List<Team>();
            
            //loop through rows and get anchors with url and team name
            foreach (HtmlNode row in rows)
            {
                HtmlNode anchor = row.Descendants().Where(a => a.Name == "a").FirstOrDefault<HtmlNode>();

                teams.Add(new Team()
                {
                    Name = anchor.InnerHtml.Replace("&#39;", "'"),
                    Url = "http://games.espn.go.com" + anchor.Attributes["href"].Value.Replace("&amp;", "&")
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
            List<HtmlNode> rows = teamTable.Descendants().Where(row => row.Name == "tr" && row.Id != null && row.Id != "").ToList<HtmlNode>();

            foreach (HtmlNode row in rows)
            {
                HtmlNode playerCell = row.SelectSingleNode("./td[2]");

                if (playerCell != null && playerCell.Id != null)
                {
                    HtmlNode statusNode = playerCell.Descendants().Where(s => s.Name == "span").FirstOrDefault<HtmlNode>();
                    if (statusNode != null) { playerCell.RemoveChild(statusNode); }

                    string playerTeamPosition = playerCell.InnerText.Replace("&nbsp;", " ").Trim();

                    //find indecies of start of team and position
                    int positionStart = playerTeamPosition.LastIndexOf(" ") + 1;
                    int teamStart = playerTeamPosition.Substring(0, positionStart - 1).LastIndexOf(" ");

                    //get player attributes
                    players.Add(new Player()
                    {
                        Name = playerTeamPosition.Substring(0, teamStart - 1),
                        Position = playerTeamPosition.Substring(positionStart, playerTeamPosition.Length - positionStart).Trim(),
                        Team = playerTeamPosition.Substring(teamStart, positionStart - teamStart - 1).Trim().ToUpper()
                    });
                }
            }

            return players;
        }
    }
}