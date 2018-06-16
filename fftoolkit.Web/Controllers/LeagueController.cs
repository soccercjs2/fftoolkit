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

        public LeagueController()
        {
            _context = new DataContext();
        }

        public ActionResult Index()
        {
            UserManager userManager = new UserManager(_context);
            User user = userManager.GetCurrentUser(User.Identity.GetUserId());

            if (user == null)
            {
                throw new Exception("No user found;");
            }

            return View(user.Leagues);
        }

        public ActionResult Add()
        {
            UserManager userManager = new UserManager(_context);
            User user = userManager.GetCurrentUser(User.Identity.GetUserId());

            League league = new League();
            league.UserId = user.UserId;

            return View("Edit", league);
        }

        public ActionResult Edit(int leagueId)
        {
            LeagueManager leagueManager = new LeagueManager(_context);
            League league = leagueManager.Get(leagueId) ?? throw new Exception("No league found.");
            
            return View(league);
        }

        [HttpPost]
        public ActionResult Edit(League league)
        {
            if (ModelState.IsValid)
            {
                LeagueManager leagueManager = new LeagueManager(_context);

                if (league.LeagueId == 0)
                    leagueManager.Add(league);
                else
                    leagueManager.Update(league);

                return RedirectToAction("Index");
            }
            else
            {
                return View(league);
            }
        }
    }
}