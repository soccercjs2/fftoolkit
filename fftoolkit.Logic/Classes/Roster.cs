using fftoolkit.DB.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fftoolkit.Logic.Classes
{
    public class Roster
    {
        public List<Player> Quarterbacks { get; set; }
        public List<Player> RunningBacks { get; set; }
        public List<Player> WideReceivers { get; set; }
        public List<Player> TightEnds { get; set; }
        public List<Player> Flexes { get; set; }
    }
}
