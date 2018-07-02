using fftoolkit.DB.Context;
using fftoolkit.DB.Model;
using fftoolkit.Logic.Managers;
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

        public ActionResult Map()
        {
            return View();
        }

        public ActionResult Team()
        {

        }
    }
}