using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using Serilog.Sinks.MSSqlServer;
using CloudinaryDotNet;
using StackExchange.Redis;
using XClone.Application.Helpers;
using XClone.Infrastructure.Services;
using XClone.Application.Interfaces.Services;
using XClone.Application.Models.Services;
using XClone.Application.Services;
using XClone.Domain.Database.SqlServer.Context;
using XClone.Domain.DataBase.SqlServer;
using XClone.Domain.Exceptions;
using XClone.Domain.Interfaces.Repositories;
using XClone.Infrastructure.Persistence.SqlServer;
using XClone.Infrastructure.Persistence.SqlServer.Repositories;
using XClone.Shared;
using XClone.Shared.Constants;
using XClone.WebApi.Middlewares;

namespace XClone.WebApi.Extensions
{
    public static class ServiceCollectionExtension
    {
        /// <summary>
        /// Metodo que sirve para añadir todos los servicios de la aplicacion, 
        /// como el servicio de post, el servicio de usuario, etc. 
        /// Este metodo se llama en el Program.cs para registrar los servicios en el contenedor de dependencias.
        /// </summary>
        /// <param name="services"></param>
        public static void AddServices(this IServiceCollection services)
        {
            services.AddScoped<IPostService, PostService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<ICacheService, CacheService>();
            services.AddScoped<IEmailTemplateService, EmailTemplateService>();
            services.AddScoped<IAppService, AppService>();
            services.AddScoped<IFollowingService, FollowingService>();
            services.AddScoped<IBlockService, BlockService>();
            services.AddScoped<IChatService, ChatService>();
            services.AddScoped<ICommunityService, CommunityService>();
            services.AddScoped<ILikeService, LikeService>();
            services.AddScoped<IReplyService, ReplyService>();
            services.AddScoped<IRepostService, RepostService>();
            services.AddScoped<IQuoteService, QuoteService>();
            services.AddScoped<IHashtagService, HashtagService>();
            services.AddScoped<IFeedService, FeedService>();
        }

        /// <summary>
        /// Método que sirve para añadir todos los repositorios de la aplicación
        ///
        /// </summary>
        /// <param name="services"></param>
        public static void AddRepositories(this IServiceCollection services)
        {
            services.AddTransient<IPostRepository, PostRepository>();
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IEmailTemplateRepository, EmailTemplateRepository>();
            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<IFollowingRepository, FollowingRepository>();
            services.AddScoped<IBlockRepository, BlockRepository>();
            services.AddScoped<IChatRepository, ChatRepository>();
            services.AddScoped<IMessageRepository, MessageRepository>();
            services.AddScoped<ICommunityRepository, CommunityRepository>();
            services.AddScoped<ICommunityMemberRepository, CommunityMemberRepository>();
            services.AddScoped<ILikeRepository, LikeRepository>();
            services.AddScoped<IReplyRepository, ReplyRepository>();
            services.AddScoped<IRepostRepository, RepostRepository>();
            services.AddScoped<IQuoteRepository, QuoteRepository>();
            services.AddScoped<IHashtagRepository, HashtagRepository>();
            services.AddScoped<IPostHashtagRepository, PostHashtagRepository>();
            services.AddScoped<IMediaFileRepository, MediaFileRepository>();
        }

        public async static Task AddSMTP(this IServiceCollection services, IConfiguration configuration)
        {
            var host = Environment.GetEnvironmentVariable(EnvironmentConstants.SMTP_HOST) //entonrno de producción
                ?? configuration[ConfigurationConstants.SMTP_HOST]//entorno de desarrollo
                ?? throw new Exception(ResponseConstants.ConfigurationPropertyNotFound(ConfigurationConstants.SMTP_HOST)); //si no se encuentra la configuración, se lanza una excepción

            var from = Environment.GetEnvironmentVariable(EnvironmentConstants.SMTP_FROM)
                ?? configuration[ConfigurationConstants.SMTP_FROM]
                ?? throw new Exception(ResponseConstants.ConfigurationPropertyNotFound(ConfigurationConstants.SMTP_FROM));

            var portValue = Environment.GetEnvironmentVariable(EnvironmentConstants.SMTP_PORT) ??
                configuration[ConfigurationConstants.SMTP_PORT];

            var port = Convert.ToInt32(portValue ?? "587");

            var user = Environment.GetEnvironmentVariable(EnvironmentConstants.SMTP_USER)
                ?? configuration[ConfigurationConstants.SMTP_USER]
                ?? throw new Exception(ResponseConstants.ConfigurationPropertyNotFound(ConfigurationConstants.SMTP_USER));

            var password = Environment.GetEnvironmentVariable(EnvironmentConstants.SMTP_PASSWORD)
                ?? configuration[ConfigurationConstants.SMTP_PASSWORD]
                ?? throw new Exception(ResponseConstants.ConfigurationPropertyNotFound(ConfigurationConstants.SMTP_PASSWORD));

            var smtp = new SMTP(host, from, port, user, password);
            services.AddSingleton(smtp);
        }

        /// <summary>
		/// Método que añade lo esencial que necesita nuestra aplicación para funcionar
		/// </summary>
		/// <param name="services"></param>
        public async static Task AddCore(this IServiceCollection services, IConfiguration configuration)
        {
            await services.AddSMTP(configuration);
            //ConfigureApiBehaviorOptions sirve para configurar el comportamiento de la API, como por ejemplo, el formato de los errores, etc.
            services.AddControllers().ConfigureApiBehaviorOptions(options =>
            {
                options.InvalidModelStateResponseFactory = (errorContext) =>
                {
                    var errors = errorContext.ModelState.Values.SelectMany(value => value.Errors.Select(error => error.ErrorMessage).ToList()).ToList();
                    var response = ResponseHelper.Create(
                        data: ValidationConstants.VALIDATION_MESSAGE,
                        errors: errors,
                        message: ValidationConstants.VALIDATION_MESSAGE
                        );
                    return new BadRequestObjectResult(response);
                };
            });

            services.AddOpenApi();

            //services.AddSqlServer<XcloneContext>(configuration.GetConnectionString("Database"));
            var databaseConnetingString = Environment.GetEnvironmentVariable(EnvironmentConstants.CONNECTION_STRING_DATABASE)
                ?? configuration[ConfigurationConstants.CONNECTION_STRING_DATABASE];

            services.AddSqlServer<XcloneContext>(databaseConnetingString);

            services.AddServices();

            services.AddRepositories();

            services.AddMiddlleWares();

            services.AddLogging(databaseConnetingString);

            services.AddAuth(configuration);

            services.AddCache(configuration);

            services.AddCloudinary(configuration);

            services.AddSignalR();

            await Initialize(services);

        }


        /// <summary>
		/// Método que añade los middlewares de la aplicación
		/// </summary>
		/// <param name="services"></param>
        public static void AddMiddlleWares(this IServiceCollection services)
        {
            services.AddScoped<ErrorHandlerMiddleware>();
        }

        /// <summary>
        /// Método para añadir todo lo relacionado con log
        /// </summary>
        /// <param name="services"></param>
        public static void AddLogging(this IServiceCollection services, string databaseConnectionString)
        {
            services.AddSerilog();

            Log.Logger = new LoggerConfiguration()
                .WriteTo.File(Path.Combine(Directory.GetCurrentDirectory(), "logs", "log.txt"), rollingInterval: RollingInterval.Day)
                .WriteTo.Console()
                .WriteTo
                .MSSqlServer(
                connectionString: databaseConnectionString,
                sinkOptions: new MSSqlServerSinkOptions { TableName = "LogEvents" })
                .CreateLogger();
        }



        public async static Task Initialize(this IServiceCollection services)
        {
            var templatesData = new EmailTemplateData();
            services.AddSingleton(templatesData);

            var provider = services.BuildServiceProvider();
            var scope = provider.CreateAsyncScope();

            var userService = scope.ServiceProvider.GetRequiredService<IUserService>();
            await userService.CreateFirstUser();

            var emailTemplateService = scope.ServiceProvider.GetRequiredService<IEmailTemplateService>();
            await emailTemplateService.Init();
        }


        public static void AddAuth(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication(builder =>
            {
                builder.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                builder.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;


            }).AddJwtBearer(builder =>
            {
                var tokenConfiguration = TokenHelper.Configuration(configuration);

                builder.TokenValidationParameters = new TokenValidationParameters
                {

                    ValidateIssuer = true,
                    ValidIssuer = tokenConfiguration.Issuer,
                    ValidateAudience = true,
                    ValidAudience = tokenConfiguration.Audience,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = tokenConfiguration.SecurityKey,
                    ClockSkew = TimeSpan.Zero
                };

                builder.Events = new JwtBearerEvents
                {
                    OnChallenge = async context =>
                    {
                        var response = ResponseHelper.Create(ResponseConstants.AUTH_TOKEN_NOT_FOUND);
                        throw new UnauthorizedException(ResponseConstants.AUTH_TOKEN_NOT_FOUND);
                    }
                };
            });
            services.AddAuthorization();

        }

        public static void AddCache(this IServiceCollection services, IConfiguration configuration)
        {
            var redisConnectionString = Environment.GetEnvironmentVariable(EnvironmentConstants.REDIS_CONNECTION_STRING)
                ?? configuration[ConfigurationConstants.REDIS_CONNECTION_STRING];

            if (!string.IsNullOrEmpty(redisConnectionString))
            {
                services.AddStackExchangeRedisCache(options =>
                {
                    options.Configuration = redisConnectionString;
                });
                services.AddSingleton(ConnectionMultiplexer.Connect(redisConnectionString));
            }
            else
            {
                services.AddDistributedMemoryCache();
            }
        }

        public static void AddCloudinary(this IServiceCollection services, IConfiguration configuration)
        {
            var cloudName = Environment.GetEnvironmentVariable(EnvironmentConstants.CLOUDINARY_CLOUD_NAME)
                ?? configuration[ConfigurationConstants.CLOUDINARY_CLOUD_NAME]
                ?? throw new Exception(ResponseConstants.ConfigurationPropertyNotFound(ConfigurationConstants.CLOUDINARY_CLOUD_NAME));

            var apiKey = Environment.GetEnvironmentVariable(EnvironmentConstants.CLOUDINARY_API_KEY)
                ?? configuration[ConfigurationConstants.CLOUDINARY_API_KEY]
                ?? throw new Exception(ResponseConstants.ConfigurationPropertyNotFound(ConfigurationConstants.CLOUDINARY_API_KEY));

            var apiSecret = Environment.GetEnvironmentVariable(EnvironmentConstants.CLOUDINARY_API_SECRET)
                ?? configuration[ConfigurationConstants.CLOUDINARY_API_SECRET]
                ?? throw new Exception(ResponseConstants.ConfigurationPropertyNotFound(ConfigurationConstants.CLOUDINARY_API_SECRET));

            var account = new Account(cloudName, apiKey, apiSecret);
            var cloudinary = new Cloudinary(account);
            services.AddSingleton(cloudinary);
            services.AddScoped<ICloudinaryService, CloudinaryService>();
        }
    }
}