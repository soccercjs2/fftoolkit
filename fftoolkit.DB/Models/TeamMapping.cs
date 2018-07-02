using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fftoolkit.DB.Models
{
    public class TeamMapping
    {
        public int TeamMappingId { get; set; }
        public string OldTeam { get; set; }
        public string NewTeam { get; set; }
    }
}
