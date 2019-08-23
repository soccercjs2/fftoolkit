using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace fftoolkit.DB.Models
{
    public class User
    {
        [Required]
        public int UserId { get; set; }

        [Required]
        public string AspNetUserId { get; set; }

        public virtual List<League> Leagues { get; set; }
        public virtual List<DraftInvite> DraftInvites { get; set; }
        public virtual List<DraftParticipant> DraftParticipants { get; set; }
    }
}
