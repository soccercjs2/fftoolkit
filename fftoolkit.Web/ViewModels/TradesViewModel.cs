using fftoolkit.DB.Model;
using fftoolkit.Logic.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace fftoolkit.Web.ViewModels
{
    public class TradesViewModel
    {
        public League League { get; set; }
        public string TeamSelectorMode { get; set; }
        public int SelectedTeamId { get; set; }
        public Team SelectedTeam { get; set; }

        public Team MyTeam { get; set; }
        public List<TradeFilterViewModel> MyFilters { get; set; }
        public List<Player> MyExclusions { get; set; }

        public Team TheirTeam { get; set; }
        public List<TradeFilterViewModel> TheirFilters { get; set; }
        public List<Player> TheirExclusions { get; set; }

        public List<Team> Teams { get; set; }

        public List<Trade> Trades { get; set; }
        public int TradePage { get; set; }

        public TradesViewModel() { }

        public TradesViewModel(List<Team> teams)
        {
            Teams = teams ?? throw new Exception("No teams provided.");

            MyFilters = new List<TradeFilterViewModel>(new TradeFilterViewModel[4]);
            MyExclusions = new List<Player>();

            TheirFilters = new List<TradeFilterViewModel>(new TradeFilterViewModel[4]);
            TheirExclusions = new List<Player>();
            Trades = new List<Trade>();
            TradePage = 1;
        }
    }
}