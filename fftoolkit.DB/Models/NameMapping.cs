using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fftoolkit.DB.Models
{
    public class NameMapping
    {
        public int NameMappingId { get; set; }
        public string OldName { get; set; }
        public string NewName { get; set; }
    }
}
