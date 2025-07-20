using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using ServiceSitoPanel.src.dtos.users;
using ServiceSitoPanel.src.helpers;

namespace ServiceSitoPanel.Helpers;

public class JwtService
{
    private readonly IConfiguration _configuration;
    private readonly IHttpContextAccessor _httpContextAccessor;


    public JwtService(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
    {
        _configuration = configuration;
        _httpContextAccessor = httpContextAccessor;
    }

    private TokenValidationParameters GetValidationParameters()
    {
        return new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey =
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Secret"])),
            ValidateAudience = false,
            ValidateIssuer = false,
            ClockSkew = TimeSpan.Zero,
            ValidateLifetime = true
        };
    }

    public bool ValidateToken(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var validationParameters = GetValidationParameters();

        try
        {
            tokenHandler.ValidateToken(token, validationParameters, out var validatedToken);
            return true;
        }
        catch (Exception ex)
        {
            return false;
        }
    }

    public ReadUserSessionDTO GenerateJwtToken(CreateUserSessionDTO user)
    {
        var identity = new ClaimsIdentity(new[]
        {
            new Claim("id", user.id),
            new Claim("name", user.name),
            new Claim("tenant_id", user.tenant_id),
            new Claim("loginTimeStamp", DateTime.UtcNow.ToString())
        });

        var chave = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Secret"]));
        var signInCredentials = new SigningCredentials(chave, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            expires: DateTime.Now.NowInBrasilia().AddHours(5),
            claims: identity.Claims,
            signingCredentials: signInCredentials
        );

        string tokenString = new JwtSecurityTokenHandler().WriteToken(token);

        return new ReadUserSessionDTO(
            user.id,
            user.name,
            user.tenant_id,
            DateTime.Now.NowInBrasilia().ToString(),
            tokenString
        );
    }

    public string? GetTenantFromToken()
    {
        var token = GetTokenFromCookie();
        var tokenHandler = new JwtSecurityTokenHandler();
        var validationParameters = GetValidationParameters();

        try
        {
            var principal = tokenHandler.ValidateToken(token, validationParameters, out var validatedToken);
            var tenantIdClaim = principal.FindFirst("tenant_id");

            return tenantIdClaim?.Value;
        }
        catch
        {
            return null;
        }
    }

    public string? GetIdFromToken()
    {
        var token = GetTokenFromCookie();
        var tokenHandler = new JwtSecurityTokenHandler();
        var validationParameters = GetValidationParameters();

        try
        {
            var principal = tokenHandler.ValidateToken(token, validationParameters, out var validatedToken);
            var id = principal.FindFirst("id");

            return id?.Value;
        }
        catch
        {
            return null;
        }
    }

    public string? GetTokenFromCookie()
    {
        return _httpContextAccessor.HttpContext.Request.Cookies["accessToken"];
    }
}