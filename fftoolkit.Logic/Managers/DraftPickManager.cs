﻿using fftoolkit.Data.Workers;
using fftoolkit.DB.Context;
using fftoolkit.DB.Models;
using System;
using System.Collections.Generic;

namespace fftoolkit.Logic.Managers
{
    public class DraftPickManager
    {
        private DataContext _context;
        private DraftPickWorker _draftPickWorker;

        public DraftPickManager(DataContext context)
        {
            _context = context ?? throw new Exception("The context cannot be null.");
            _draftPickWorker = new DraftPickWorker(_context);
        }

        public void Add(DraftPick draftPick)
        {
            _draftPickWorker.Add(draftPick);
        }

        public DraftPick Add(int draftId, int playerId, int round, int pick)
        {
            DraftPick draftPick = new DraftPick()
            {
                DraftId = draftId,
                PlayerId = playerId,
                Round = round,
                Pick = pick
            };

            _draftPickWorker.Add(draftPick);

            return draftPick;
        }

        public DraftPick Get(int draftPickId)
        {
            return _draftPickWorker.Get(draftPickId);
        }

        public void Update(DraftPick draftPick)
        {
            _draftPickWorker.Update(draftPick);
        }

        public void Delete(int draftPickId)
        {
            _draftPickWorker.Delete(draftPickId);
        }
    }
}
