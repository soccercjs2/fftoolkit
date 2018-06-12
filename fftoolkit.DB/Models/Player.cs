using System;
using System.Collections.Generic;
using System.Text;

namespace fftoolkit.DB.Model
{
    public class Player
    {
        public int PlayerId { get; set; }
        public string Name { get; set; }
        public string Position { get; set; }
        public string Team { get; set; }

        public decimal PassingYards { get; set; }
        public decimal PassingTouchdowns { get; set; }
        public decimal Interceptions { get; set; }

        public decimal RushingYards { get; set; }
        public decimal RushingTouchdowns { get; set; }

        public decimal Receptions { get; set; }
        public decimal ReceivingYards { get; set; }
        public decimal ReceivingTouchdowns { get; set; }
    }
}
