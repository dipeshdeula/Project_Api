using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Project_Api.Dtos;
using Project_Api.Interfaces;
using Project_Api.Interfaces.AdminInterface;
using Project_Api.Models;
using Project_Api.Utilities;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Security.Claims;
using System.Text;

namespace Project_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IAdminRepository _adminRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly JwtTokenHelper _jwtTokenHelper;

        public AuthController(IConfiguration configuration, IAdminRepository adminRepository, ICustomerRepository cutomerRepository, JwtTokenHelper jwtTokenHelper)
        {
            _configuration = configuration;
            _customerRepository = cutomerRepository;
            _adminRepository = adminRepository;
            _jwtTokenHelper = jwtTokenHelper;
        }

       

        [AllowAnonymous]
        [HttpPost("login")]

        public IActionResult Login(LoginDto user)
        {
            var authenticatedUser =  AuthenticateUser(user, out var userId);
            if (authenticatedUser != null)
            { 
                var token = _jwtTokenHelper.GenerateToken(authenticatedUser);
                return Ok(new { token = token, messge = "Logged in successfully", userId = userId , role = authenticatedUser.Role});
            }
            else
            {
                return Unauthorized(new { message = "Invalid email or password" });
            }
        }

        private LoginDto AuthenticateUser(LoginDto user, out int userId)
        {
            userId = 0;
           

            var hashedPassword = _jwtTokenHelper.HashPassword(user.Password);
            var customer = _customerRepository.GetllCustomersAsync().Result.FirstOrDefault(x => x.Email == user.Email && x.Password == hashedPassword);

            if (customer != null)
            {
                userId = customer.Id;               
                var authenticatedUser = new LoginDto
                {
                    Email = customer.Email,
                    Role = "User"

                };
            }

            var admin = _adminRepository.GetAllAdminAsync().Result.FirstOrDefault(x => x.Email == user.Email && x.Password == hashedPassword);

            if (admin != null)
            { 
                userId = admin.Id;
               
                return new LoginDto { Email = admin.Email, Role="Admin" };
            }
            return null;
        }

        /*private string HashPassword(string password)
        { 
            using var sha256 = System.Security.Cryptography.SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(password);
            var hash = sha256.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }*/
    }
}
