using fftoolkit.Data.Workers;
using fftoolkit.DB.Context;
using fftoolkit.DB.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fftoolkit.Logic.Managers
{
    public class LeagueManager
    {
        private DataContext _context;
        private LeagueWorker _leaguerWorker;

        public LeagueManager(DataContext context)
        {
            _context = context ?? throw new Exception("The context cannot be null.");
            _leaguerWorker = new LeagueWorker(_context);
        }

        public void Add(League league)
        {
            _leaguerWorker.Add(league);
        }

        public League Get(int leagueId)
        {
            return _leaguerWorker.Get(leagueId);
        }

        public void Update(League league)
        {
            _leaguerWorker.Update(league);
        }

        public void Delete(int leagueId)
        {
            _leaguerWorker.Delete(leagueId);
        }
    }
}
