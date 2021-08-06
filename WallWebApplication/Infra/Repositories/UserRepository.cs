using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WallWebApplication.Domain.Models;

namespace WallWebApplication.Infra.Repositories
{
    public class UserRepository
    {
        private readonly MySQLContext _context;

        public UserRepository(MySQLContext context)
        {
            this._context = context;
        }

        public async Task<List<ApplicationUser>> GetAllUsersAsync()
        {

            return await _context.User.ToListAsync();
        }

        public async Task<ApplicationUser> GetUserByIdAsync(string userId)
        {
            var user = await _context.User.FindAsync(userId);
            return user;
        }

        public async Task<ApplicationUser> UpdateUserAsync(ApplicationUser user)
        {
            _context.Entry(user).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return _context.Entry(user).Entity;
        }


        public async Task<ApplicationUser> CreateNewUserAsync(ApplicationUser user)
        {
            _context.User.Add(user);
            await _context.SaveChangesAsync();

            return user;
        }

        public async Task<ApplicationUser> DeleteUserAsync(string userId)
        {
            var user = await _context.User.FindAsync(userId);
            _context.User.Remove(user);
            await _context.SaveChangesAsync();

            return user;
        }
    }
}
