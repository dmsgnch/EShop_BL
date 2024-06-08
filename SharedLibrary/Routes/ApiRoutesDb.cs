namespace SharedLibrary.Routes;

public static class ApiRoutesDb
{
    public static class Universal
    {
        public const string Create = "add";
        public const string Delete = "delete/{id}";
        public const string Update = "update";
        public const string GetAll = "getAll";
        public const string GetById = "GetById/{id}";
    }
}