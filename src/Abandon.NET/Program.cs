using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc.Authorization;
using System.Reflection;
using System.Text;
using System.Text.Json;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using CorrelationId;
using CorrelationId.DependencyInjection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Panda.DynamicWebApi;
using Abandon.NET;
using Abandon.NET.Utility.Middleware;
using Abandon.NET.Utility.Authorization;
using Abandon.NET.Utility.Options;
using Microsoft.FeatureManagement;
using Swashbuckle.AspNetCore.Filters;
using Swashbuckle.AspNetCore.SwaggerGen;

var builder = WebApplication.CreateBuilder(args);

/******************************************Add XXX***************************************************/
//Autofac
//builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
//builder.Host.ConfigureContainer<ContainerBuilder>(container => { container.RegisterModule(new Abandon.NET.AutofacServiceInjection()); });

//Add Service
builder.Services.AddBizService(builder.Configuration);

//FeatureManagement
builder.Services.AddFeatureManagement();

//MVC
builder.Services.AddControllers().AddJsonOptions(config => { config.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase; });
builder.Services.AddHttpContextAccessor();
builder.Services.AddMvc(option =>
{
    option.EnableEndpointRouting = false;
    option.Filters.Add(new AuthorizeFilter());
});

//DynamicWebApi
builder.Services.AddDynamicWebApi(new DynamicWebApiOptions
{
    DefaultApiPrefix = "api",
    RemoveControllerPostfixes = ["Service"]
});
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

//Swagger
var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
var swaggerDoc = "Abandon.NET";
var version = "1.0.0";
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen((Action<SwaggerGenOptions>)(c =>
{
    c.SwaggerDoc(swaggerDoc, new OpenApiInfo()
    {
        Title = "CBI Mega Terminal API",
        Version = "v" + version
    });
    c.DocInclusionPredicate((Func<string, ApiDescription, bool>)((docName, description) => true));
    c.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme()
    {
        Type = SecuritySchemeType.ApiKey,
        Name = "Authorization",
        In = ParameterLocation.Header
    });
    if (!string.IsNullOrEmpty(xmlPath))
        c.IncludeXmlComments(xmlPath, true);
    c.OperationFilter<SecurityRequirementsOperationFilter>();
    c.OperationFilter<AddResponseHeadersFilter>();
    c.OperationFilter<AppendAuthorizeToSummaryOperationFilter>();
}));

//Configuration vs Option
builder.Services.AddOptions<JwtSetting>().Bind(builder.Configuration.GetSection("Jwt"))
    .ValidateDataAnnotations()
    .ValidateOnStart();

//CorrelationId
builder.Services.AddDefaultCorrelationId(options => { options.UpdateTraceIdentifier = true; });

//MCP
builder.Services
    .AddMcpServer()
    .WithHttpTransport()
    .WithToolsFromAssembly();

var app = builder.Build();

/***************************************Use XXX******************************************************/

app.UseCorrelationId();

app.UseSwagger(c => { c.RouteTemplate = "{documentName}/swagger.json"; });

app.UseSwaggerUI(c =>
{
    c.ShowExtensions();
    c.EnableValidator(null);
    c.SwaggerEndpoint($"/Abandon.NET/swagger.json", "Default");
});

app.UseMiddleware<GlobalExceptionHandler>();
app.UseWebSockets();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.MapMcp();

app.Run();