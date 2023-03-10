namespace CarRental.Application.Interfaces.Identity
{
    public interface IIdentityService
    {
        Guid GetUserIdentity();
        string GetUserEmail();
    }
}
