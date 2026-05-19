using Microsoft.Extensions.Configuration;
using XClone.Application.Models.Helpers;
using XClone.Shared.Constants;

namespace XClone.Application.Helpers
{
    public static class CacheHelper
    {
        //otra forma de hacerlo
        //public static CacheKey AuthToken(string token, IConfiguration configuration)
        //{
        //    var rnd = new Random();
        //    var randomExpiration = rnd.Next(Convert.ToInt32(configuration[ConfigurationConstants.JWT_EXPIRATION_IN_MINUTES_MIN]), Convert.ToInt32(configuration[ConfigurationConstants.JWT_EXPIRATION_IN_MINUTES_MIN]));

        //    return new CacheKey
        //    {
        //        Key = $"auth:tokens:{token}",
        //        Expiration = TimeSpan.FromMinutes(randomExpiration)
        //    };
        //}

        public static string AuthTokenKey(string value)
        {
            return $"auth:tokens:{value}";
        }

        public static CacheKey AuthTokenCreation(string value, TimeSpan expiration)
        {
            return new CacheKey
            {
                Key = AuthTokenKey(value),
                Expiration = expiration
            };
        }

        public static string AuthRefreshTokenKey(string value)
        {
            return $"auth:refresh_tokens:{value}";
        }

        public static CacheKey AuthRefreshTokenCreation(string value, IConfiguration configuration)
        {
            return new CacheKey
            {
                Key = AuthRefreshTokenKey(value),
                Expiration = TimeSpan.FromDays(Convert.ToInt32(configuration[ConfigurationConstants.AUTH_REFRESH_TOKEN_EXPIRATION_IN_DAYS] ?? "15"))
            };
        }

        //otra forma de hacerlo
        //public static CacheKey AuthRefreshRoken(string value, IConfiguration configuration)
        //{
        //    return new CacheKey
        //    {
        //        Key = $"auth:refresh_token:{value}",
        //        //Arreglar
        //        Expiration = TimeSpan.FromDays(Convert.ToInt32(configuration[ConfigurationConstants.JWT_EXPIRATION_IN_MINUTES_MAX]))
        //    };
        //}




        public static string AuthRegisterTokenKey(string value)
        {
            return $"auth:register:tokens:{value}";
        }

        //crea un token y le asigna una expiracion, para luego ser almacenado en cache
        public static CacheKey AuthRegisterTokenCreation(string value, TimeSpan expiration)
        {
            return new CacheKey
            {
                Key = AuthRegisterTokenKey(value),
                Expiration = expiration
            };
        }

        public static string AuthRecoverPasswordOTPKey(string value)
        {
            return $"auth:recover_password:otps:{value}";
        }

        //crea un otp y le asigna una expiracion, para luego ser almacenado en cache
        public static CacheKey AuthRecoverPasswordOTPCreation(string value, TimeSpan expiration)
        {
            return new CacheKey
            {
                Key = AuthRecoverPasswordOTPKey(value),
                Expiration = expiration
            };
        }
    }
}
