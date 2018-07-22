using fftoolkit.DB.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace fftoolkit.Web.ViewModels
{
    public class MapUnmatchedPlayersViewModel
    {
        public UnmatchedPlayer UnmatchedPlayer { get; set; }

        public List<string> StandardTeams { get; set; }
        public List<string> Names { get; set; }

        public string SelectedTeam { get; set; }
        public string SelectedName { get; set; }
    }
}