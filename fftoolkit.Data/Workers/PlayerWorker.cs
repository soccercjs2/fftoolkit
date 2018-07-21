﻿using fftoolkit.DB.Context;
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

        public void Add(List<Player> players)
        {
            _context.Players.AddRange(players);
            _context.SaveChanges();
        }

        public void AddUnmatchedPlayer(Player player)
        {
            _context.UnmatchedPlayers.Add(player);
            _context.SaveChanges();
        }

        public List<Player> GetUnmatchedPlayers()
        {
            return _context.UnmatchedPlayers.ToList();
        }

        public Player FindUnmatchedPlayer(Player player)
        {
            return _context.UnmatchedPlayers
                .Where(p => p.Name == player.Name && p.Position == player.Position && p.Team == player.Team)
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
    }
}