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

        public string Name { get; set; }
        public string Url { get; set; }
        public string Host { get; set; }

        public int Quarterbacks { get; set; }
        [DisplayName("Running Backs")]
        public int RunningBacks { get; set; }
        [DisplayName("Wide Receivers")]
        public int WideReceivers { get; set; }
        [DisplayName("Tight Ends")]
        public int TightEnds { get; set; }
        [DisplayName("Flex Players")]
        public int Flexes { get; set; }

        [DisplayName("Points Per Reception")]
        public decimal PointsPerReception { get; set; }
        [DisplayName("Points Per Passing Yard")]
        public decimal PointsPerPassingYard { get; set; }
        [DisplayName("Points Per Passing Touchdown")]
        public decimal PointsPerPassingTouchdown { get; set; }
        [DisplayName("Points Lost Per Interception")]
        public decimal PointsLostPerInterception { get; set; }
    }
}
