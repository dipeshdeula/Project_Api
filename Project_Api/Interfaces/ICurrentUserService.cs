namespace Project_Api.Interfaces
{
    public class ICurrentUserService
    {
        string UserId { get; }
        List<string> Roles { get; }
        string IpAddress { get; }
    }
}
