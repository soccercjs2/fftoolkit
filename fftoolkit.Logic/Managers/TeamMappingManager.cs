using fftoolkit.Data.Workers;
using fftoolkit.DB.Context;
using fftoolkit.DB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fftoolkit.Logic.Managers
{
    public class TeamMappingManager
    {
        private DataContext _context;
        private TeamMappingWorker _teamMappingWorker;

        public TeamMappingManager(DataContext context)
        {
            _context = context ?? throw new Exception("The context cannot be null.");
            _teamMappingWorker = new TeamMappingWorker(_context);
        }

        public void CreateTeamMapping(string oldTeam, string newTeam)
        {
            TeamMapping existingTeamMapping = _teamMappingWorker.GetTeamMapping(oldTeam);

            if (existingTeamMapping == null)
            {
                _teamMappingWorker.CreateTeamMapping(oldTeam, newTeam);
            }
        }

        public string GetTeam(string oldTeam)
        {
            TeamMapping teamMapping = _teamMappingWorker.GetTeamMapping(oldTeam);
            return (teamMapping == null) ? null : teamMapping.NewTeam;
        }

        public Dictionary<string, string> GetTeamMappings()
        {
            return _teamMappingWorker.GetTeamMappings().ToDictionary(tm => tm.OldTeam, tm => tm.NewTeam);
        }
    }
}
