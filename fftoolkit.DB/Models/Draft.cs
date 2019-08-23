using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace fftoolkit.DB.Models
{
    public class Draft
    {
        //ids

        public int DraftId { get; set; }
        public int OwnerUserId { get; set; }

        [Required]
        public int LeagueId { get; set; }
        public virtual League League { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public int Teams { get; set; }

        [Required]
        public int Rounds { get; set; }

        public virtual List<DraftInvite> DraftInvites { get; set; }
        public virtual List<DraftParticipant> DraftParticipants { get; set; }
    }
}