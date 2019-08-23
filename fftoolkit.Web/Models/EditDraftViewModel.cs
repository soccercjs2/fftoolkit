using fftoolkit.DB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace fftoolkit.Web.Models
{
    public class EditDraftViewModel
    {
        public Draft Draft { get; set; }
        public DraftInvite NewDraftInvite { get; set; }
        public DraftParticipant NewDraftParticipant { get; set;  }
        public List<League> Leagues { get; set; }
    }
}