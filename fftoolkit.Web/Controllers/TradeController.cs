using fftoolkit.DB.Context;
using fftoolkit.DB.Model;
using fftoolkit.Logic.Classes;
using fftoolkit.Logic.Managers;
using fftoolkit.Web.ViewModels;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json;
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

            //try get players from session
            List<Player> players = (List<Player>)Session["Players"];

            //try get players from json file
            if (players == null)
            {
                players = JsonConvert.DeserializeObject<List<Player>>(System.IO.File.ReadAllText(@"C:\Users\Collin\Source\Repos\fftoolkit\fftoolkit.Logic\Json Data\players.json"));
            }

            //scrape players
            if (players == null)
            {
                players = playerManager.Get(league);
                players = tradeManager.GetTradeValues(players, league);
                Session["Players"] = players;
            }                

            //build teams with players

            //try get teams from session
            List<Team> teams = (List<Team>)Session["TeamsWithPlayers"];

            //try get teams from json file
            if (teams == null)
            {
                teams = JsonConvert.DeserializeObject<List<Team>>(System.IO.File.ReadAllText(@"C:\Users\Collin\Source\Repos\fftoolkit\fftoolkit.Logic\Json Data\teams.json"));
            }

            //scrape teams
            if (teams == null)
            {
                teams = tradeManager.GetTeamsWithPlayers(players, league);
                Session["TeamsWithPlayers"] = teams;
            }

            TradesViewModel model = new TradesViewModel(teams)
            {
                League = league
            };

            return View(model);
        }

        [HttpPost]
        public ActionResult FindTrades(TradesViewModel model)
        {
            if (model.MyTeam == null) { return View(model); }

            List<Trade> trades = (List<Trade>)Session["Trades"];

            if (trades == null)
            {
                TradeManager tradeManager = new TradeManager(_context);
                trades = new List<Trade>(); //tradeManager.FindTrades(model.MyTeam, model.TheirTeam, model.League);
                Team myTeam = model.Teams[11];

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

            return PartialView("TradeList", model);
        }

        [HttpPost]
        public ActionResult ChangePage(TradesViewModel model)
        {
            List<Trade> trades = (List<Trade>)Session["Trades"];
            model.Trades = trades;
            return PartialView("TradeList", model);
        }

        public ActionResult GetTradeDetail(int id)
        {
            List<Trade> trades = (List<Trade>)Session["Trades"];
            Trade trade = trades.Where(t => t.TradeId == id).FirstOrDefault();
            return PartialView("TradeDetail", trade);
        }

        [HttpPost]
        public ActionResult SetFilterTeam(TradesViewModel model)
        {
            Team selectedTeam = model.Teams.Where(t => t.TeamId == model.SelectedTeamId).FirstOrDefault();

            if (model.TeamSelectorMode == "TheirTeam")
            {
                model.TheirTeam = selectedTeam;
                model.TheirFilters = new List<TradeFilterViewModel>(new TradeFilterViewModel[4]);
            }
            else if (model.TeamSelectorMode == "MyTeam")
            {
                model.MyTeam = selectedTeam;
                model.MyFilters = new List<TradeFilterViewModel>(new TradeFilterViewModel[4]);
            }

            model.SelectedTeamId = 0;

            return PartialView("TradeFilter", model);
        }
    }
}