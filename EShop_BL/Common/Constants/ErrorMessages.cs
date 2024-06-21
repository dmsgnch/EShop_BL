namespace EShop_BL.Common.Constants;

public static class ErrorMessages
{
    public static class AuthenticationMessages
    {
        public const string UserAlreadyExistByEmail = $"User with this email already exists";
        public const string UserAlreadyExistByPhone = $"User with this phone number already exists";
        public const string IncorrectEmailFormat = $"Incorrect email format";
        public const string IncorrectEmailOrPassword = $"Email or password is incorrect";
    }
    
    public static class UserMessages
    {
        public const string TokenIsIncorrect = $"Token is incorrect";
    }
}