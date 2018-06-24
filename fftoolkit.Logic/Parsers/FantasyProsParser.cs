using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fftoolkit.Logic.Parsers
{
    public class FantasyProsParser
    {
        public string Player { get; set; }
        public string Team { get; set; }

        public FantasyProsParser(HtmlNode player)
        {
            Player = GetNameFromFantasyProPlayer(player);
            Team = GetTeamFromFantasyProPlayer(player);
        }

        public string GetNameFromFantasyProPlayer(HtmlNode playerData)
        {
            List<HtmlNode> nodes = playerData.Descendants().ToList<HtmlNode>();
            HtmlNode anchor = playerData.Descendants().Where(t => t.Name == "a").FirstOrDefault<HtmlNode>();
            return anchor.InnerText;
        }

        public string GetTeamFromFantasyProPlayer(HtmlNode playerData)
        {
            List<HtmlNode> nodes = playerData.Descendants().ToList<HtmlNode>();
            HtmlNode team = nodes[2];

            if (team == null) { return null; }
            else { return team.InnerText.Trim(); }
        }
    }
}
