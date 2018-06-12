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
    public class LeagueController : Controller
    {
        private DataContext _context;
        private User _user;

        public LeagueController()
        {
            _context = new DataContext();

            UserManager userWorker = new UserManager(_context);
            _user = userWorker.GetCurrentUser();
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Edit()
        {
            return View();
        }
    }
}