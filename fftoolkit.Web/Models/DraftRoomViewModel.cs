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
        public List<DraftPick> DraftPicks { get; set; }

        public int SelectedPlayerId { get; set; }
        public string SelectedPlayerName { get; set; }
        public int CurrentRound { get; set; }
        public int CurrentPick { get; set; }
        public bool DraftPickAscending { get; set; }

        public string Message { get; set; }
        public bool IsErrorMessage { get; set; }
    }
}