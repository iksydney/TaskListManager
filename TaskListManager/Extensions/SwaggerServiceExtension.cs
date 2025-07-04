using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace TaskListManager.API.Extensions
{
    public static class SwaggerServiceExtension
    {
        public static IServiceCollection SwaggerExtensions(this IServiceCollection services)
        {
            services.AddSwaggerGen(
                opt =>
                {
                    opt.SwaggerDoc("v1", new OpenApiInfo
                    {
                        Title = "Task Management API",
                        Version = "v1",
                        Description = "Endpoints that handles all the implementations of the Task management Service"
                    });
                    opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                    {
                        In = ParameterLocation.Header,
                        Description = "Please enter a valid token",
                        Name = "Authorization",
                        Type = SecuritySchemeType.Http,
                        BearerFormat = "JWT",
                        Scheme = "Bearer"
                    });
                    opt.AddSecurityRequirement(new OpenApiSecurityRequirement
                    {
                        {
                            new OpenApiSecurityScheme
                            {Reference = new OpenApiReference
                            {
                                Type= ReferenceType.SecurityScheme,
                                Id="Bearer"
                            }
                            }, new string[]{ }
                        }
                    });
                });
            return services;
        }

        public static IApplicationBuilder SwaggerDocumentation(this IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "API V1");
            });
            return app;
        }
        public static IServiceCollection AddJwtSecurityKey(this IServiceCollection services, IConfiguration configuration)
        {
            var jwtSecret = configuration["JWT:Secret"];
            if (string.IsNullOrEmpty(jwtSecret))
            {
                throw new InvalidOperationException("JWT Secret is not configured.");
            }

            var symmetricKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(jwtSecret));
            services.AddSingleton(symmetricKey);

            return services;
        }
    }
}
