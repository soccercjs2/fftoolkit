using fftoolkit.DB.Context;
using fftoolkit.DB.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fftoolkit.Data.Workers
{
    public class PlayerWorker
    {
        private DataContext _context;

        public PlayerWorker(DataContext context)
        {
            _context = context ?? throw new Exception("The context cannot be null.");
        }

        public void Add(Player player)
        {
            _context.Players.Add(player);
            _context.SaveChanges();
        }

        public void Update(Player player)
        {
            _context.Players.Attach(player);
            _context.Entry(player).State = System.Data.Entity.EntityState.Modified;
            _context.SaveChanges();
        }

        public void Delete(int playerId)
        {
            Player player = _context.Players.Find(playerId);
            _context.Players.Remove(player);
            _context.SaveChanges();
        }

        public List<Player> Get()
        {
            return _context.Players.ToList();
        }

        public Player Get(int playerId)
        {
            Player player = _context.Players.Find(playerId);
            return player;
        }
    }
}