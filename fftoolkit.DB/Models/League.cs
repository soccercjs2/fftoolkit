using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace fftoolkit.DB.Model
{
    public class League
    {
        public int LeagueId { get; set; }
        public int UserId { get; set; }

        [Required]
        public string Name { get; set; }
        [Required]
        public string Url { get; set; }
        [Required]
        public string Host { get; set; }

        [Required]
        public int Quarterbacks { get; set; }
        [DisplayName("Running Backs")]
        [Required]
        public int RunningBacks { get; set; }
        [DisplayName("Wide Receivers")]
        [Required]
        public int WideReceivers { get; set; }
        [DisplayName("Tight Ends")]
        [Required]
        public int TightEnds { get; set; }
        [DisplayName("Flex Players")]
        [Required]
        public int Flexes { get; set; }

        [DisplayName("Points Per Reception")]
        [Required]
        public decimal PointsPerReception { get; set; }
        [DisplayName("Points Per Passing Yard")]
        [Required]
        public decimal PointsPerPassingYard { get; set; }
        [DisplayName("Points Per Passing Touchdown")]
        [Required]
        public decimal PointsPerPassingTouchdown { get; set; }
        [DisplayName("Points Lost Per Interception")]
        [Required]
        public decimal PointsLostPerInterception { get; set; }
    }
}
