using fftoolkit.Data.Workers;
using fftoolkit.DB.Context;
using fftoolkit.DB.Models;
using System;
using System.Collections.Generic;

namespace fftoolkit.Logic.Managers
{
    public class DraftParticipantManager
    {
        private DataContext _context;
        private DraftParticipantWorker _draftParticipantWorker;

        public DraftParticipantManager(DataContext context)
        {
            _context = context ?? throw new Exception("The context cannot be null.");
            _draftParticipantWorker = new DraftParticipantWorker(_context);
        }

        public void Add(DraftParticipant draftParticipant)
        {
            _draftParticipantWorker.Add(draftParticipant);
        }

        public DraftParticipant Get(int draftParticipantId)
        {
            return _draftParticipantWorker.Get(draftParticipantId);
        }

        public void Update(DraftParticipant draftParticipant)
        {
            _draftParticipantWorker.Update(draftParticipant);
        }

        public void Delete(int draftParticipantId)
        {
            _draftParticipantWorker.Delete(draftParticipantId);
        }
    }
}
