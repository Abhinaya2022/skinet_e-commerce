using API.Dtos;
using API.Errors;
using Carter;
using Core.Entities.Identity;
using Core.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace API.EndPoints;

public class AccountsModule : ICarterModule
{
    // private readonly ITokenService _token;

    // public AccountsModule(ITokenService token)
    // {
    //     _token = token;
    // }   

    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost(
            "/checkendpoint",
            async (
                RegisterDto registerDto,
                UserManager<AppUser> userManager,
                SignInManager<AppUser> signInManager
            ) =>
            {
               
                var user = new AppUser
                {
                    DisplayName = registerDto.DisplayName,
                    Email = registerDto.Email,
                    UserName = registerDto.Email
                };

                var result = await userManager.CreateAsync(user, registerDto.Password);

                if (!result.Succeeded)
                    return Results.BadRequest(new ApiErrorResponse(400));

                return Results.Ok(
                    new UserDto
                    {
                        DisplayName = user.DisplayName,
                        //Token = _token.CreateToken(user),
                        Email = user.Email
                    }
                );
            }
        );
    }
}
