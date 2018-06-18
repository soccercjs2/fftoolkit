using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace fftoolkit.DB.Model
{
    public class League
    {
        //ids

        public int LeagueId { get; set; }
        public int UserId { get; set; }

        //league attributes

        [Required]
        public string Name { get; set; }

        [Required]
        public string Url { get; set; }

        [Required]
        public string Host { get; set; }

        //roster settings

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

        //scoring settings

        [DisplayName("Points Per Reception")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:0.#######}")]
        [RegularExpression(@"((\d+)((\.\d{1,2})?))$", ErrorMessage = "Must be a valid Decimal number with maximum 2 decimal places.")]
        [Required]
        public decimal PointsPerReception { get; set; }

        [DisplayName("Points Per Passing Yard")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:0.#######}")]
        [RegularExpression(@"((\d+)((\.\d{1,2})?))$", ErrorMessage = "Must be a valid Decimal number with maximum 2 decimal places.")]
        [Required]
        public decimal PointsPerPassingYard { get; set; }

        [DisplayName("Points Per Passing Touchdown")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:0.#######}")]
        [RegularExpression(@"((\d+)((\.\d{1,2})?))$", ErrorMessage = "Must be a valid Decimal number with maximum 2 decimal places.")]
        [Required]
        public decimal PointsPerPassingTouchdown { get; set; }

        [DisplayName("Points Lost Per Interception")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:0.#######}")]
        [RegularExpression(@"((\d+)((\.\d{1,2})?))$", ErrorMessage = "Must be a valid Decimal number with maximum 2 decimal places.")]
        [Required]
        public decimal PointsLostPerInterception { get; set; }
    }
}
