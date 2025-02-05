using Project_Api.Dtos;

namespace Project_Api.Interfaces.AdminInterface
{
    public interface IAdminService
    {
        Task<IEnumerable<AdminDto>> GetAllAdminAsync();
        Task<AdminDto> GetAdminByIdAsync(int id);
        Task AddAdminAsync(AdminDto admin);
        Task UpdateAdminAsync(AdminDto admin);
        Task DeleteAdminAsync(int id);
    }
}
