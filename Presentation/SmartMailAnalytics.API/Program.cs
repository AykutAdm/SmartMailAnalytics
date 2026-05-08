using Microsoft.AspNetCore.Connections;
using SmartMailAnalytics.Application.Interfaces.MailCategoryInterfaces;
using SmartMailAnalytics.Application.Interfaces.MailInterfaces;
using SmartMailAnalytics.Application.Interfaces.UserInterfaces;
using SmartMailAnalytics.Application.Services.MailCategoryServices;
using SmartMailAnalytics.Application.Services.MailServices;
using SmartMailAnalytics.Application.Services.UserServices;
using SmartMailAnalytics.Infrastructure.Data;
using SmartMailAnalytics.Infrastructure.Repositories.MailCategoryRepositories;
using SmartMailAnalytics.Infrastructure.Repositories.MailRepositories;
using SmartMailAnalytics.Infrastructure.Repositories.UserRepositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// CORS Configuration
builder.Services.AddCors(options =>
{
    options.AddPolicy("SmartMailAnalyticsPolicy", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

builder.Services.AddScoped<DbConnectionFactory>(sp =>
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    return new DbConnectionFactory(connectionString);
});

builder.Services.AddScoped<IMailRepository, MailRepository>();
builder.Services.AddScoped<MailService>();

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<UserService>();

builder.Services.AddScoped<IMailCategoryRepository, MailCategoryRepository>();
builder.Services.AddScoped<MailCategoryService>();

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

// CORS Middleware
app.UseCors("SmartMailAnalyticsPolicy");

app.UseAuthorization();

app.MapControllers();

app.Run();
