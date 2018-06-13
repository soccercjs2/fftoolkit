using System;
using System.Collections.Generic;
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
        public int RunningBacks { get; set; }
        public int WideReceivers { get; set; }
        public int TightEnds { get; set; }
        public int Flexes { get; set; }

        public decimal PointsPerReception { get; set; }
        public decimal PointsPerPassingTouchdown { get; set; }
        public decimal PointsPerPassingYard { get; set; }
        public decimal PointsLostPerPassingYard { get; set; }
    }
}
