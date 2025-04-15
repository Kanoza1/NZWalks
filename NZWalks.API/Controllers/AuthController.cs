using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace NZWalks.API.Controllers;
[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly UserManager<IdentityUser> userManager;
    private readonly ITokenRepository tokenRepository;

    public AuthController(UserManager<IdentityUser> userManager , ITokenRepository tokenRepository)
    {
        this.userManager = userManager;
        this.tokenRepository = tokenRepository;
    }
    // POst: /api/auth/Register
    [HttpPost("Register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequestDto registerRequestDto)
    {
        var identityUser = new IdentityUser()
        {
            UserName = registerRequestDto.UserName,
            Email = registerRequestDto.UserName,
        };
        var identityResult = await userManager.CreateAsync(identityUser, registerRequestDto.Password);
        if (identityResult.Succeeded)
        {
            // Add roles to this user 
            //await userManager.AddToRoleAsync(identityUser, "reader");
            //await userManager.AddToRoleAsync(identityUser, "writer");
            //await userManager.AddToRoleAsync(identityUser, registerRequestDto.Roles);
            //return Ok(new
            //{
            //    UserName = identityUser.UserName,
            //    Email = identityUser.Email,
            //    Id = identityUser.Id,
            //    Roles = await userManager.GetRolesAsync(identityUser)
            //});
            //return CreatedAtAction(nameof(GetById), new { id = identityUser.Id }, identityUser);

            if (registerRequestDto.Roles != null && registerRequestDto.Roles.Any())
            {
                identityResult = await userManager.AddToRolesAsync(identityUser, registerRequestDto.Roles);
                if (identityResult.Succeeded)
                {
                    return Ok("User was registered! please login.");
                }
            }
        }
        // if identityResult is not succeeded
        // return the errors
        return BadRequest("SomeThing went wrong");

    }

    // POst: /api/auth/Login
    [HttpPost("Login")]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto loginRequestDto)
    {
        var identityUser = await userManager.FindByEmailAsync(loginRequestDto.UserName);
        if (identityUser != null)
        {
            var isPasswordValid = await userManager.CheckPasswordAsync(identityUser, loginRequestDto.Password);
            if (isPasswordValid)
            {
                // get roles for this user
                var roles = await userManager.GetRolesAsync(identityUser);
                if (roles != null)
                {
                    // create token
                    var jwtToken = tokenRepository.CreateJWTToken(identityUser, roles.ToList());
                    var response = new LoginResponseDto
                    {
                        jwtToken = jwtToken
                    };
                    return Ok(response);
                }
            }
        }
        return BadRequest("Invalid UserName or Password");
    }
}
