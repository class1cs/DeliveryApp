using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
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

        // Достаем утверждения о юзере по его роли, далее преобразовываем в Claim и в список
        var claims = new List<Claim>();

        claims.Add(new(ClaimTypes.Role, user.Role.Name));
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

public class AuthOptions
{
    public const string Issuer = "DeliveryApp"; // издатель токена

    public const string Audience = "Client"; // потребитель токена

    private const string Key = "ultrasecretkey_topsecret131111237"; // ключ для шифрования
    
    public static SymmetricSecurityKey GetSymmetricSecurityKey() => new(Encoding.UTF8.GetBytes(Key));
}