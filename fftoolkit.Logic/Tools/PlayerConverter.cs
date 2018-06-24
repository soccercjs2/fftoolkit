using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fftoolkit.Logic.Tools
{
    public class PlayerConverter
    {
        public string Name { get; set; }
        public string Team { get; set; }

        public PlayerConverter(string name, string nflTeam)
        {
            Name = GetTeam(name);
            Team = GetNflTeam(nflTeam);
        }

        private string GetTeam(string name)
        {
            switch (name)
            {
                default: return name;
            }
        }

        private string GetNflTeam(string nflTeam)
        {
            if (nflTeam != null) { nflTeam = nflTeam.ToUpper(); }

            switch (nflTeam)
            {
                default: return nflTeam;
            }
        }
    }
}
