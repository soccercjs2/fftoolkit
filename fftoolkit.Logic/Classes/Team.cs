﻿using fftoolkit.DB.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fftoolkit.Logic.Classes
{
    public class Team
    {
        public int TeamId { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public List<Player> Players { get; set; }
    }
}
