using fftoolkit.DB.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace fftoolkit.Web.ViewModels
{
    public class TradeViewModel
    {
        public List<TradeFilterViewModel> MyFilters { get; set; }
        public List<Player> MyExclusions { get; set; }

        public List<TradeFilterViewModel> TheirFilters { get; set; }
        public List<Player> TheirExclusions { get; set; }
    }
}