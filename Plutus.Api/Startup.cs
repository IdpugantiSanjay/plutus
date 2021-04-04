using System;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Plutus.Application.Repositories;
using Plutus.Infrastructure;
using Plutus.Infrastructure.Repositories;
using Serilog;


namespace Plutus.Api
{
    public class Startup
    {

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo {Title = "Plutus.Api", Version = "v1"});
            });

            services.AddMediatR(typeof(ITransactionRepository), typeof(Startup));
            
            services.AddDbContext<AppDbContext>(options =>
            {
                var connectionString = BuildConnectionString();
                options.UseNpgsql(connectionString);
            });

            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<ITransactionRepository, TransactionRepository>();
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

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }

        private static string BuildConnectionString()
        {
            var connectionString = $"Host={Environment.GetEnvironmentVariable("PGHOST")};Database={Environment.GetEnvironmentVariable("PGDATABASE")};Username={Environment.GetEnvironmentVariable("PGUSER")};Password={Environment.GetEnvironmentVariable("PGPASSWORD")}";
            Log.Information($"The formed connection string is {connectionString}");
            return connectionString;
        }
    }
}