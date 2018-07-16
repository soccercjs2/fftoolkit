using fftoolkit.DB.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace fftoolkit.Web.ViewModels
{
    public class TradeFilterViewModel
    {
        public TradeFilterType Type { get; set; }
        public Player Player { get; set; }
        public string Position { get; set; }
    }

    public enum TradeFilterType
    {
        Player,
        Position
    }
}