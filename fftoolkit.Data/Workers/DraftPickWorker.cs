using fftoolkit.DB.Context;
using fftoolkit.DB.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace fftoolkit.Data.Workers
{
    public class DraftPickWorker
    {
        private DataContext _context;

        public DraftPickWorker(DataContext context)
        {
            _context = context ?? throw new Exception("The context cannot be null.");
        }

        public void Add(DraftPick draftPick)
        {
            _context.DraftPicks.Add(draftPick);
            _context.SaveChanges();
        }

        public void Update(DraftPick draftPick)
        {
            _context.DraftPicks.Attach(draftPick);
            _context.Entry(draftPick).State = System.Data.Entity.EntityState.Modified;
            _context.SaveChanges();
        }

        public void Delete(int draftPickId)
        {
            DraftPick draftPick = _context.DraftPicks.Find(draftPickId);
            _context.DraftPicks.Remove(draftPick);
            _context.SaveChanges();
        }

        public DraftPick Get(int draftPickId)
        {
            DraftPick draftPick = _context.DraftPicks.Find(draftPickId);
            return draftPick;
        }
    }
}
