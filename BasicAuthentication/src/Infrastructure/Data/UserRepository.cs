using Core.Entities;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    public class UserRepository : IUserRepository
    {
        private readonly UserContext _context;
        public UserRepository(UserContext context)
        {
            _context = context;
        }

        public async Task<User> AddAsync(User entity)
        {
            var ent = await _context.Set<User>().AddAsync(entity);
            await _context.SaveChangesAsync();
            return ent.Entity;
        }

        public async Task<int> CountAsync()
        {
            return await _context.Set<User>().CountAsync();
        }

        public async Task<int> CountAsync(Expression<Func<User, bool>> predicate)
        {
            return await _context.Set<User>().CountAsync(predicate);
        }

        public async Task<User> DeleteAsync(int id)
        {
            User found = await _context.Set<User>().FirstOrDefaultAsync((u) => u.Id == id);

            if (found == default)
                return found;

            _context.Set<User>().Remove(found);
            await _context.SaveChangesAsync();
            return found;
        }

        public async Task<User> FirstOrDefaultAsync(Expression<Func<User, bool>> predicate)
        {
            return await _context.Set<User>()
                .FirstOrDefaultAsync(predicate);
        }

        public async Task<IEnumerable<User>> GetAllAsync(int page, int limit)
        {
            return await _context.Set<User>()
                .OrderBy((u) => u.Id)
                .Skip(page)
                .Take(limit)
                .ToListAsync();
        }

        public async Task<IEnumerable<User>> GetAllAsync(Expression<Func<User, bool>> predicate, int page, int limit)
        {
            return await _context.Set<User>()
                .Where(predicate)
                .OrderBy((u) => u.Id)
                .Skip(page)
                .Take(limit)
                .ToListAsync();
        }

        public async Task<User> UpdateAsync(User entity)
        {
            User u = _context.Set<User>().Find(entity.Id);

            u.FirstName = entity.FirstName;
            u.LastName = entity.LastName;
            u.GenderAbbreviation = entity.GenderAbbreviation;
            u.Phone = entity.Phone;
            u.DateOfBirth = entity.DateOfBirth;
            u.Username = entity.Username;
            u.Email = entity.Email;
            u.UpdatedAt = entity.UpdatedAt;

            _context.Set<User>().Update(u);
            await _context.SaveChangesAsync();
            return u;
        }
    }
}
