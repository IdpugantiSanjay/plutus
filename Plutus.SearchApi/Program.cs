using Microsoft.OpenApi.Models;
using Nest;
using Plutus.Application.Transactions.Indexes;
using Plutus.ElasticSearch.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "Plutus.SearchApi", Version = "v1" });
});


string ELASTIC_HOST = Environment.GetEnvironmentVariable("ELASTIC_HOST")!;
builder.Services.AddElasticSearch(new ConnectionSettings(new Uri(ELASTIC_HOST)));
builder.Services.AddSingleton<TransactionIndex>();

var app = builder.Build();


// Configure the HTTP request pipeline.
if (builder.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Plutus.SearchApi v1"));
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
