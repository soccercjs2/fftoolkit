using fftoolkit.DB.Context;
using fftoolkit.DB.Models;
using fftoolkit.Logic.Classes;
using fftoolkit.Logic.HostParsers;
using fftoolkit.Logic.Scrapers;
using fftoolkit.Logic.Tools;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;

namespace fftoolkit.Logic.Managers
{
    public class ScraperManager
    {
        private DataContext _context;

        public ScraperManager(DataContext context)
        {
            _context = context ?? throw new Exception("The context cannot be null.");
        }

        public List<Player> ScrapeFantasyPros(int week)
        {
            FantasyProsScraper fantasyProsScraper = new FantasyProsScraper();
            List<Player> projections = new List<Player>();

            projections.AddRange(fantasyProsScraper.Scrape("QB", week));
            projections.AddRange(fantasyProsScraper.Scrape("RB", week));
            projections.AddRange(fantasyProsScraper.Scrape("WR", week));
            projections.AddRange(fantasyProsScraper.Scrape("TE", week));
            projections.AddRange(fantasyProsScraper.Scrape("K", week));
            projections.AddRange(fantasyProsScraper.Scrape("DST", week));

            return projections;
        }

        public List<Player> ScrapeNfl(int year, int week)
        {
            NflScraper nflScraper = new NflScraper();
            List<Player> players = ConvertPlayers(nflScraper.Scrape(year, week));
            return players;
        }

        public List<Team> ScrapeLeague(League league)
        {
            WebScraper scraper = new WebScraper();
            LeagueWrapper leagueWrapper = new LeagueWrapper(league);
            IHostParser parser = null;

            //determine which scraper to use based on url
            if (league.Url.Contains("fleaflicker.com")) { parser = new FleaflickerLeagueParser(); }
            else if (league.Url.Contains("myfantasyleague.com")) { parser = new MFLParser(); }
            else if (league.Url.Contains("games.espn.go.com")) { parser = new EspnParser(); }
            else if (league.Url.Contains("football.fantasysports.yahoo.com")) { parser = new YahooParser(); }
            //else if (league.Url.Contains("fantasy.nfl.com")) { parser = new NflParser(); }
            //else if (league.Url.Contains("football.cbssports.com")) { parser = new CbsSportsParser(); }
            else
            {
                //throw exceptions saying league host not supported
            }

            HtmlDocument leagueHomePage = scraper.Scrape(league.Url);
            List<Team> teams = parser.ParseLeague(leagueHomePage, league);

            for (int i = 0; i < teams.Count; i++)
            {
                teams[i].TeamId = i + 1;
                HtmlDocument teamPage = scraper.Scrape(teams[i].Url);
                teams[i].Players = ConvertPlayers(parser.ParseTeam(teamPage));
            }

            return teams;
        }

        private List<Player> ConvertPlayers(List<Player> players)
        {
            MappingManager mappingManager = new MappingManager(_context);
            Dictionary<string, string> nameMappings = mappingManager.GetNameMappings();
            Dictionary<string, string> teamMappings = mappingManager.GetTeamMappings();

            foreach (Player player in players)
            {
                player.Name = (nameMappings.ContainsKey(player.Name)) ? nameMappings[player.Name] : player.Name;
                player.Team = (teamMappings.ContainsKey(player.Team)) ? teamMappings[player.Team] : player.Team;
            }

            return players;
        }
    }
}
