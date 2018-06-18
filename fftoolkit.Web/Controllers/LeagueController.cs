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

            League league = new League()
            {
                UserId = user.UserId,
                Quarterbacks = 1,
                RunningBacks = 2,
                WideReceivers = 2,
                TightEnds = 1,
                Flexes = 1,
                PointsPerReception = 1,
                PointsPerPassingTouchdown = 4,
                PointsPerPassingYard = 0.25M,
                PointsLostPerInterception = 2
            };

            return View("Edit", league);
        }

        public ActionResult Edit(int id)
        {
            LeagueManager leagueManager = new LeagueManager(_context);
            League league = leagueManager.Get(id) ?? throw new Exception("No league found.");
            
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

        public ActionResult Delete(int id)
        {
            LeagueManager leagueManager = new LeagueManager(_context);
            leagueManager.Delete(id);

            return RedirectToAction("Index");
        }
    }
}