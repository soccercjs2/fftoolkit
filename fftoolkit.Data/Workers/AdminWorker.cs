using fftoolkit.DB.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fftoolkit.Data.Workers
{
    public class AdminWorker
    {
        private DataContext _context;

        public AdminWorker(DataContext context)
        {
            _context = context ?? throw new Exception("The context cannot be null.");
        }
    }
}
