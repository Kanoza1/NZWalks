using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace NZWalks.API.Repositories;

public class TokenRepository : ITokenRepository
{
    private readonly IConfiguration configuration;
    //private readonly SymmetricSecurityKey symmetricSecurityKey;
    public TokenRepository(IConfiguration configuration)
    {
        this.configuration = configuration;
        //symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]));
    }

    public string CreateJWTToken(IdentityUser identityUser, List<string> roles)
    {
        // create claims
        var claims = new List<Claim>();
        claims.Add(new Claim(ClaimTypes.Email, identityUser.Email));

        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }
        var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]));
        var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);
        // create token
        //var tokenHandler = new JwtSecurityTokenHandler();

        var token = new JwtSecurityToken(
            configuration["Jwt:Issuer"],
            configuration["Jwt:Audience"],
            claims,
            expires: DateTime.Now.AddMinutes(15),
            signingCredentials: signingCredentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

}
