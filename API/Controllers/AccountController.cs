using System.Security.Claims;
using API.Dtos;
using API.Errors;
using API.Extensions;
using AutoMapper;
using Core.Entities.Identity;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

/// <summary>
/// Account Controller
/// </summary>
public class AccountController : BaseApiController
{
    private readonly UserManager<AppUser> _userManager;
    private readonly SignInManager<AppUser> _signInManager;
    private readonly ITokenService _tokenService;
    private readonly IMapper _mapper;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="userManager"></param>
    /// <param name="signInManager"></param>
    /// <param name="token"></param>
    /// <param name="mapper"></param>
    public AccountController(
        UserManager<AppUser> userManager,
        SignInManager<AppUser> signInManager,
        ITokenService token,
        IMapper mapper
    )
    {
        this._signInManager = signInManager;
        this._userManager = userManager;
        this._tokenService = token;
        _mapper = mapper;
    }

    /// <summary>
    /// Get Current LoggedIn User
    /// </summary>
    /// <returns></returns>
    [Authorize]
    [HttpGet]
    public async Task<ActionResult<UserDto>> GetCurrentUser()
    {
        //TODO: Two ways to claims the token in comment old one and new updated for email variable0
        // var email = HttpContext.User
        //     ?.Claims?.FirstOrDefault(x => x.Type == ClaimTypes.Email)
        //     ?.Value;

        var email = User.FindFirstValue(ClaimTypes.Email);

        AppUser user = await _userManager.FindByEmailAsync(email);

        return new UserDto
        {
            Email = user.Email,
            Token = _tokenService.CreateToken(user),
            DisplayName = user.DisplayName
        };
    }

    /// <summary>
    /// CheckEmailExists already
    /// </summary>
    /// <param name="email"></param>
    /// <returns></returns>
    [HttpGet("emailexists")]
    public async Task<ActionResult<bool>> CheckEmailExistsAsync([FromQuery] string email)
    {
        return await _userManager.FindByEmailAsync(email) != null;
    }

    /// <summary>
    /// Get User with address
    /// </summary>
    /// <returns></returns>
    [Authorize]
    [HttpGet("address")]
    public async Task<ActionResult<AddressDto>> GetUserAddress()
    {
        var claims = User;
        AppUser user = await _userManager.FindUserByClaimPrincipalWithAddress(claims);

        return this._mapper.Map<Address, AddressDto>(user.Address);
    }

    /// <summary>
    /// Update LoggedIn user Address
    /// </summary>
    /// <param name="address"></param>
    /// <returns></returns>
    [Authorize]
    [HttpPut("address")]
    public async Task<ActionResult<AddressDto>> UpdateUserAddress(AddressDto address)
    {
        var claims = User;
        AppUser user = await _userManager.FindUserByClaimPrincipalWithAddress(claims);
        user.Address = this._mapper.Map<AddressDto, Address>(address);

        var result = await this._userManager.UpdateAsync(user);

        if (result.Succeeded)
            return this._mapper.Map<Address, AddressDto>(user.Address);

        return BadRequest("Problem in user updation!");
    }

    /// <summary>
    /// Login User
    /// </summary>
    /// <param name="loginDto"></param>
    /// <returns></returns>
    [HttpPost("login")]
    public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
    {
        AppUser user = await _userManager.FindByEmailAsync(loginDto.Email);
        if (user == null)
            return Unauthorized(new ApiErrorResponse(401));

        var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);

        if (!result.Succeeded)
            return Unauthorized(new ApiErrorResponse(401));

        return new UserDto
        {
            Email = user.Email,
            Token = _tokenService.CreateToken(user),
            DisplayName = user.DisplayName
        };
    }

    /// <summary>
    /// Sign-Up User
    /// </summary>
    /// <param name="registerDto"></param>
    /// <returns></returns>
    [HttpPost("register")]
    public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
    {
        if (CheckEmailExistsAsync(registerDto.Email).Result.Value)
        {
            return new BadRequestObjectResult(
                new ApiValidationErrorResponse { Errors = new[] { "Email address is in use" } }
            );
        }
        var user = new AppUser
        {
            DisplayName = registerDto.DisplayName,
            Email = registerDto.Email,
            UserName = registerDto.Email
        };

        var result = await _userManager.CreateAsync(user, registerDto.Password);

        if (!result.Succeeded)
            return BadRequest(new ApiErrorResponse(400));

        return new UserDto
        {
            DisplayName = user.DisplayName,
            Token = _tokenService.CreateToken(user),
            Email = user.Email
        };
    }
}
