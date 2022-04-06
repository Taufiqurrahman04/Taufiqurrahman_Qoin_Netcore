
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.ObjectPool;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Configuration;
using Taufiqurrahman_Test_Qoin.EntityFramework;
using Taufiqurrahman_Test_Qoin.EntityFramework.Models;
using Taufiqurrahman_Test_Qoin.EntityFramework.Repositories;
//using Taufiqurrahman_Test_Qoin.RabbitMqPoolObject;

var builder = WebApplication.CreateBuilder(args);

var setting = builder.Configuration.GetSection("DbItems").Get<DbItems>();
var rabbitConfig = builder.Configuration.GetSection("rabbit");
builder.Services.Configure<RabbitMq>(rabbitConfig);
// Add services to the container.
builder.Services.AddCors(opt =>
{
    opt.AddPolicy("ApiCors", builder => builder.AllowAnyOrigin());
});
builder.Services.AddSingleton<ObjectPoolProvider, DefaultObjectPoolProvider>();
builder.Services.AddSingleton<IRabbitMqs, RabbitMqs>();
builder.Services.AddDbContextPool<AppDbContext>(options =>
options.UseMySql(
    setting.ConnString,
    ServerVersion.AutoDetect(setting.ConnString)
    ));
builder.Services.AddTransient<ITest01s, Test01s>();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("ApiCors");
app.UseAuthorization();

app.MapControllers();

app.Run();
