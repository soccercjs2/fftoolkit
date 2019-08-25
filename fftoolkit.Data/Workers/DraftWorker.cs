using fftoolkit.DB.Context;
using fftoolkit.DB.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace fftoolkit.Data.Workers
{
    public class DraftWorker
    {
        private DataContext _context;

        public DraftWorker(DataContext context)
        {
            _context = context ?? throw new Exception("The context cannot be null.");
        }

        public void Add(Draft draft)
        {
            _context.Drafts.Add(draft);
            _context.SaveChanges();
        }

        public void Update(Draft draft)
        {
            _context.Drafts.Attach(draft);
            _context.Entry(draft).State = System.Data.Entity.EntityState.Modified;
            _context.SaveChanges();
        }

        public void Delete(int draftId)
        {
            Draft draft = _context.Drafts.Find(draftId);
            _context.Drafts.Remove(draft);
            _context.SaveChanges();
        }

        public Draft Get(int draftId)
        {
            Draft draft = _context.Drafts.Find(draftId);
            return draft;
        }

        public List<Draft> Get(User user)
        {
            List<Draft> drafts = _context.Drafts.Where(d => d.OwnerUserId == user.UserId).ToList();
            return drafts;
        }
    }
}
