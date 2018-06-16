using fftoolkit.DB.Context;
using fftoolkit.DB.Model;
using fftoolkit.Logic.Managers;
using Microsoft.AspNet.Identity;
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
            _user = userWorker.GetCurrentUser(User.Identity.GetUserId());
        }

        public ActionResult Index()
        {
            if (_user == null)
            {
                throw new Exception("No user found;");
            }

            return View(_user.Leagues);
        }

        public ActionResult Add()
        {
            League league = new League();
            league.UserId = _user.UserId;

            return View(league);
        }

        public ActionResult Edit()
        {
            return View();
        }
    }
}