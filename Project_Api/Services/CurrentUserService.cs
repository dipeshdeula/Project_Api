using System.Security.Claims;

namespace Project_Api.Services
{
   /* public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IIpHelperService _iHelperService;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor, IIpHelperService ipHelperService)
        
        {
            this._httpContextAccessor = httpContextAccessor;
            this._iHelperService = ipHelperService;
            this.DeviceToken = _httpContextAccessor.HttpContext?.Request.Headers["DeviceToken"].ToString();
            this.UserAgent = _httpContextAccessor.HttpContext?.Request.Headers["User-Agent"].ToString();

        }

        public string UserId => _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);

        public List<string> Roles => _httpContextAccessor.HttpContext?.User.Claims.Where(x => x.Type == ClaimTypes.Role).Select(x => x.Value).ToList();

        public string IpAddress => _ipHelperService.GetPublicIPAddress();

        public string DeviceToken { get; }
        public string UserAgent { get; }
    }*/
}
