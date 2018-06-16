using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace fftoolkit.DB.Model
{
    public class Player
    {
        [Required]
        public int PlayerId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Position { get; set; }
        [Required]
        public string Team { get; set; }

        [Required]
        public decimal PassingYards { get; set; }
        [Required]
        public decimal PassingTouchdowns { get; set; }
        [Required]
        public decimal Interceptions { get; set; }

        [Required]
        public decimal RushingYards { get; set; }
        [Required]
        public decimal RushingTouchdowns { get; set; }

        [Required]
        public decimal Receptions { get; set; }
        [Required]
        public decimal ReceivingYards { get; set; }
        [Required]
        public decimal ReceivingTouchdowns { get; set; }
    }
}
