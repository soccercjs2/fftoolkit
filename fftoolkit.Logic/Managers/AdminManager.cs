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
    public class AdminManager
    {
        private DataContext _context;
        private AdminWorker _adminWorker;

        public AdminManager(DataContext context)
        {
            _context = context ?? throw new Exception("The context cannot be null.");
            _adminWorker = new AdminWorker(_context);
        }

        public void CreateTeamMapping(string oldTeam, string newTeam)
        {
            TeamMapping existingTeamMapping = _adminWorker.GetTeamMapping(oldTeam);

            if (existingTeamMapping == null)
            {
                _adminWorker.CreateTeamMapping(oldTeam, newTeam);
            }
        }

        public string GetTeam(string oldTeam)
        {
            TeamMapping teamMapping = _adminWorker.GetTeamMapping(oldTeam);
            return (teamMapping == null) ? null : teamMapping.NewTeam;
        }
    }
}
