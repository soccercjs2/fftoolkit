using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace fftoolkit.DB.Model
{
    public class User
    {
        [Required]
        public int UserId { get; set; }

        [Required]
        public string AspNetUserId { get; set; }

        public virtual List<League> Leagues { get; set; }
    }
}
