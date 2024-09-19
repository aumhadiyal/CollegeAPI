using CollegeApp.Data;
using CollegeApp.Data.Repository;
using CollegeApp.Properties.Configurations;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.Text;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

Log.Logger = new LoggerConfiguration().MinimumLevel.Information().WriteTo.File("Log/log.txt", rollingInterval: RollingInterval.Day).CreateLogger();

builder.Logging.AddSerilog();

builder.Services.AddCors(o => o.AddPolicy("MyTestCors",
    policy =>
    { //Allows every origin with any header and method
        policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
    }));

var key = Encoding.ASCII.GetBytes(builder.Configuration.GetValue<string>("JwtSecret"));
builder.Services.AddAuthentication(o =>
{
    o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(o =>
{
    o.RequireHttpsMetadata = false;
    o.SaveToken = true;
    o.TokenValidationParameters = new()
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateAudience = false,
        ValidateIssuer = false
    };
}); ;


builder.Services.AddControllers().AddNewtonsoftJson();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<CollegeDBContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("Default")));
builder.Services.AddAutoMapper(typeof(AutoMapperConfig));
builder.Services.AddScoped<IStudentRepository, StudentRepository>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("MyTestCors");
app.UseAuthorization();

app.MapControllers();

app.Run();
