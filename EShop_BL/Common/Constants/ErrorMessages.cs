namespace EShop_BL.Common.Constants;

public static class ErrorMessages
{
    public static class Authentication
    {
        public const string UserAlreadyExistByEmail = $"User with this email already exists";
        public const string UserAlreadyExistByPhone = $"User with this phone number already exists";
        public const string IncorrectEmailFormat = $"Incorrect email format";
        public const string IncorrectEmailOrPassword = $"Email or password is incorrect";
    }
}