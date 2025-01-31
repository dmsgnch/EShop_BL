﻿using SharedLibrary.Requests;
using SharedLibrary.Responses;

namespace EShop_BL.Services.Secondary.Abstract;

public interface IAuthenticationService
{
    Task<UniversalResponse<string>> RegisterAsync(RegisterRequest registerRequest);
    Task<UniversalResponse<string>> LoginAsync(string email, string password);
}
