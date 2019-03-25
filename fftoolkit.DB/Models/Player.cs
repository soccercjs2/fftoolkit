using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace fftoolkit.DB.Model
{
    public class Player : IEquatable<Player>
    {
        //ids

        [Required]
        public int PlayerId { get; set; }

        //player attributes

        [Required]
        public string Name { get; set; }

        [Required]
        public string Position { get; set; }

        [Required]
        public string Team { get; set; }

        //passing stats

        [Required]
        [DisplayFormat(DataFormatString = "{0:0.##}")]
        public decimal PassingYards { get; set; }

        [Required]
        [DisplayFormat(DataFormatString = "{0:0.##}")]
        public decimal PassingTouchdowns { get; set; }

        [Required]
        [DisplayFormat(DataFormatString = "{0:0.##}")]
        public decimal Interceptions { get; set; }

        //rushing stats

        [Required]
        [DisplayFormat(DataFormatString = "{0:0.##}")]
        public decimal RushingYards { get; set; }

        [Required]
        [DisplayFormat(DataFormatString = "{0:0.##}")]
        public decimal RushingTouchdowns { get; set; }

        //receiving stats

        [Required]
        [DisplayFormat(DataFormatString = "{0:0.##}")]
        public decimal Receptions { get; set; }

        [Required]
        [DisplayFormat(DataFormatString = "{0:0.##}")]
        public decimal ReceivingYards { get; set; }

        [Required]
        [DisplayFormat(DataFormatString = "{0:0.##}")]
        public decimal ReceivingTouchdowns { get; set; }

        //fantasy values

        [NotMapped]
        [DisplayFormat(DataFormatString = "{0:0.##}")]
        public decimal FantasyPoints { get; set; }

        [NotMapped]
        [DisplayFormat(DataFormatString = "{0:0.##}")]
        public decimal TradeValue { get; set; }

        [NotMapped]
        public int GamesPlayed { get; set; }

        public Player() { }
        public Player(Player player)
        {
            Name = player.Name;
            Position = player.Position;
            Team = player.Team;
            PassingYards = player.PassingYards;
            PassingTouchdowns = player.PassingTouchdowns;
            RushingYards = player.RushingYards;
            RushingTouchdowns = player.RushingTouchdowns;
            Receptions = player.Receptions;
            ReceivingYards = player.ReceivingYards;
            ReceivingTouchdowns = player.ReceivingTouchdowns;
            FantasyPoints = player.FantasyPoints;
            GamesPlayed = player.GamesPlayed;
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

        public bool Equals(Player player)
        {
            if (player == null) return false;
            return (player.Name == Name) && (player.Position == Position) && (player.Team == Team);
        }
    }
}
