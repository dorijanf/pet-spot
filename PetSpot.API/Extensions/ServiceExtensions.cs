using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using PetSpot.API.Configuration;
using PetSpot.API.Repositories;
using PetSpot.API.Services;
using PetSpot.DATA;
using PetSpot.DATA.Entities;
using PetSpot.DATA.Models;
using PetSpot.DATA.Validators;
using PetSpot.LOGGING;
using System.Text;

namespace PetSpot.API.Extensions
{
    /// <summary>
    /// Extension class which holds various configuration methods in order to make 
    /// the startup class more readable.
    /// </summary>
    public static class ServiceExtensions
    {
        /// <summary>
        /// Configures Cors which is necessary to connect the API
        /// with an external application.
        /// </summary>
        /// <param name="services"></param>
        public static void ConfigureCors(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader());
            });
        }

        /// <summary>
        /// Configures IIS Integration options.
        /// </summary>
        /// <param name="services"></param>
        public static void ConfigureIISIntegration(this IServiceCollection services)
        {
            services.Configure<IISOptions>(options =>
            {

            });
        }

        /// <summary>
        /// Configures the database context and establishes a connection
        /// with the database.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="Configuration"></param>
        public static void ConfigureDatabaseContext(this IServiceCollection services, IConfiguration Configuration)
        {
            services.AddDbContext<PetSpotDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("PetSpotDb")));
        }

        /// <summary>
        /// Configures swagger which is used to document and test
        /// the API.
        /// </summary>
        /// <param name="services"></param>
        public static void ConfigureSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Pet Spot API",
                    Version = "v1"
                });

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please insert JWT with Bearer into field",
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] { }
                    }
                });
            });
        }

        /// <summary>
        /// Configures Jwt Token settings and handles the creation of JWT Bearer 
        /// Token which is necessary for authorization.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="Configuration"></param>
        public static void ConfigureJwtSettings(this IServiceCollection services, IConfiguration Configuration)
        {
            var jwtSettings = new JwtSettings();
            Configuration.GetSection("JwtSettings").Bind(jwtSettings);
            services.AddSingleton<JwtSettings>(jwtSettings);
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = false,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key)),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                };
            });
        }

        /// <summary>
        /// Configures identity classes.
        /// </summary>
        /// <param name="services"></param>
        public static void ConfigureIdentity(this IServiceCollection services)
        {
            services.AddIdentity<User, UserRole>()
                .AddEntityFrameworkStores<PetSpotDbContext>();
            services.AddTransient<IHttpContextAccessor, HttpContextAccessor>();
            services.AddTransient<UserResolverService>();
        }

        /// <summary>
        /// Configures custom logging services using the Nlog library.
        /// </summary>
        /// <param name="services"></param>
        public static void ConfigureLoggingServices(this IServiceCollection services)
        {
            services.AddSingleton<ILoggerManager, LoggerManager>();
        }

        /// <summary>
        /// Adds controller services. Each controller has its controller service
        /// which hides implementation details and makes the controller more readable.
        /// </summary>
        /// <param name="services"></param>
        public static void AddControllerServices(this IServiceCollection services)
        {
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IAnimalsService, AnimalsService>();
            services.AddScoped<ISyncService, SyncService>();
        }

        /// <summary>
        /// Adds repositories which hide implementation details for certain 
        /// objects (location) from animal service.
        /// </summary>
        /// <param name="services"></param>
        public static void AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<ILocationRepository, LocationRepository>();
        }

        /// <summary>
        /// Adds validators for various binding model classes.
        /// </summary>
        /// <param name="services"></param>
        public static void AddValidationServices(this IServiceCollection services)
        {
            services.AddTransient<IValidator<AuthorizeBm>, AuthorizeBmValidator>();
            services.AddTransient<IValidator<RegisterUserBm>, RegisterUserBmValidator>();
            services.AddTransient<IValidator<AnimalBm>, AnimalBmValidator>();
        }
    }
}
