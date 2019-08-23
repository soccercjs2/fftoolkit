using fftoolkit.DB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace fftoolkit.Web.Models
{
    public class DraftRoomViewModel
    {
        public Draft Draft { get; set; }
        public List<Player> Players { get; set; }
        public List<string> Positions { get; set; }
    }
}