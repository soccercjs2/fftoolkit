using fftoolkit.DB.Context;
using fftoolkit.DB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fftoolkit.Data.Workers
{
    public class MappingWorker
    {
        private DataContext _context;

        public MappingWorker(DataContext context)
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

        public List<NameMapping> GetNameMappings()
        {
            return _context.NameMappings.ToList();
        }

        public NameMapping GetNameMapping(string oldName)
        {
            return _context.NameMappings.Where(n => n.OldName == oldName).FirstOrDefault();
        }

        public void CreateNameMapping(string oldName, string newName)
        {
            NameMapping nameMapping = new NameMapping()
            {
                OldName = oldName,
                NewName = newName
            };

            _context.NameMappings.Add(nameMapping);
            _context.SaveChanges();
        }
    }
}
