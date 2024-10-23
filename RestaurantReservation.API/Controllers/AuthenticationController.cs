using Microsoft.AspNetCore.Mvc;
using RestaurantReservation.API.Authentication;
using RestaurantReservation.Db.Models;
using RestaurantReservation.Db.Repositories.UserRepository;

namespace RestaurantReservation.API.Controllers;

[ApiController]
[Route("api/authentication")]
public class AuthenticationController : ControllerBase
{
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly IUserRepository _userRepository;
    private readonly JwtAuthenticationConfig _jwtAuthenticationConfig;
    
    public AuthenticationController(IJwtTokenGenerator jwtTokenGenerator, IUserRepository userRepository, JwtAuthenticationConfig jwtAuthenticationConfig)
    {
        _jwtTokenGenerator = jwtTokenGenerator;
        _userRepository = userRepository;
        _jwtAuthenticationConfig = jwtAuthenticationConfig;
    }
    
    
    /// <summary>
    /// Authenticates a user and returns a JWT token if successful.
    /// </summary>
    /// <param name="authenticationRequestBody">The login credentials.</param>
    /// <returns>Returns an access token and user information if authentication is successful.</returns>
    /// <response code="200">If the user is authenticated successfully.</response>
    /// <response code="401">If the user credentials are invalid.</response>
    [HttpPost("authenticate")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<object>> Authenticate(AuthenticationRequestBody authenticationRequestBody)
    {
        var user = await ValidateUserCredentials(authenticationRequestBody);

        if (user == null)
        {
            return Unauthorized("Invalid username or password.");
        }

        var token = _jwtTokenGenerator.GenerateToken(user);
        var expiration = DateTime.UtcNow.AddMinutes(_jwtAuthenticationConfig.TokenExpiryInMinutes);

        return Ok(new
        {
            access_token = token,
            token_type = "Bearer",
            expires_in = expiration,
            user_info = new
            {
                user_id = user.UserId,
                first_name = user.FirstName,
                last_name = user.LastName
            }
        });

    }

    private async Task<User?> ValidateUserCredentials(AuthenticationRequestBody authenticationRequestBody)
    {
        var user = await _userRepository.Get(authenticationRequestBody.Username, authenticationRequestBody.Password);

        return user;

    }
}