using fftoolkit.DB.Models;
using System;
using System.Collections.Generic;

namespace fftoolkit.Logic.Classes
{
    public class Team : IEquatable<Team>
    {
        public int TeamId { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public List<Player> Players { get; set; }

        public bool Equals(Team team)
        {
            if (team == null) return false;
            return team.TeamId == TeamId;
        }
    }
}
