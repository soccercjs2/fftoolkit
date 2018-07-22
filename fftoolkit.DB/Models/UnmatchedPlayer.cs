using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace fftoolkit.DB.Model
{
    public class UnmatchedPlayer
    {
        //ids

        [Required]
        public int UnmatchedPlayerId { get; set; }

        //player attributes

        [Required]
        public string Name { get; set; }

        [Required]
        public string Position { get; set; }

        [Required]
        public string Team { get; set; }

        public UnmatchedPlayer() { }

        public UnmatchedPlayer(Player player)
        {
            Name = player.Name;
            Position = player.Position;
            Team = player.Team;
        }

        public override int GetHashCode()
        {
            return String.Format("{0}{1}{2}", Name, Position, Team).GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj == null || this.GetType() != obj.GetType())
            {
                return false;
            }
            else
            {
                Player player = (Player)obj;
                return (player.Name == Name) && (player.Position == Position) && (player.Team == Team);
            }
        }
    }
}
