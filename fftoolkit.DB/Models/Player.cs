using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace fftoolkit.DB.Model
{
    public class Player
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
        [DisplayFormat(DataFormatString = "{0:0.#######}")]
        public decimal PassingYards { get; set; }

        [Required]
        [DisplayFormat(DataFormatString = "{0:0.#######}")]
        public decimal PassingTouchdowns { get; set; }

        [Required]
        [DisplayFormat(DataFormatString = "{0:0.#######}")]
        public decimal Interceptions { get; set; }

        //rushing stats

        [Required]
        [DisplayFormat(DataFormatString = "{0:0.#######}")]
        public decimal RushingYards { get; set; }

        [Required]
        [DisplayFormat(DataFormatString = "{0:0.#######}")]
        public decimal RushingTouchdowns { get; set; }

        //receiving stats

        [Required]
        [DisplayFormat(DataFormatString = "{0:0.#######}")]
        public decimal Receptions { get; set; }

        [Required]
        [DisplayFormat(DataFormatString = "{0:0.#######}")]
        public decimal ReceivingYards { get; set; }

        [Required]
        [DisplayFormat(DataFormatString = "{0:0.#######}")]
        public decimal ReceivingTouchdowns { get; set; }

        //fantasy values

        [NotMapped]
        [DisplayFormat(DataFormatString = "{0:0.#######}")]
        public decimal FantasyPoints { get; set; }
    }
}
