namespace SharedLibrary.Routes;

public static class ApiRoutes
{
    public static class Controllers
    {
        public const string Authentication = "auth/";
        public const string User = "user/";
    }

    public static class Authentication
    {
        public const string Register = "reg";
        public const string Login = "login";
    }
    
    public static class User
    {
        public const string GetByToken = "GetByToken";
    }
}