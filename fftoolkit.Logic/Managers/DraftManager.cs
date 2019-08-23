using fftoolkit.Data.Workers;
using fftoolkit.DB.Context;
using fftoolkit.DB.Models;
using System;

namespace fftoolkit.Logic.Managers
{
    public class DraftManager
    {
        private DataContext _context;
        private DraftWorker _draftrWorker;

        public DraftManager(DataContext context)
        {
            _context = context ?? throw new Exception("The context cannot be null.");
            _draftrWorker = new DraftWorker(_context);
        }

        public void Add(Draft draft)
        {
            _draftrWorker.Add(draft);
        }

        public Draft Get(int draftId)
        {
            return _draftrWorker.Get(draftId);
        }

        public void Update(Draft draft)
        {
            _draftrWorker.Update(draft);
        }

        public void Delete(int draftId)
        {
            _draftrWorker.Delete(draftId);
        }
    }
}
