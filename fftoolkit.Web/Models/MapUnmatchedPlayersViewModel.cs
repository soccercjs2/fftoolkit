using fftoolkit.DB.Models;
using System.Collections.Generic;

namespace fftoolkit.Web.Models
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