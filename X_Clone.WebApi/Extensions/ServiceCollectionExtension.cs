using XClone.Application.Interfaces.Services;
using XClone.Application.Services;
using XClone.Domain.Database.SqlServer.Context;
using XClone.Domain.Interfaces.Repositories;
using XClone.Infrastructure.Persistence.SqlServer.Repositories;
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

        }

        /// <summary>
		/// Método que añade lo esencial que necesita nuestra aplicación para funcionar
		/// </summary>
		/// <param name="services"></param>
        public static void AddCore(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddControllers();
            services.AddOpenApi();

            services.AddSqlServer<XcloneContext>(configuration.GetConnectionString("Database")); AddRepositories(services);

            services.AddRepositories();

            services.AddServices();

            services.AddMiddlleWares();

        }

        /// <summary>
		/// Método que añade los middlewares de la aplicación
		/// </summary>
		/// <param name="services"></param>
        public static void AddMiddlleWares(this IServiceCollection services)
        {
            services.AddScoped<ErrorHandlerMiddleware>();
        }
    }
}
