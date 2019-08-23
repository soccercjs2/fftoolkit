using fftoolkit.DB.Context;
using fftoolkit.DB.Models;
using System;
using System.Collections.Generic;
using System.Linq;

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

        public void Add(List<Player> players)
        {
            _context.Players.AddRange(players);
            _context.SaveChanges();
        }

        public void AddUnmatchedPlayer(UnmatchedPlayer unmatchedPlayer)
        {
            _context.UnmatchedPlayers.Add(unmatchedPlayer);
            _context.SaveChanges();
        }

        public List<UnmatchedPlayer> GetUnmatchedPlayers()
        {
            return _context.UnmatchedPlayers.ToList();
        }

        public UnmatchedPlayer FindUnmatchedPlayer(UnmatchedPlayer unmatchedPlayer)
        {
            return _context.UnmatchedPlayers
                .Where(p => p.Name == unmatchedPlayer.Name && p.Position == unmatchedPlayer.Position && p.Team == unmatchedPlayer.Team)
                .FirstOrDefault();
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

        public void DeleteAll()
        {
            _context.Database.ExecuteSqlCommand("TRUNCATE TABLE Player");
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

        public void DeleteUnmatchedPlayer(int unmatchedPlayerId)
        {
            UnmatchedPlayer unmatchedPlayer = _context.UnmatchedPlayers.Find(unmatchedPlayerId);
            _context.UnmatchedPlayers.Remove(unmatchedPlayer);
            _context.SaveChanges();
        }
    }
}