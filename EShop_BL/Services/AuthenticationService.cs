using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;
using EShop_BL.Components;
using EShop_BL.Helpers;
using EShop_BL.Models.MainModels;
using EShop_BL.Services.Abstract;
using EShop_BL.Common.Constants;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using SharedLibrary.Requests;
using SharedLibrary.Responses;
using SharedLibrary.Routes;

namespace EShop_BL.Services;

public class AuthenticationService : IAuthenticationService
{
    private readonly IConfiguration _configuration;
    private readonly IHttpClientService _httpClient;
    private readonly IHashProvider _hashProvider;

    private readonly int _jwtTokenDaysLiveTime = 24;

    public AuthenticationService(IConfiguration configuration, IHttpClientService httpClient,
        IHashProvider hashProvider)
    {
        _configuration = configuration;
        _httpClient = httpClient;
        _hashProvider = hashProvider;
    }

    private async Task<List<User>> GetAllUsers()
    {
        var response =
            await _httpClient.SendRequestAsync(new RestRequestForm("user/" + ApiRoutesDb.Universal.GetAll,
                HttpMethod.Get));
        return await JsonHelper.GetTypeFromResponse<List<User>>(response);
    }

    public async Task<AuthenticationResponse> RegisterAsync(RegisterRequest registerRequest)
    {
        List<User> users = await GetAllUsers();

        if (users.Any(u => u.Email.Equals(registerRequest.Email)))
            return new AuthenticationResponse(info: new string[]
                { ErrorMessages.Authentication.UserAlreadyExistByEmail });
        if (users.Any(u => u.PhoneNumber.Equals(registerRequest.PhoneNumber)))
            return new AuthenticationResponse(info: new string[]
                { ErrorMessages.Authentication.UserAlreadyExistByPhone });

        if (users.Any(u => !Regex.IsMatch(u.Email, @"^[^@\s]+@[^@\s]+\.(com|net|org|gov)$", RegexOptions.IgnoreCase)))
            return new AuthenticationResponse(info: new string[] { ErrorMessages.Authentication.IncorrectEmailFormat });

        var user = GetFilledUser(registerRequest);

        var response =
            await _httpClient.SendRequestAsync(new RestRequestForm("user/" + ApiRoutesDb.Universal.Create,
                HttpMethod.Post, jsonData: JsonConvert.SerializeObject(user)));

        if (!response.IsSuccessStatusCode)
        {
            return new AuthenticationResponse(info: new[] { await response.Content.ReadAsStringAsync() });
        }

        return new AuthenticationResponse(user, GenerateJwtToken(AssembleClaimsIdentity(user)),
            new[] { SuccessMessages.Authentication.SuccessfulRegister });
    }

    private User GetFilledUser(RegisterRequest registerRequest)
    {
        var user = new User()
        {
            UserId = Guid.NewGuid(),
            Name = registerRequest.Name,
            LastName = registerRequest.LastName,
            Patronymic = registerRequest.Patronymic,

            Email = registerRequest.Email,
            PhoneNumber = registerRequest.PhoneNumber,
        };
        user.ProvideSaltAndHash(_hashProvider);

        return user;
    }

    public async Task<AuthenticationResponse> LoginAsync(string email, string password)
    {
        List<User> users = await GetAllUsers();

        User? user = users.FirstOrDefault(u => u.Email.Equals(email));

        if (user is null || user.PasswordHash != _hashProvider.ComputeHash(password, user.Salt))
            return new AuthenticationResponse(info: new[] { ErrorMessages.Authentication.IncorrectEmailOrPassword });

        return new AuthenticationResponse(user, GenerateJwtToken(AssembleClaimsIdentity(user)),
            new[] { SuccessMessages.Authentication.SuccessfulLogin });
    }

    private ClaimsIdentity AssembleClaimsIdentity(User user)
    {
        var subject = new ClaimsIdentity(new[]
        {
            new Claim("id", user.UserId.ToString())
        });

        return subject;
    }

    private string GenerateJwtToken(ClaimsIdentity subject)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"] ?? throw new Exception("Config key error"));
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = subject,
            Expires = DateTime.Now.AddHours(_jwtTokenDaysLiveTime),
            SigningCredentials =
                new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}