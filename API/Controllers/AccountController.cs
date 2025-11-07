using BusinessLogicLayer.DTOs.User;
using BusinessLogicLayer.Interfaces;
using DataAccessLayer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class AccountController : ControllerBase
{
    private readonly IAuthService _authService;

    public AccountController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    [ProducesResponseType(typeof(TokenResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(GenericResponseDto), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(GenericResponseDto), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Register([FromBody] UserRegisterDto dto)
    {
        try
        {
            var token = await _authService.RegisterAsync(dto);
            return Ok(token);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError,
                new GenericResponseDto
                {
                    Status = "Error",
                    Message = ex.Message
                });
        }
    }


    [HttpPost("login")]
    [ProducesResponseType(typeof(TokenResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(GenericResponseDto), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(GenericResponseDto), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Login([FromBody] UserLoginDto dto)
    {
        try
        {
            var token = await _authService.LoginAsync(dto);
            return Ok(token);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError,
                new GenericResponseDto
                {
                    Status = "Error",
                    Message = ex.Message
                });
        }
    }

    [Authorize]
    [HttpPost("logout")]
    [ProducesResponseType(typeof(GenericResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(GenericResponseDto), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(GenericResponseDto), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(GenericResponseDto), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(GenericResponseDto), StatusCodes.Status500InternalServerError)]

    public IActionResult Logout()
    {
        try
        {
            return Ok(new GenericResponseDto
            {
                Status = "Success",
                Message = "User logged out successfully"
            });
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new GenericResponseDto
            {
                Status = "Error",
                Message = ex.Message
            });
        }
    }

    [Authorize]
    [HttpGet("profile")]
    [ProducesResponseType(typeof(UserProfileDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(GenericResponseDto), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(GenericResponseDto), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(GenericResponseDto), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetProfile()
    {
        try
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var profile = await _authService.GetProfileAsync(userId);
            return Ok(profile);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new GenericResponseDto
            {
                Status = "Error",
                Message = ex.Message
            });
        }
    }

    [Authorize]
    [HttpPut("profile")]
    [ProducesResponseType(typeof(GenericResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(GenericResponseDto), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(GenericResponseDto), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(GenericResponseDto), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(GenericResponseDto), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> EditProfile([FromBody] UserProfileEditDto dto)
    {
        try
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            await _authService.EditProfileAsync(userId, dto);

            return Ok();
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new GenericResponseDto
            {
                Status = "Error",
                Message = ex.Message
            });
        }
    }
}
