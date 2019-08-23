using fftoolkit.DB.Models;
using System.Collections.Generic;

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
