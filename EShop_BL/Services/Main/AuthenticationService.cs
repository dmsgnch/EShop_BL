using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;
using EShop_BL.Components;
using EShop_BL.Helpers;
using EShop_BL.Common.Constants;
using EShop_BL.Services.Main.Abstract;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using SharedLibrary.Models.MainModels;
using SharedLibrary.Requests;
using SharedLibrary.Responses;
using SharedLibrary.Routes;

namespace EShop_BL.Services.Main;

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

    public async Task<AuthenticationResponse> RegisterAsync(RegisterRequest registerRequest)
    {
        return await GetAllUsersAsync(registerRequest);
    }
    
    #region Register logic

    private async Task<AuthenticationResponse> GetAllUsersAsync(RegisterRequest registerRequest)
    {
        var usersResponse = await _httpClient.SendRequestAsync(new RestRequestForm(
            ApiRoutesDb.Controllers.User + ApiRoutesDb.Universal.GetAll,
            HttpMethod.Get));

        if (!usersResponse.IsSuccessStatusCode)
        {
            var errorResponse = await JsonHelper.GetTypeFromResponseAsync<LambdaResponse>(usersResponse);

            return new AuthenticationResponse(info: errorResponse.Info);
        }

        var users = await JsonHelper.GetTypeFromResponseAsync<List<User>>(usersResponse);

        return await CheckUsersAsync(registerRequest, users);
    }

    private async Task<AuthenticationResponse> CheckUsersAsync(RegisterRequest registerRequest, List<User> users)
    {
        if (users.Any(u => u.Email.Equals(registerRequest.Email)))
            return new AuthenticationResponse(info: ErrorMessages.Authentication.UserAlreadyExistByEmail);
        if (users.Any(u => u.PhoneNumber.Equals(registerRequest.PhoneNumber)))
            return new AuthenticationResponse(info: ErrorMessages.Authentication.UserAlreadyExistByPhone);

        if (users.Any(u => !Regex.IsMatch(u.Email, @"^[^@\s]+@[^@\s]+\.(com|net|org|gov)$", RegexOptions.IgnoreCase)))
            return new AuthenticationResponse(info: ErrorMessages.Authentication.IncorrectEmailFormat);

        return await AddNewUserAsync(registerRequest);
    }

    private async Task<AuthenticationResponse> AddNewUserAsync(RegisterRequest registerRequest)
    {
        var user = GetFilledUser(registerRequest);

        var response =
            await _httpClient.SendRequestAsync(new RestRequestForm(
                ApiRoutesDb.Controllers.User + ApiRoutesDb.Universal.Create,
                HttpMethod.Post, jsonData: JsonConvert.SerializeObject(user)));

        if (!response.IsSuccessStatusCode)
        {
            var errorResponse = await JsonHelper.GetTypeFromResponseAsync<LambdaResponse>(response);

            return new AuthenticationResponse(info: errorResponse.Info);
        }

        return new AuthenticationResponse(GenerateJwtToken(AssembleClaimsIdentity(user)),
            SuccessMessages.Authentication.SuccessfulRegister);
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

            PasswordHash = registerRequest.Password,
        };
        user.ProvideSaltAndHash(_hashProvider);

        return user;
    }
    
    #endregion 

    public async Task<AuthenticationResponse> LoginAsync(string email, string password)
    {
        var usersResponse = await _httpClient.SendRequestAsync(new RestRequestForm(
            ApiRoutesDb.Controllers.User + ApiRoutesDb.Universal.GetAll,
            HttpMethod.Get));

        if (!usersResponse.IsSuccessStatusCode)
        {
            var errorResponse = await JsonHelper.GetTypeFromResponseAsync<LambdaResponse>(usersResponse);

            return new AuthenticationResponse(info: errorResponse.Info);
        }

        var users = await JsonHelper.GetTypeFromResponseAsync<List<User>>(usersResponse);

        User? user = users.FirstOrDefault(u => u.Email.Equals(email));

        if (user is null || user.PasswordHash != _hashProvider.ComputeHash(password, user.Salt))
            return new AuthenticationResponse(info: ErrorMessages.Authentication.IncorrectEmailOrPassword);

        return new AuthenticationResponse(GenerateJwtToken(AssembleClaimsIdentity(user)),
            SuccessMessages.Authentication.SuccessfulLogin);
    }

    private ClaimsIdentity AssembleClaimsIdentity(User user)
    {
        var subject = new ClaimsIdentity(new[]
        {
            new Claim("id", user.UserId.ToString()),
            new Claim("name", user.Name.ToString()),
            new Claim("lastName", user.LastName.ToString()),
            new Claim("email", user.Email.ToString()),
            new Claim("phoneNumber", user.PhoneNumber.ToString()),
            new Claim("role", user.Role.RoleTag.ToString()),
        });

        return subject;
    }

    private string GenerateJwtToken(ClaimsIdentity subject)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]
                                          ?? throw new Exception("Config key error"));

        if (key.Length < 32)
        {
            throw new Exception("The key must be at least 32 bytes long.");
        }

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