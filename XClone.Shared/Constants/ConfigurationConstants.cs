namespace XClone.Shared.Constants
{
    public static class ConfigurationConstants
    {
        //FIRST_APP
        public static string FIRST_APP_TIME_USER_USERNAME = "FirstAppTime:User:UserName";
        public static string FIRST_APP_TIME_USER_DISPLAYNAME = "FirstAppTime:User:DisplayName";
        public static string FIRST_APP_TIME_USER_EMAIL = "FirstApptime:User:Email";
        public static string FIRST_APP_TIME_USER_POSITION = "FirstAppTime:User:Position";
        public static string FIRST_APP_TIME_USER_PASSWORD = "FirstApptime:User:Password";


        //ConnectionStrings
        public static string CONNECTION_STRING_DATABASE = "ConnectionStrings:Database";



        //JWT
        public static string JWT_PRIVATE_KEY = "Jwt:PrivateKey";
        public static string JWT_AUDIENCE = "Jwt:Audience";
        public static string JWT_ISSUER = "Jwt:Issuer";
        public static string JWT_EXPIRATION_IN_MINUTES_MIN = "Jwt:ExpirationInMinutesMin";
        public static string JWT_EXPIRATION_IN_MINUTES_MAX = "Jwt:ExpirationInMinutesMax";

        //Auth
        public const string AUTH_REFRESH_TOKEN_EXPIRATION_IN_DAYS = "Auth:RefreshToken:ExpirationInDays";

    }
}
