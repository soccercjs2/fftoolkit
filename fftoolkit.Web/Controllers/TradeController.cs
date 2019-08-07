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
using System.Threading.Tasks;
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

            Team myTeam = (Team)Session["MyTeam"];
            Team theirTeam = (Team)Session["TheirTeam"];

            //if teams have changed, clear trades from session
            //else, get trades from session
            List<Trade> trades = null;
            if ((myTeam != null &&!myTeam.Equals(model.MyTeam)) ||
                (theirTeam != null && model.TheirTeam != null && !theirTeam.Equals(model.TheirTeam)))
            {
                Session["Trades"] = null;
            }
            else
            {
                trades = (List<Trade>)Session["Trades"];
            }

            Session["MyTeam"] = model.MyTeam;
            Session["TheirTeam"] = model.TheirTeam;

            //if no applicable trades in session, find trades
            if (trades == null)
            {
                TradeManager tradeManager = new TradeManager(_context);
                trades = new List<Trade>();

                if (model.TheirTeam != null)
                {
                    //find trades with their team
                    List<Trade> teamTrades = tradeManager.FindTrades(model.MyTeam, model.TheirTeam, model.League);
                    trades.AddRange(teamTrades);
                }
                else
                {
                    //find trades with all teams
                    Parallel.ForEach(model.Teams, otherTeam =>
                    {
                        if (model.MyTeam.TeamId != otherTeam.TeamId)
                        {
                            trades.AddRange(tradeManager.FindTrades(model.MyTeam, otherTeam, model.League));
                        }
                    });
                }

                //order trades by those most valuable to both
                trades = trades
                    .OrderByDescending(t => t.MyTradeSide.Difference * t.TheirTradeSide.Difference)
                    .ToList();

                //store teams and trades in session
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
        public ActionResult ShowTeamSelectorList(TradesViewModel model)
        {
            Team selectedTeam = model.Teams
                .Where(t => t.TeamId == model.SelectedTeamId)
                .FirstOrDefault();

            List<Team> teams = new List<Team>(model.Teams);
            
            if (model.TeamSelectorMode == "TheirTeam")
            {
                if (model.MyTeam != null)
                {
                    int indexToRemove = 0;
                    for (int i = 0; i < teams.Count; i++)
                    {
                        if (teams[i].TeamId == model.MyTeam.TeamId)
                        {
                            indexToRemove = i;
                        }
                    }

                    teams.RemoveAt(indexToRemove);
                }

                teams.Insert(0, new Team() { TeamId = 0, Name = "All Teams" });
            }

            return PartialView("TeamList", teams);
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

                if (model.TheirTeam != null && model.MyTeam.TeamId == model.TheirTeam.TeamId)
                {
                    model.TheirTeam = null;
                }
            }

            model.TeamSelectorMode = null;
            model.SelectedTeamId = 0;

            ModelState.Clear();

            return PartialView("TradeFilter", model);
        }
    }
}