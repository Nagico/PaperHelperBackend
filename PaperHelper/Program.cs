using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Microsoft.VisualBasic.CompilerServices;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using PaperHelper.Entities;
using PaperHelper.Exceptions;
using PaperHelper.Extensions;
using PaperHelper.Utils;

var builder = WebApplication.CreateBuilder(args);

// Database
var connectionString = builder.Configuration.GetConnectionString("PaperHelperDatabase");
builder.Services.AddDbContext<PaperHelperContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetConnectionString("PaperHelperCache");
});

//跨域设置
builder.Services.AddCors(option =>
    option.AddPolicy("cors", policy =>
        policy.AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials()
            .WithOrigins(
                "http://localhost:8080",
                "http://localhost:3000", 
                "http://127.0.0.1:8080",
                "http://127.0.0.1:3000",
                "https://paperhelper.nagico.cn"
            )
        )
    );


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

// Json serializer settings
JsonConvert.DefaultSettings = () => new JsonSerializerSettings
{
    Formatting = Formatting.None,
    ContractResolver = new DefaultContractResolver
    {
        NamingStrategy = new SnakeCaseNamingStrategy
        {
            ProcessDictionaryKeys = true
        }
    }
};

// Add services to the container.
builder.Services.AddControllers()
    .AddNewtonsoftJson(options =>
    {
        options.SerializerSettings.Formatting = Formatting.None;
        // 设置下划线方式，首字母是小写
        options.SerializerSettings.ContractResolver = new DefaultContractResolver
        {
            NamingStrategy = new SnakeCaseNamingStrategy
            {
                ProcessDictionaryKeys = true
            }
        };
    });

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

// 获取xml文件
var basePath = AppContext.BaseDirectory;
var d = new DirectoryInfo(basePath);
var files = d.GetFiles("*.xml");
var xmlCommentsFilePath = files.Select(a => Path.Combine(basePath, a.FullName)).ToList();
    
// swagger xml配置
builder.Services.AddSwaggerGen(c =>
{
    foreach (var item in xmlCommentsFilePath)
    {
        c.IncludeXmlComments(item);
    }
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();


    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint($"/swagger/v1/swagger.json?{CodeUtil.GenRandomCode(6)}", "My API V1");
        c.RoutePrefix = "swagger";
    });
}

app.UseCors("cors");

app.UseApiExceptionHandler();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers()
    .RequireCors("cors");

app.Run();