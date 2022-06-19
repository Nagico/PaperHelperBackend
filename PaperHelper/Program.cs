using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using PaperHelper.Entities;
using PaperHelper.Exceptions;
using PaperHelper.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Database
var connectionString = builder.Configuration.GetConnectionString("PaperHelperDatabase");
builder.Services.AddDbContext<PaperHelperContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

// Add JWT authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.FromSeconds(Convert.ToInt32(builder.Configuration.GetSection("JWT")["ClockSkew"])),
            ValidateIssuerSigningKey = true,
            ValidAudience = builder.Configuration.GetSection("JWT")["ValidAudience"],
            ValidIssuer = builder.Configuration.GetSection("JWT")["ValidIssuer"],
            IssuerSigningKey =
                new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(builder.Configuration.GetSection("JWT")["IssuerSigningKey"]))
        };
        options.Events = new JwtBearerEvents
        {
            //权限验证失败后执行
            OnChallenge = context =>
            {
                //终止默认的返回结果(必须有)
                context.HandleResponse();
                throw new AppError("A0310", "请登录后重试");
            }
        };
    });

builder.Services.AddAuthorization();

// Add services to the container.
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    var scheme = new OpenApiSecurityScheme()
    {
        Description = "Authorization header. \r\nExample: 'Bearer 12345abcdef'",
        Reference = new OpenApiReference
        {
            Type = ReferenceType.SecurityScheme,
            Id = "Authorization"
        },
        Scheme = "oauth2",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
    };
    c.AddSecurityDefinition("Authorization", scheme);
    var requirement = new OpenApiSecurityRequirement
    {
        [scheme] = new List<string>()
    };
    c.AddSecurityRequirement(requirement);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseApiExceptionHandler();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();