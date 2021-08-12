using System;
using System.Linq;
using System.Security.Authentication;
using System.Text;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Nest;
using Plutus.Api.Middleware;
using Plutus.Application.Repositories;
using Plutus.Application.Transactions.Commands;
using Plutus.Application.Transactions.Indexes;
using Plutus.ElasticSearch.Infrastructure;
using Plutus.Infrastructure;
using Plutus.Infrastructure.Repositories;
using Serilog;
using Profile = Plutus.Application.MappingProfiles.Profile;


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
            services.AddElasticSearch(new ConnectionSettings(new Uri(Configuration["Elastic:ServerUrl"])));
            
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
                c.CustomSchemaIds(t => t.ToString());
                c.SwaggerDoc("v1", new OpenApiInfo {Title = "Plutus.Api", Version = "v1"});
            });

            services.AddMediatR(typeof(ITransactionRepository), typeof(Startup));

            services.AddAutoMapper(c => { c.AddProfile(new Profile()); });

            services.AddHealthChecks();

            // "Authority": "https://login.microsoftonline.com/7a91cc23-8998-4d47-b09b-b257e1787512/",

            services.AddDbContext<AppDbContext>(options =>
            {
                var connectionString = BuildConnectionString(Configuration);
                // Log.Information($"The formed connection string is {connectionString}");
                options.UseNpgsql(connectionString);
            });

            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<ITransactionRepository, TransactionRepository>();
            services.AddTransient<ICategoryRepository, CategoryRepository>();
            services.AddSingleton<TransactionIndex>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseSerilogRequestLogging();

            app.UseMiddleware<RequestLogger>();
            
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "Plutus.Api v1"); });
            }

            app.UseHttpsRedirection();
            app.UseRouting();


            app.UseCors(OriginPolicy);

            app.UseAuthentication();
            app.UseAuthorization();


            app.UseMiddleware<ErrorHandlerMiddleware>();
            if (env.IsEnvironment("Testing") is false)
                app.UseMiddleware<CheckIfUsernameInTokenIsSameAsRequestUsername>();


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHealthChecks("/health");
            });
        }

        private static string BuildConnectionString(IConfiguration configuration)
        {
            var host = Environment.GetEnvironmentVariable("PGHOST");
            var database = Environment.GetEnvironmentVariable("PGDATABASE");
            var user = Environment.GetEnvironmentVariable("PGUSER");
            var password = Environment.GetEnvironmentVariable("PGPASSWORD");

            var credentialsInvalid = new[] {host, database, user, password}.Any(string.IsNullOrEmpty);

            return credentialsInvalid switch
            {
                false =>
                    $"Host={host};Database={database};Username={user};Password={password};Include Error Detail=true",
                _ when configuration.GetConnectionString("Database") is not null => configuration.GetConnectionString(
                    "Database"),
                _ => throw new InvalidCredentialException("Invalid Postgres Credentials")
            };
        }
    }
}