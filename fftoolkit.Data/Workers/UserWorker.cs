using fftoolkit.DB.Context;
using fftoolkit.DB.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fftoolkit.Data.Workers
{
    public class UserWorker
    {
        private DataContext _context;

        public UserWorker(DataContext context)
        {
            _context = context ?? throw new Exception("The context cannot be null.");
        }

        public void Add(User user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();
        }

        public void Update(User user)
        {
            _context.Users.Attach(user);
            _context.Entry(user).State = System.Data.Entity.EntityState.Modified;
            _context.SaveChanges();
        }

        public void Delete(int userId)
        {
            User user = _context.Users.Find(userId);
            _context.Users.Remove(user);
            _context.SaveChanges();
        }

        public User Get(int userId)
        {
            User user = _context.Users.Find(userId);
            return user;
        }
    }
}
