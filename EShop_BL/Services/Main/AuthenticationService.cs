using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;
using EShop_BL.Components;
using EShop_BL.Helpers;
using EShop_BL.Common.Constants;
using EShop_BL.Extensions;
using EShop_BL.Services.Secondary.Abstract;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using SharedLibrary.Models.DtoModels.MainModels;
using SharedLibrary.Models.Enums;
using SharedLibrary.Requests;
using SharedLibrary.Responses;
using SharedLibrary.Routes;

namespace EShop_BL.Services.Secondary;

public class AuthenticationService : IAuthenticationService
{
    private readonly IConfiguration _configuration;
    private readonly HttpClientServiceBase _httpClient;
    private readonly IHashProvider _hashProvider;

    private readonly int _jwtTokenDaysLiveTime = 24;

    public AuthenticationService(IConfiguration configuration, HttpClientServiceBase httpClient,
        IHashProvider hashProvider)
    {
        _configuration = configuration;
        _httpClient = httpClient;
        _hashProvider = hashProvider;
    }

    public async Task<UniversalResponse<string>> RegisterAsync(RegisterRequest registerRequest)
    {
        return await GetAllUsersAsync(registerRequest);
    }

    #region Register logic

    private async Task<UniversalResponse<string>> GetAllUsersAsync(RegisterRequest registerRequest)
    {
        var usersResponse = await _httpClient.ProcessRequestAsync(new HttpRequestForm(
            ApiRoutesDb.Controllers.UserContr + ApiRoutesDb.UniversalActions.GetAllAction,
            HttpMethod.Get));

        var serverResponse = await JsonHelper.GetTypeFromResponseAsync<UniversalResponse<List<UserDTO>>>(usersResponse);

        if (!usersResponse.IsSuccessStatusCode)
        {
            return new UniversalResponse<string>(errorInfo: serverResponse.ErrorInfo);
        }

        if (serverResponse.ResponseObject is null) throw new Exception("List<User> is null");

        return await CheckUsersAsync(registerRequest, serverResponse.ResponseObject);
    }

    private async Task<UniversalResponse<string>> CheckUsersAsync(RegisterRequest registerRequest, List<UserDTO> users)
    {
        if (users.Any(u => u.Email.Equals(registerRequest.Email)))
            return new UniversalResponse<string>(
                errorInfo: ErrorMessages.AuthenticationMessages.UserAlreadyExistByEmail);
        if (users.Any(u => u.PhoneNumber.Equals(registerRequest.PhoneNumber)))
            return new UniversalResponse<string>(
                errorInfo: ErrorMessages.AuthenticationMessages.UserAlreadyExistByPhone);

        if (users.Any(u => !Regex.IsMatch(u.Email, @"^[^@\s]+@[^@\s]+\.(com|net|org|gov)$", RegexOptions.IgnoreCase)))
            return new UniversalResponse<string>(errorInfo: ErrorMessages.AuthenticationMessages.IncorrectEmailFormat);

        return await AddNewUserAsync(registerRequest);
    }

    private async Task<UniversalResponse<string>> AddNewUserAsync(RegisterRequest registerRequest)
    {
        var user = registerRequest.ToUserDto();
        user.ProvideSaltAndHash(_hashProvider);

        var response =
            await _httpClient.ProcessRequestAsync(new HttpRequestForm(
                endPoint: ApiRoutesDb.Controllers.UserContr + ApiRoutesDb.UniversalActions.CreateAction,
                requestMethod: HttpMethod.Post,
                jsonData: JsonConvert.SerializeObject(user)));

        var serverResponse = await JsonHelper.GetTypeFromResponseAsync<UniversalResponse<UserDTO>>(response);

        if (!response.IsSuccessStatusCode)
        {
            return new UniversalResponse<string>(errorInfo: serverResponse.ErrorInfo);
        }

        if (serverResponse.ResponseObject is null) throw new Exception("Response object is null");

        return new UniversalResponse<string>(
            responseObject: GenerateJwtToken(AssembleClaimsIdentity(serverResponse.ResponseObject)),
            info: SuccessMessages.AuthenticationMessages.SuccessfulRegister);
    }

    #endregion

    public async Task<UniversalResponse<string>> LoginAsync(string email, string password)
    {
        var usersResponse = await _httpClient.ProcessRequestAsync(new HttpRequestForm(
            endPoint: ApiRoutesDb.Controllers.UserContr + ApiRoutesDb.UniversalActions.GetAllAction,
            requestMethod: HttpMethod.Get));

        var serverResponse = await JsonHelper.GetTypeFromResponseAsync<UniversalResponse<List<UserDTO>>>(usersResponse);

        if (!usersResponse.IsSuccessStatusCode)
        {
            return new UniversalResponse<string>(errorInfo: serverResponse.ErrorInfo);
        }

        if (serverResponse.ResponseObject is null) throw new Exception("Response object is null");

        UserDTO? user = serverResponse.ResponseObject.FirstOrDefault(u => u.Email.Equals(email));

        if (user is null || user.PasswordHash != _hashProvider.ComputeHash(password, user.Salt))
            return new UniversalResponse<string>(errorInfo: ErrorMessages.AuthenticationMessages
                .IncorrectEmailOrPassword);

        return new UniversalResponse<string>(
            responseObject: GenerateJwtToken(AssembleClaimsIdentity(user)),
            info: SuccessMessages.AuthenticationMessages.SuccessfulLogin);
    }

    private ClaimsIdentity AssembleClaimsIdentity(UserDTO user)
    {
        var subject = new ClaimsIdentity(new[]
        {
            new Claim(Claims.UserId.ToString(), user.UserDtoId.ToString()),
            new Claim(Claims.Name.ToString(), user.Name.ToString()),
            new Claim(Claims.LastName.ToString(), user.LastName.ToString()),
            new Claim(Claims.Role.ToString(), user.RoleDto?.RoleTag.ToString() ?? throw new Exception("Role is null")),
        });

        if (user.SellerDtoId is not null)
            subject.AddClaim(new Claim(Claims.SellerId.ToString(), user.SellerDtoId.ToString()));
        if (user.RoleDto.RoleTag == RoleTag.Seller && user.SellerDtoId is null)
            throw new Exception("Seller user don`t have Seller entity0");

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