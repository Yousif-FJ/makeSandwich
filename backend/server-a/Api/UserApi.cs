using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using server_a.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace server_a.Api;

[ApiController]
public class UserApi(UserManager<IdentityUser> userManager,
 IUserStore<IdentityUser> userStore, 
 SignInManager<IdentityUser> signInManager) 
: ControllerBase
{
    /// <summary>
    /// Create user
    /// </summary>
    /// <remarks>This can only be done by the logged in user.</remarks>
    /// <param name="body">Created user object</param>
    /// <response code="0">successful operation</response>
    [HttpPost]
    [Route("/v1/user")]
    public async Task<IActionResult> CreateUserAsync([FromBody] User body)
    {
        var emailStore = (IUserEmailStore<IdentityUser>)userStore;
        var email = body.Email;

        if (string.IsNullOrEmpty(email))
        {
            return BadRequest("invalid email");
        }

        var user = new IdentityUser(body.Username);
        await userStore.SetUserNameAsync(user, email, CancellationToken.None);
        await emailStore.SetEmailAsync(user, email, CancellationToken.None);
        var result = await userManager.CreateAsync(user, body.Password);

        if (!result.Succeeded)
        {
            return BadRequest(result.ToString());
        }

        return Ok();
    }


    /// <summary>
    /// Get user by user name
    /// </summary>
    /// <param name="username">The name that needs to be fetched. Use user1 for testing.</param>
    /// <response code="200">successful operation</response>
    /// <response code="400">Invalid username supplied</response>
    /// <response code="404">User not found</response>
    [HttpGet]
    [Route("/v1/user/{username}")]
    [ProducesResponseType(statusCode: 200, type: typeof(User))]
    public async Task<IActionResult> GetUserByName([FromRoute][Required] string username)
    {
        var user = await userManager.FindByNameAsync(username);

        if (user == null)
        {
            return NotFound();
        }

        return Ok(new User
        {
            Email = user.Email!,
            Username = user.UserName!,
            Password = "********"
        });
    }

    /// <summary>
    /// Logs user into the system
    /// </summary>
    /// <param name="user">The user for login (only email is respected here)</param>
    /// <response code="200">successful operation</response>
    /// <response code="400">Invalid username/password supplied</response>
    [HttpPost]
    [Route("/v1/user/login")]
    [ProducesResponseType(statusCode: 200, type: typeof(string))]
    public async Task<IActionResult> LoginUserAsync([FromBody] User user)
    {
        var useCookieScheme = true;
        var isPersistent = true;
        signInManager.AuthenticationScheme = useCookieScheme ? IdentityConstants.ApplicationScheme : IdentityConstants.BearerScheme;

        var result = await signInManager.PasswordSignInAsync(user.Email, user.Password, isPersistent,
         lockoutOnFailure: false);


        if (!result.Succeeded)
        {
            return BadRequest(result.ToString());
        }

        // The signInManager already produced the needed response in the form of a cookie or bearer token.
        return Ok();
    }

    /// <summary>
    /// Logs out current logged in user session
    /// </summary>
    /// <response code="0">successful operation</response>
    [HttpPost]
    [Route("/v1/user/logout")]
    [Authorize]
    public async Task<IActionResult> LogoutUser()
    {
        await signInManager.SignOutAsync();

        return Ok();
    }
}
