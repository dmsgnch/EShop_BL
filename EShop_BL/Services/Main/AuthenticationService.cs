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
using SharedLibrary.Models.DbModels.MainModels;
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

    public async Task<LambdaResponse<string>> RegisterAsync(RegisterRequest registerRequest)
    {
        return await GetAllUsersAsync(registerRequest);
    }

    #region Register logic

    private async Task<LambdaResponse<string>> GetAllUsersAsync(RegisterRequest registerRequest)
    {
        var usersResponse = await _httpClient.SendRequestAsync(new RestRequestForm(
            ApiRoutesDb.Controllers.UserContr + ApiRoutesDb.UniversalActions.GetAllPath,
            HttpMethod.Get));

        var serverResponse = await JsonHelper.GetTypeFromResponseAsync<LambdaResponse<List<User>>>(usersResponse);

        if (!usersResponse.IsSuccessStatusCode)
        {
            return new LambdaResponse<string>(errorInfo: serverResponse.ErrorInfo);
        }

        if (serverResponse.ResponseObject is null) throw new Exception("List<User> is null");

        return await CheckUsersAsync(registerRequest, serverResponse.ResponseObject);
    }

    private async Task<LambdaResponse<string>> CheckUsersAsync(RegisterRequest registerRequest, List<User> users)
    {
        if (users.Any(u => u.Email.Equals(registerRequest.Email)))
            return new LambdaResponse<string>(errorInfo: ErrorMessages.AuthenticationMessages.UserAlreadyExistByEmail);
        if (users.Any(u => u.PhoneNumber.Equals(registerRequest.PhoneNumber)))
            return new LambdaResponse<string>(errorInfo: ErrorMessages.AuthenticationMessages.UserAlreadyExistByPhone);

        if (users.Any(u => !Regex.IsMatch(u.Email, @"^[^@\s]+@[^@\s]+\.(com|net|org|gov)$", RegexOptions.IgnoreCase)))
            return new LambdaResponse<string>(errorInfo: ErrorMessages.AuthenticationMessages.IncorrectEmailFormat);

        return await AddNewUserAsync(registerRequest);
    }

    private async Task<LambdaResponse<string>> AddNewUserAsync(RegisterRequest registerRequest)
    {
        var user = GetFilledUser(registerRequest);

        var response =
            await _httpClient.SendRequestAsync(new RestRequestForm(
                ApiRoutesDb.Controllers.UserContr + ApiRoutesDb.UniversalActions.CreatePath,
                HttpMethod.Post, jsonData: JsonConvert.SerializeObject(user)));

        var serverResponse = await JsonHelper.GetTypeFromResponseAsync<LambdaResponse<User>>(response);

        if (!response.IsSuccessStatusCode)
        {
            return new LambdaResponse<string>(errorInfo: serverResponse.ErrorInfo);
        }
        
        if (serverResponse.ResponseObject is null) throw new Exception("Response object is null");

        return new LambdaResponse<string>(
            responseObject: GenerateJwtToken(AssembleClaimsIdentity(serverResponse.ResponseObject)),
            info: SuccessMessages.AuthenticationMessages.SuccessfulRegister);
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

    public async Task<LambdaResponse<string>> LoginAsync(string email, string password)
    {
        var usersResponse = await _httpClient.SendRequestAsync(new RestRequestForm(
            ApiRoutesDb.Controllers.UserContr + ApiRoutesDb.UniversalActions.GetAllPath,
            HttpMethod.Get));
        
        var serverResponse = await JsonHelper.GetTypeFromResponseAsync<LambdaResponse<List<User>>>(usersResponse);

        if (!usersResponse.IsSuccessStatusCode)
        {
            return new LambdaResponse<string>(errorInfo: serverResponse.ErrorInfo);
        }
        
        if (serverResponse.ResponseObject is null) throw new Exception("Response object is null");

        User? user = serverResponse.ResponseObject.FirstOrDefault(u => u.Email.Equals(email));

        if (user is null || user.PasswordHash != _hashProvider.ComputeHash(password, user.Salt))
            return new LambdaResponse<string>(errorInfo: ErrorMessages.AuthenticationMessages.IncorrectEmailOrPassword);

        return new LambdaResponse<string>(
            responseObject: GenerateJwtToken(AssembleClaimsIdentity(user)),
            info: SuccessMessages.AuthenticationMessages.SuccessfulLogin);
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
            new Claim("role", user.Role?.RoleTag.ToString() ?? throw new Exception("Role is null")),
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