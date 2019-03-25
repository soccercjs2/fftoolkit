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
            List<Player> players = (List<Player>)Session["Players"];
            if (players == null)
            {
                players = playerManager.Get(league);
                players = tradeManager.GetTradeValues(players, league);
                Session["Players"] = players;
            }                

            //build teams with players
            List<Team> teams = (List<Team>)Session["TeamsWithPlayers"];
            if (teams == null)
            {
                teams = tradeManager.GetTeamsWithPlayers(players, league);
                Session["TeamsWithPlayers"] = teams;
            }

            TradesViewModel model = new TradesViewModel(teams)
            {
                League = league,
                MyTeam = teams[0],
                TheirTeam = teams[1]
            };

            return View(model);
        }

        [HttpPost]
        public ActionResult FindTrades(TradesViewModel model)
        {
            if (model.MyTeam == null || model.TheirTeam == null) { return View(model); }

            List<Trade> trades = (List<Trade>)Session["Trades"];

            if (trades == null)
            {
                TradeManager tradeManager = new TradeManager(_context);
                trades = new List<Trade>(); //tradeManager.FindTrades(model.MyTeam, model.TheirTeam, model.League);
                Team myTeam = model.Teams[0];

                for (int i = 0; i < model.Teams.Count; i++)
                {
                    Team theirTeam = model.Teams[i];

                    if (myTeam.Name != theirTeam.Name)
                    {
                        List<Trade> teamTrades = tradeManager.FindTrades(myTeam, theirTeam, model.League);
                        trades.AddRange(teamTrades);
                    }
                }

                trades = trades
                    .OrderByDescending(t => t.MyTradeSide.Difference * t.TheirTradeSide.Difference)
                    .ToList();

                Session["Trades"] = trades;
            }

            model.Trades = trades;

            return View(model);
        }
    }
}