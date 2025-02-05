using AutoMapper;
using Project_Api.Dtos;
using Project_Api.Interfaces.AdminInterface;
using Project_Api.Models;
using Project_Api.Utilities;

namespace Project_Api.Services.AdminService
{
    public class AdminService : IAdminService
    {
        private readonly IAdminRepository _adminRepository;
        private readonly IMapper _mapper;
        private readonly JwtTokenHelper _jwtTokenHelper;

        public AdminService(IAdminRepository adminRepository, IMapper mapper, JwtTokenHelper jwtTokenHelper)
        {
            _adminRepository = adminRepository;
            _mapper = mapper;
            _jwtTokenHelper = jwtTokenHelper;
        }

        public async Task<IEnumerable<AdminDto>> GetAllAdminAsync()
        {
            var admins = await _adminRepository.GetAllAdminAsync();
            return _mapper.Map<IEnumerable<AdminDto>>(admins);
        }

        public async Task<AdminDto> GetAdminByIdAsync(int id)
        {
            var admin = await _adminRepository.GetAdminByIdAsync(id);
            return _mapper.Map<AdminDto>(admin);
        }

        public Task AddAdminAsync(AdminDto admin)
        {
            var newAdmin = _mapper.Map<Admin>(admin);
            newAdmin.Password = _jwtTokenHelper.HashPassword(newAdmin.Password);
            return _adminRepository.AddAdminAsync(newAdmin);
        }

        public Task UpdateAdminAsync(AdminDto admin)
        {
            var Updateadmin = _mapper.Map<Admin>(admin);
            Updateadmin.Password = _jwtTokenHelper.HashPassword(Updateadmin.Password);
            return _adminRepository.UpdateAdminAsync(Updateadmin);
        }

        public async Task DeleteAdminAsync(int id)
        {
            await _adminRepository.DeleteAdminAsync(id);
        }

     

    }
}
