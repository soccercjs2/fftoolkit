using fftoolkit.DB.Context;
using fftoolkit.DB.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace fftoolkit.Data.Workers
{
    public class DraftParticipantWorker
    {
        private DataContext _context;

        public DraftParticipantWorker(DataContext context)
        {
            _context = context ?? throw new Exception("The context cannot be null.");
        }

        public void Add(DraftParticipant draftParticipant)
        {
            _context.DraftParticipants.Add(draftParticipant);
            _context.SaveChanges();
        }

        public void Update(DraftParticipant draftParticipant)
        {
            _context.DraftParticipants.Attach(draftParticipant);
            _context.Entry(draftParticipant).State = System.Data.Entity.EntityState.Modified;
            _context.SaveChanges();
        }

        public void Delete(int draftId)
        {
            DraftParticipant draftParticipant = _context.DraftParticipants.Find(draftId);
            _context.DraftParticipants.Remove(draftParticipant);
            _context.SaveChanges();
        }

        public DraftParticipant Get(int draftId)
        {
            DraftParticipant draftParticipant = _context.DraftParticipants.Find(draftId);
            return draftParticipant;
        }
    }
}
