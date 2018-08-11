using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ChatRoom.DAL;
using Microsoft.EntityFrameworkCore;
using ChatRoom.Core.Abstractions;
using ChatRoom.Core.Abstractions.RepositoryInterfaces;
using ChatRoom.DAL.Repositories;
using ChatRoom.Core.Abstractions.OperationInterfaces;
using ChatRoom.BLL.Operations;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Reflection;
using System.IO;
using Swashbuckle.AspNetCore.Swagger;
using ChatRoom.API.ErrorHandling;
using ChatRoom.Core.Models;
using ChatRoom.API;
using ChatRoom.API.Middleware;

namespace ChatRoom
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors();

            services.AddDbContextPool<ChatRoomContext>(options =>
            {
                options.UseSqlServer(Configuration["ConnectionString"], m => m.MigrationsAssembly("ChatRoom.API"));
                options.EnableSensitiveDataLogging(true);
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "Chat Room API" });
            });

            services.AddTransient<IRepositoryManager, RepositoryManager>();
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<IUserChatRoomRepository, UserChatRoomRepository>();
            services.AddTransient<IMessageRepository, MessageRepository>();
            services.AddTransient<IUserOperations, UserOperations>();
            services.AddTransient<IUserChatRoomOperations, UserChatRoomOperations>();

            services.AddMvc();
            var appSettingsSection = Configuration.GetSection("TokenAuthentification");
            services.Configure<TokenAuthentification>(appSettingsSection);

            var appSettings = appSettingsSection.Get<TokenAuthentification>();
            var key = Encoding.ASCII.GetBytes(appSettings.SecretKey);
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.Events = new JwtBearerEvents
                {
                    OnTokenValidated = context =>
                    {
                        var userService = context.HttpContext.RequestServices.GetRequiredService<IUserOperations>();
                        var userId = int.Parse(context.Principal.Identity.Name);
                        var user = userService.GetById(userId);
                        if (user == null)
                        {
                            context.Fail("Unauthorized");
                        }
                        return Task.CompletedTask;
                    }
                };
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });

            services.AddScoped<IUserOperations, UserOperations>();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseMiddleware<ErrorHandlingMiddleware>();

            app.UseCors(x => x
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials());

            app.UseAuthentication();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint($"/swagger/v1/swagger.json", "My API V1");
            });
            app.SeedData();
            app.UseMvc();
            app.UseWebSockets();
            app.UseMiddleware<WebSocketMiddleware>();
        }
    }
}
