using Microsoft.AspNetCore.Http.Features;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Abandon.NET;
using Abandon.NET.Utility.Middleware;
using Abandon.NET.Utility.Authorization;
using Abandon.NET.Utility.Options;
using Microsoft.FeatureManagement;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

/******************************************Add XXX***************************************************/
//AddOpenApi
builder.Services.AddOpenApi();

//Add Service
builder.Services.AddBizService(builder.Configuration);

//FeatureManagement
builder.Services.AddFeatureManagement();

//AddControllers
builder.Services.AddControllers().AddJsonOptions(config => config.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase);

//AddHttpContextAccessor
builder.Services.AddHttpContextAccessor();

//
builder.Services.Configure<FormOptions>(x =>
{
    x.ValueLengthLimit = int.MaxValue;
    x.MultipartBodyLengthLimit = int.MaxValue;
});

// --- JWT 配置 ---
var jwtIssuer = builder.Configuration["Jwt:Issuer"];
var jwtAudience = builder.Configuration["Jwt:Audience"];
var jwtKey = builder.Configuration["Jwt:Key"];
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    // 配置 JWT 验证参数
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true, // 验证签发者
        ValidateAudience = true, // 验证受众
        ValidateLifetime = true, // 验证令牌有效期
        ValidateIssuerSigningKey = true, // 验证签名密钥

        ValidIssuer = jwtIssuer,
        ValidAudience = jwtAudience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey!))
    };
});

// 授权和认证通过HttpContext.User关联
builder.Services.AddAuthorization(options =>
{
    // 定义一个要求 "Admin" 角色的策略
    options.AddPolicy("RequireAdminRole", policy =>
        policy.RequireRole("Admin"));

    // 定义一个要求特定 Claim 的策略
    options.AddPolicy("MustBeEmployee", policy =>
        policy.RequireClaim("EmployeeId")); // 要求 JWT 中包含 EmployeeId Claim

    // 定义一个自定义的授权要求
    options.AddPolicy("MyRequirement", policy =>
        policy.AddRequirements(new MyRequirement()));
});

//Configuration vs Option
builder.Services.AddOptions<JwtSetting>().Bind(builder.Configuration.GetSection("Jwt"))
    .ValidateDataAnnotations()
    .ValidateOnStart();

//MCP
builder.Services
    .AddMcpServer()
    .WithHttpTransport()
    .WithToolsFromAssembly();

var app = builder.Build();

/***************************************Use XXX******************************************************/

app.UseMiddleware<GlobalExceptionHandler>();

app.MapOpenApi();
app.MapScalarApiReference();
app.UseWebSockets();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.MapMcp();
app.Run();