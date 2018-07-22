using fftoolkit.DB.Context;
using fftoolkit.DB.Model;
using fftoolkit.DB.Models;
using fftoolkit.Logic.Classes;
using fftoolkit.Logic.Managers;
using fftoolkit.Logic.Scrapers;
using fftoolkit.Web.ViewModels;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace fftoolkit.Controllers
{
    public class AdminController : Controller
    {
        private DataContext _context;

        public AdminController()
        {
            _context = new DataContext();
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult UpdatePlayers()
        {
            PlayerManager playerManager = new PlayerManager(_context);
            ProjectionManager projectionsManager = new ProjectionManager(_context);

            List<Player> projections = projectionsManager.GetProjections();
            playerManager.DeleteAll();
            playerManager.Add(projections);

            return RedirectToAction("Index");
        }

        public ActionResult MapTeams()
        {
            UserManager userManager = new UserManager(_context);
            User user = userManager.GetCurrentUser(User.Identity.GetUserId());

            if (user == null)
            {
                throw new Exception("No user found;");
            }

            return View(user.Leagues);
        }

        public ActionResult CreateTeamMappingForLeague(int id)
        {
            ScraperManager scraperManager = new ScraperManager(_context);
            LeagueManager leagueManager = new LeagueManager(_context);
            MappingManager adminManager = new MappingManager(_context);

            //get league
            League league = leagueManager.Get(id);

            //get standard teams from fantasy pros
            List<Player> players = scraperManager.ScrapeFantasyPros(0);
            List<string> standardTeams = players.Select(p => p.Team).Distinct().OrderBy(t => t).ToList();
            standardTeams.Insert(0, "");

            ViewBag.StandardTeams = standardTeams;

            //get league data
            List<Team> teams = scraperManager.ScrapeLeague(league);
            List<string> leagueTeams = new List<string>();

            //collect distinct list of teams from each team
            foreach (Team team in teams)
            {
                leagueTeams.AddRange(team.Players.Select(p => p.Team).Distinct());
            }

            //collect distinct list of teams
            leagueTeams = leagueTeams.Select(t => t).Distinct().ToList();

            //get unmapped teams for the league
            List<TeamMapping> unmappedTeams = new List<TeamMapping>();

            foreach (string team in leagueTeams)
            {
                if (!standardTeams.Contains(team) && adminManager.GetTeam(team) == null)
                {
                    unmappedTeams.Add(new TeamMapping() { OldTeam = team });
                }
            }

            return View(unmappedTeams);
        }

        [HttpPost]
        public ActionResult CreateTeamMappingForLeague(List<TeamMapping> teamMappings)
        {
            if (ModelState.IsValid)
            {
                MappingManager adminManager = new MappingManager(_context);

                foreach (TeamMapping teamMapping in teamMappings)
                {
                    if (teamMapping.NewTeam != "")
                    {
                        adminManager.CreateTeamMapping(teamMapping.OldTeam, teamMapping.NewTeam);
                    }
                }

                return RedirectToAction("MapTeams");
            }
            else
            {
                return View(teamMappings);
            }
        }

        public ActionResult MapUnmatchedPlayers()
        {
            PlayerManager playerManager = new PlayerManager(_context);
            ScraperManager scraperManager = new ScraperManager(_context);

            //get players from fantasy pros
            List<Player> players = scraperManager.ScrapeFantasyPros(0);

            //get standard teams from fantasy pros
            List<string> standardTeams = players.Select(p => p.Team).Distinct().OrderBy(t => t).ToList();
            standardTeams.Insert(0, "");

            //get player names from fantasy pros
            List<string> names = players.Select(p => p.Name).Distinct().OrderBy(t => t).ToList();
            names.Insert(0, "");

            //get unmatched players
            List<UnmatchedPlayer> unmatchedPlayers = playerManager.GetUnmatchedPlayers();

            MapUnmatchedPlayersViewModel model = new MapUnmatchedPlayersViewModel();
            model.UnmatchedPlayer = unmatchedPlayers.Take(1).FirstOrDefault();
            model.StandardTeams = standardTeams;
            model.Names = names;
            model.SelectedName = null;
            model.SelectedTeam = null;

            return View(model);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult MapUnmatchedPlayers(MapUnmatchedPlayersViewModel model)
        {
            MappingManager mappingManager = new MappingManager(_context);
            PlayerManager playerManager = new PlayerManager(_context);

            if (model.SelectedName != null && model.SelectedName != "")
            {
                mappingManager.CreateNameMapping(model.UnmatchedPlayer.Name, model.SelectedName);
            }

            if (model.SelectedTeam != null && model.SelectedTeam != "")
            {
                mappingManager.CreateTeamMapping(model.UnmatchedPlayer.Team, model.SelectedTeam);
            }

            playerManager.DeleteUnmatchedPlayer(model.UnmatchedPlayer);

            return RedirectToAction("MapUnmatchedPlayers", "Admin", null);
        }
    }
}