using fftoolkit.DB.Context;
using fftoolkit.DB.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace fftoolkit.Data.Workers
{
    public class DraftInviteWorker
    {
        private DataContext _context;

        public DraftInviteWorker(DataContext context)
        {
            _context = context ?? throw new Exception("The context cannot be null.");
        }

        public void Add(DraftInvite draftInvite)
        {
            _context.DraftInvites.Add(draftInvite);
            _context.SaveChanges();
        }

        public void Update(DraftInvite draftInvite)
        {
            _context.DraftInvites.Attach(draftInvite);
            _context.Entry(draftInvite).State = System.Data.Entity.EntityState.Modified;
            _context.SaveChanges();
        }

        public void Delete(int draftId)
        {
            DraftInvite draftInvite = _context.DraftInvites.Find(draftId);
            _context.DraftInvites.Remove(draftInvite);
            _context.SaveChanges();
        }

        public DraftInvite Get(int draftId)
        {
            DraftInvite draftInvite = _context.DraftInvites.Find(draftId);
            return draftInvite;
        }

        public DraftInvite Get(string guid)
        {
            DraftInvite draftInvite = _context.DraftInvites.Where(di => di.Guid == guid).FirstOrDefault();
            return draftInvite;
        }
    }
}
