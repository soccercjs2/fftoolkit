using fftoolkit.Data.Workers;
using fftoolkit.DB.Context;
using fftoolkit.DB.Models;
using System;
using System.Collections.Generic;

namespace fftoolkit.Logic.Managers
{
    public class DraftInviteManager
    {
        private DataContext _context;
        private DraftInviteWorker _draftInviteWorker;

        public DraftInviteManager(DataContext context)
        {
            _context = context ?? throw new Exception("The context cannot be null.");
            _draftInviteWorker = new DraftInviteWorker(_context);
        }

        public void Add(DraftInvite draftInvite)
        {
            _draftInviteWorker.Add(draftInvite);
        }

        public DraftInvite Get(int draftInviteId)
        {
            return _draftInviteWorker.Get(draftInviteId);
        }

        public DraftInvite Get(string guid)
        {
            return _draftInviteWorker.Get(guid);
        }

        public void Update(DraftInvite draftInvite)
        {
            _draftInviteWorker.Update(draftInvite);
        }

        public void Delete(int draftInviteId)
        {
            _draftInviteWorker.Delete(draftInviteId);
        }
    }
}
