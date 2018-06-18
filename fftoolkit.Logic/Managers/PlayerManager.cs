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
    public class PlayerManager
    {
        private DataContext _context;
        private PlayerWorker _playerWorker;

        public PlayerManager(DataContext context)
        {
            _context = context ?? throw new Exception("The context cannot be null.");
            _playerWorker = new PlayerWorker(_context);
        }

        public List<Player> Get()
        {
            return _playerWorker.Get();
        }

        public List<Player> Get(League league)
        {
            //get players and calculate fantasy points
            List<Player> players = _playerWorker.Get().ToList();
            players.ForEach(p => p.FantasyPoints = CalculateFantasyPoints(p, league));

            return players;
        }

        private decimal CalculateFantasyPoints(Player player, League league)
        {
            decimal passingPoints = (player.PassingYards * league.PointsPerPassingYard) +
                                    (player.PassingTouchdowns * league.PointsPerPassingTouchdown) -
                                    (player.Interceptions * league.PointsLostPerInterception);

            decimal rushingPoints = (player.RushingYards / 10) +
                                    (player.RushingTouchdowns * 6);

            decimal receivingPoints = (player.Receptions * league.PointsPerReception) +
                                        (player.ReceivingYards / 10) +
                                        (player.ReceivingTouchdowns * 6);

            return passingPoints + rushingPoints + receivingPoints;
        }
    }
}
