using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc.Authorization;
using System.Reflection;
using System.Text.Json;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using CorrelationId;
using CorrelationId.DependencyInjection;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.OpenApi.Models;
using Panda.DynamicWebApi;
using Swashbuckle.AspNetCore.Filters;
using Swashbuckle.AspNetCore.SwaggerGen;

var builder = WebApplication.CreateBuilder(args);

/******************************************Add XXX***************************************************/
//Autofac
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
builder.Host.ConfigureContainer<ContainerBuilder>(container => { container.RegisterModule(new SLN.ServiceInjection()); });

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

//Swagger
var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
var swaggerDoc = "SLN";
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

//CorrelationId
builder.Services.AddDefaultCorrelationId(options => { options.UpdateTraceIdentifier = true; });

var app = builder.Build();

/***************************************Use XXX******************************************************/

app.UseCorrelationId();

app.UseSwagger(c => { c.RouteTemplate = "{documentName}/swagger.json"; });

app.UseSwaggerUI(c =>
{
    c.ShowExtensions();
    c.EnableValidator(null);
    c.SwaggerEndpoint($"/SLN/swagger.json", "Default");
});

app.UseWebSockets();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();