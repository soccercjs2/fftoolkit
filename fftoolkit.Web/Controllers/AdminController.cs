using fftoolkit.DB.Context;
using fftoolkit.DB.Model;
using fftoolkit.DB.Models;
using fftoolkit.Logic.Managers;
using fftoolkit.Logic.Scrapers;
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
            ProjectionManager projectionsManager = new ProjectionManager();

            List<Player> projections = projectionsManager.GetProjections();
            playerManager.DeleteAll();
            playerManager.Add(projections);

            return RedirectToAction("Index");
        }

        public ActionResult MapTeams()
        {
            AdminManager adminManager = new AdminManager(_context);

            List<TeamMapping> teamMappings = adminManager.GetTeamMappings();

            return View(teamMappings);
        }

        public ActionResult CreateTeamMapping()
        {
            ScraperManager scraperManager = new ScraperManager();
            List<Player> players = scraperManager.ScrapeFantasyPros(0);

            List<string> standardTeams = players.Select(p => p.Team).Distinct().ToList();
            ViewBag["StandardTeams"] = standardTeams;

            TeamMapping teamMapping = new TeamMapping();
            return View(teamMapping);
        }
    }
}