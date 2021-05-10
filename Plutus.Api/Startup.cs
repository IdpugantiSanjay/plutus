using System;
using System.Text;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Identity.Web;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Plutus.Api.Middleware;
using Plutus.Application.MappingProfiles;
using Plutus.Application.Repositories;
using Plutus.Application.Transactions.Commands;
using Plutus.Infrastructure;
using Plutus.Infrastructure.Repositories;
using Serilog;


namespace Plutus.Api
{
    public class Startup
    {
        private const string OriginPolicy = "All";

        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        protected virtual void AddAuthentication(IServiceCollection services)
        {
            // services.AddMicrosoftIdentityWebApiAuthentication(Configuration);

            services.AddAuthentication(o =>
                {
                    o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(o =>
                {
                    o.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true, ValidateAudience = true, ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = Configuration["Jwt:Issuer"],
                        ValidAudience = Configuration["Jwt:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Secret"]))
                    };
                })
                ;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(o =>
            {
                o.AddPolicy(name: OriginPolicy,
                    builder => { builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader(); });
            });


            AddAuthentication(services);

            services.Configure<OpenIdConnectOptions>(options => { options.Authority += "/v2.0"; });

            services.AddControllers().AddFluentValidation(config =>
                config.RegisterValidatorsFromAssembly(typeof(CreateTransaction).Assembly));

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo {Title = "Plutus.Api", Version = "v1"});
            });

            services.AddMediatR(typeof(ITransactionRepository), typeof(Startup));

            services.AddAutoMapper(c => { c.AddProfile(new Profile()); });

            services.AddHealthChecks();

            // "Authority": "https://login.microsoftonline.com/7a91cc23-8998-4d47-b09b-b257e1787512/",

            services.AddDbContext<AppDbContext>(options =>
            {
                var connectionString = BuildConnectionString();
                // Log.Information($"The formed connection string is {connectionString}");
                options.UseNpgsql(connectionString);
            });

            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<ITransactionRepository, TransactionRepository>();
            services.AddTransient<ICategoryRepository, CategoryRepository>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Plutus.Api v1"));
            }

            app.UseHttpsRedirection();
            app.UseRouting();


            app.UseCors(OriginPolicy);

            app.UseAuthentication();
            app.UseAuthorization();


            app.UseMiddleware<ErrorHandlerMiddleware>();
            app.UseMiddleware<AddUserToRouteMiddleware>();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHealthChecks("/health");
            });
        }

        private static string BuildConnectionString()
        {
            var connectionString =
                $"Host={Environment.GetEnvironmentVariable("PGHOST")};Database={Environment.GetEnvironmentVariable("PGDATABASE")};Username={Environment.GetEnvironmentVariable("PGUSER")};Password={Environment.GetEnvironmentVariable("PGPASSWORD")};Include Error Detail=true";
            return connectionString;
        }
    }
}