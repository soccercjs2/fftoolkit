using fftoolkit.Data.Workers;
using fftoolkit.DB.Context;
using fftoolkit.DB.Models;
using System;

namespace fftoolkit.Logic.Managers
{
    public class LeagueManager
    {
        private DataContext _context;
        private LeagueWorker _leagueWorker;

        public LeagueManager(DataContext context)
        {
            _context = context ?? throw new Exception("The context cannot be null.");
            _leagueWorker = new LeagueWorker(_context);
        }

        public void Add(League league)
        {
            _leagueWorker.Add(league);
        }

        public League Get(int leagueId)
        {
            return _leagueWorker.Get(leagueId);
        }

        public void Update(League league)
        {
            _leagueWorker.Update(league);
        }

        public void Delete(int leagueId)
        {
            _leagueWorker.Delete(leagueId);
        }
    }
}
