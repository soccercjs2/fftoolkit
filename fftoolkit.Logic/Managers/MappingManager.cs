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
    public class MappingManager
    {
        private DataContext _context;
        private MappingWorker _mappingWorker;

        public MappingManager(DataContext context)
        {
            _context = context ?? throw new Exception("The context cannot be null.");
            _mappingWorker = new MappingWorker(_context);
        }

        public void CreateTeamMapping(string oldTeam, string newTeam)
        {
            TeamMapping existingTeamMapping = _mappingWorker.GetTeamMapping(oldTeam);

            if (existingTeamMapping == null)
            {
                _mappingWorker.CreateTeamMapping(oldTeam, newTeam);
            }
        }

        public void CreateNameMapping(string oldName, string newName)
        {
            TeamMapping existingNameMapping = _mappingWorker.GetNameMapping(oldName);

            if (existingNameMapping == null)
            {
                _mappingWorker.CreateNameMapping(oldName, newName);
            }
        }

        public string GetTeam(string oldTeam)
        {
            TeamMapping teamMapping = _mappingWorker.GetTeamMapping(oldTeam);
            return (teamMapping == null) ? null : teamMapping.NewTeam;
        }

        public string GetName(string oldName)
        {
            NameMapping nameMapping = _mappingWorker.GetNameMapping(oldName);
            return (nameMapping == null) ? null : nameMapping.NewName;
        }

        public Dictionary<string, string> GetTeamMappings()
        {
            return _mappingWorker.GetTeamMappings().ToDictionary(tm => tm.OldTeam, tm => tm.NewTeam);
        }

        public Dictionary<string, string> GetNameMappings()
        {
            return _mappingWorker.GetNameMappings().ToDictionary(n => n.OldName, n => n.NewName);
        }
    }
}
