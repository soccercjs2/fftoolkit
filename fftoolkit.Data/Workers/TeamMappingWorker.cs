using fftoolkit.DB.Context;
using fftoolkit.DB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fftoolkit.Data.Workers
{
    public class TeamMappingWorker
    {
        private DataContext _context;

        public TeamMappingWorker(DataContext context)
        {
            _context = context ?? throw new Exception("The context cannot be null.");
        }

        public List<TeamMapping> GetTeamMappings()
        {
            return _context.TeamMappings.ToList();
        }

        public TeamMapping GetTeamMapping(string oldTeam)
        {
            return _context.TeamMappings.Where(tm => tm.OldTeam == oldTeam).FirstOrDefault();
        }

        public void CreateTeamMapping(string oldTeam, string newTeam)
        {
            TeamMapping teamMapping = new TeamMapping()
            {
                OldTeam = oldTeam,
                NewTeam = newTeam
            };

            _context.TeamMappings.Add(teamMapping);
            _context.SaveChanges();
        }
    }
}
