using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using DeliveryApp.DAL;

namespace DeliveryApp.BL;

public class TokenService
{
    private readonly ApplicationContext _appContext;

    public TokenService(ApplicationContext appContext) => _appContext = appContext;

    public async Task<string> GenerateTokenAsync(User user)
    {
        var role = user.Role;
        var claims = new List<Claim>();

        claims.Add(new(ClaimTypes.Role, user.Role.ToString()));
        claims.Add(new("Id", user.Id.ToString()));

        var jwt = new JwtSecurityToken(AuthOptions.Issuer,
            AuthOptions.Audience,
            claims,
            expires: DateTime.UtcNow.Add(TimeSpan.FromMinutes(2)),
            signingCredentials: new(AuthOptions.GetSymmetricSecurityKey(),
                SecurityAlgorithms.HmacSha256));

        var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

        return encodedJwt;
    }
}