using fftoolkit.DB.Context;
using fftoolkit.DB.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fftoolkit.Data.Workers
{
    public class LeagueWorker
    {
        private DataContext _context;

        public LeagueWorker(DataContext context)
        {
            _context = context ?? throw new Exception("The context cannot be null.");
        }

        public void Add(League league)
        {
            _context.Leagues.Add(league);
            _context.SaveChanges();
        }

        public void Update(League league)
        {
            _context.Leagues.Attach(league);
            _context.Entry(league).State = System.Data.Entity.EntityState.Modified;
            _context.SaveChanges();
        }

        public void Delete(int leagueId)
        {
            League league = _context.Leagues.Find(leagueId);
            _context.Leagues.Remove(league);
            _context.SaveChanges();
        }

        public League Get(int leagueId)
        {
            League league = _context.Leagues.Find(leagueId);
            return league;
        }
    }
}
