using Project_Api.Models;

namespace Project_Api.Interfaces.AdminInterface
{
    public interface IAdminRepository
    {
        Task<IEnumerable<Admin>> GetAllAdminAsync();
        Task<Admin> GetAdminByIdAsync(int id);
        Task AddAdminAsync(Admin admin);
        Task UpdateAdminAsync(Admin admin);
        Task DeleteAdminAsync(int id);
    }
}
