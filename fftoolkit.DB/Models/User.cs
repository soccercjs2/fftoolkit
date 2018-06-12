using System;
using System.Collections.Generic;
using System.Text;

namespace fftoolkit.DB.Model
{
    public class User
    {
        public int UserId { get; set; }
        public string AspNetUserId { get; set; }

        public virtual List<League> Leagues { get; set; }
    }
}
