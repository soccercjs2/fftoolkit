using fftoolkit.DB.Context;
using fftoolkit.DB.Model;
using fftoolkit.Logic.Managers;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace fftoolkit.Controllers
{
    public class PlayerController : Controller
    {
        private DataContext _context;

        public PlayerController()
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

        public ActionResult League(int id)
        {
            LeagueManager leagueManager = new LeagueManager(_context);
            PlayerManager playerManager = new PlayerManager(_context);
            TradeManager tradeManager = new TradeManager();

            League league = leagueManager.Get(id);
            List<Player> players = playerManager.Get(league);
            players = tradeManager.GetTradeValues(players, league);

            if (players == null || players.Count == 0)
            {
                throw new Exception("No players were loaded.");
            }

            return View(players);
        }
    }
}