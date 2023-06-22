using FlightDoc.Model;
using Microsoft.AspNetCore.Identity;

namespace FlightDoc.Security
{
    public interface IJwtTokenGenerator
    {
        string GenerateToken(ApplicationUser user);
    }
}