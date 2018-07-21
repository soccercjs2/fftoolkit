using fftoolkit.DB.Context;
using fftoolkit.DB.Model;
using fftoolkit.Logic.Classes;
using fftoolkit.Logic.Managers;
using fftoolkit.Web.ViewModels;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace fftoolkit.Controllers
{
    public class TradeController : Controller
    {
        private DataContext _context;

        public TradeController()
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
            TradeManager tradeManager = new TradeManager(_context);
            
            League league = leagueManager.Get(id);
            List<Player> players = playerManager.Get(league);
            List<Team> teams = tradeManager.GetTeamsWithPlayers(players, league);

            TradeViewModel model = new TradeViewModel(teams);

            model.League = league;
            model.MyTeam = teams[0];
            model.TheirTeam = teams[1];

            return View(model);
        }

        [HttpPost]
        public ActionResult League(TradeViewModel model)
        {
            if (model.MyTeam == null || model.TheirTeam == null) { return View(model); }

            TradeManager tradeManager = new TradeManager(_context);
            List<Trade> trades = tradeManager.FindTrades(model.MyTeam, model.TheirTeam, model.League);

            model.Trades = trades;

            return View(model);
        }
    }
}