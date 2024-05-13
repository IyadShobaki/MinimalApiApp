using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MinimalApi.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MinimalApi.Endpoints;

public static class AuthenticationEndpoints
{

    public static void AddAuthenticationEndpoints(this WebApplication app)
    {

        app.MapPost("/api/token", [AllowAnonymous] (IConfiguration config, [FromBody] AuthenticationData data) =>
        {
            var user = ValidateCredentials(data);

            if (user is null)
            {
                return Results.Unauthorized();
            }

            string token = GenerateToken(user, config);


            return Results.Ok(token);
        });

        // Filtering endpoint example
        /*  app.MapPost("/api/token", [AllowAnonymous] (AuthenticationData data) =>
          {
              return Results.Ok();
          }).AddEndpointFilter(async (context, next) =>
          {
              var taskArgument = context.GetArgument<AuthenticationData>(0);
              var errors = new Dictionary<string, string[]>();
              // UserName contains only letters and numbers, and it must be between 6 and 16 characters long
              Regex usernameRegex = new("^[0-9A-Za-z].{6,16}$");
              if (!string.IsNullOrWhiteSpace(taskArgument.UserName) && !usernameRegex.IsMatch(taskArgument.UserName))
              {
                  errors.Add(nameof(AuthenticationData.UserName), 
                ["UserName can only be letters and numbers, and it must be between 6 and 16 characters long"]);
              }

              // Password  !string.IsNullOrWhiteSpace(taskArgument.Password) && 
              // valid, but not strong     ^(?=.*?[0-9])(?=.*?[A-Za-z]).{8,32}$
              // strong    ^(?=.*?[0-9])(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[^0-9A-Za-z]).{8,32}$
              //Regex passwordRegex = new Regex("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,32}$");
              Regex passwordRegex = new Regex("^(?=.*?[0-9])(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[^0-9A-Za-z]).{8,32}$");
              bool test = passwordRegex.IsMatch(taskArgument.Password);
              if (!passwordRegex.IsMatch(taskArgument.Password))
              {
                  errors.Add(nameof(AuthenticationData.Password),
                ["Password requires at least one number, one uppercase letter, one lowercase letter, one special character, and it must be between 8 and 32 characters long "]);
              }

              if (errors.Count > 0)
              {
                  return Results.ValidationProblem(errors);
              }
              return await next(context);
          });*/
    }

    private static string GenerateToken(UserData user, IConfiguration config)
    {
        var secretKey = new SymmetricSecurityKey(
           Encoding.ASCII.GetBytes(
              config.GetValue<string>("Authentication:SecretKey")));

        var signingCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

        List<Claim> claims = new();
        claims.Add(new(JwtRegisteredClaimNames.Sub, user.Id.ToString()));
        claims.Add(new(JwtRegisteredClaimNames.UniqueName, user.UserName));
        claims.Add(new(JwtRegisteredClaimNames.GivenName, user.FirstName));
        claims.Add(new(JwtRegisteredClaimNames.FamilyName, user.LastName));

        var token = new JwtSecurityToken(
           config.GetValue<string>("Authentication:Issuer"),
           config.GetValue<string>("Authentication:Audience"),
           claims,
           DateTime.UtcNow,
           DateTime.UtcNow.AddMinutes(1),
           signingCredentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private static UserData? ValidateCredentials(AuthenticationData data)
    {
        // THIS IS NOT PRODUCTION CODE - REPLACE THIS WITH A CALL TO YOUR AUTH SYSTEM
        if (CompareValues(data.UserName, "ishobaki") &&
            CompareValues(data.Password, "Test123"))
        {
            return new UserData(1, "Iyad", "Shobaki", data.UserName!);
        }

        if (CompareValues(data.UserName, "sstorm") &&
           CompareValues(data.Password, "Test123"))
        {
            return new UserData(2, "Sue", "Storm", data.UserName!);
        }

        return null;
    }

    private static bool CompareValues(string? actual, string expected)
    {
        if (actual is not null)
        {
            if (actual.Equals(expected))
            {
                return true;
            }

        }
        return false;
    }
}
