using CarRental.Application.Interfaces.Identity;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace CarRental.Persistence.Services.Identity
{
    public class IdentityService : IIdentityService
    {
        public readonly IHttpContextAccessor _context;

        public IdentityService(IHttpContextAccessor context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context), $"{context} cannot be null.");
        }
        public string GetUserEmail()
        {
            var userEmail = _context?.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)!.Value;

            return userEmail ?? throw new NullReferenceException(nameof(userEmail));
        }

        public Guid GetUserIdentity()
        {
            var userId = _context?.HttpContext?.User.FindFirst("emails")!.Value;

            return new Guid(userId ?? throw new NullReferenceException(nameof(userId)));
        }
    }
}
