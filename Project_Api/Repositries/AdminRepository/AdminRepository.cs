using Microsoft.EntityFrameworkCore;
using Project_Api.Data;
using Project_Api.Interfaces.AdminInterface;
using Project_Api.Models;

namespace Project_Api.Repositries.AdminRepository
{
    public class AdminRepository : IAdminRepository
    {
        private readonly ApplicationDbContext _context;

        public AdminRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Admin>> GetAllAdminAsync()
        {
           return await _context.Admins.ToListAsync();
        }

        public async Task<Admin> GetAdminByIdAsync(int id)
        {
            return await _context.Admins.FindAsync(id);
        }

        public async Task AddAdminAsync(Admin admin)
        {
           await _context.Admins.AddAsync(admin);
            await _context.SaveChangesAsync();
        }


        public async Task UpdateAdminAsync(Admin admin)
        {
            _context.Admins.Update(admin);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAdminAsync(int id)
        {
            var admin = await _context.Admins.FindAsync(id);
            if (admin != null)
            { 
                _context.Admins.Remove(admin);
                await _context.SaveChangesAsync();
            }
        }

       

      
    }
}
