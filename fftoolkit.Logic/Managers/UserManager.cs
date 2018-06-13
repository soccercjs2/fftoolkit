using fftoolkit.Data.Workers;
using fftoolkit.DB.Context;
using fftoolkit.DB.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fftoolkit.Logic.Managers
{
    public class UserManager
    {
        private DataContext _context;
        private UserWorker _userWorker;

        public UserManager(DataContext context)
        {
            _context = context ?? throw new Exception("The context cannot be null.");
            _userWorker = new UserWorker(_context);
        }

        public User GetCurrentUser(string userId)
        {
            User user = _context.Users.Where(u => u.AspNetUserId == userId).FirstOrDefault();

            if (user == null)
            {
                throw new Exception("No user found.");
            }

            return user;
        }
    }
}
