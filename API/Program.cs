using AquariumManagementAPI;
using DAL;
using DAL.UnitOfWork;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using NSwag;
using NSwag.Generation.Processors.Security;
using Serilog;
using Services;
using Services.Authentication;
using System.Text.Json.Serialization;
using Utils;

//var builder = WebApplication.CreateBuilder(args);

//      "launchUrl": "{Scheme}://{ServiceHost}:{ServicePort}/swagger",
var builder = WebApplication.CreateBuilder(new WebApplicationOptions
{
    ApplicationName = typeof(Program).Assembly.FullName,
    ContentRootPath = Constants.CurrentFolder + @"/Settings",
    //ContentRootPath = DataSimulator.Utility.Constants.CurrentFolder
});

Serilog.ILogger log = Logger.ContextLog<Program>();

builder.Host.UseSerilog(log);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<GlobalService>();
builder.Services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();


builder.Services.AddControllersWithViews().AddNewtonsoftJson(options =>
{
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
    options.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
    options.SerializerSettings.DefaultValueHandling = DefaultValueHandling.Include;

}
).AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    options.JsonSerializerOptions.IgnoreNullValues = true;
});


builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{

    options.TokenValidationParameters = Authentication.ValidationParams;
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy",
        builder => builder.WithOrigins("*").AllowAnyMethod().AllowAnyHeader());
});

builder.Services.AddControllers().AddNewtonsoftJson(opts => opts.SerializerSettings.Converters.Add(new StringEnumConverter()));
//builder.Services.AddControllers().AddOData(opt => opt.AddRouteComponents("api/odata", GetEdmModel()).Filter().Count().Select().Expand().OrderBy().SetMaxTop(null));
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
//builder.Services.AddEndpointsApiExplorer();

builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});

/*
builder.Services.AddSwaggerGen(c =>
{
}
); ;
*/

builder.Services.AddOpenApiDocument(document =>
{

    document.Title = "Aquarium API";
    document.Description = "API for Aquarium Management";

    document.AddSecurity("JWT", Enumerable.Empty<string>(), new NSwag.OpenApiSecurityScheme
    {
        Type = OpenApiSecuritySchemeType.ApiKey,
        Name = "Authorization",
        In = OpenApiSecurityApiKeyLocation.Header,
        Description = "Type into the textbox: Bearer {your JWT token}."

    });


    document.OperationProcessors.Add(
        new AspNetCoreOperationSecurityScopeProcessor("JWT"));


});


builder.WebHost.UseKestrel(options =>
{
    options.Listen(System.Net.IPAddress.Any, 5000, listenOptions =>
    {
        try
        {
            log.Debug("Starting on Port 5000");
        }
        catch (Exception ex)
        {
            log.Fatal(ex, "Could not start Webserver on 5000", ex);
        }
    });
});



builder.Services.AddSwaggerGenNewtonsoftSupport();
//builder.Services.AddSwaggerDocument(settings =>
//{
//    settings.Title = "Bike Station API v1";
//});

builder.Services.AddHealthChecks();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    //app.UseSwagger();
    //app.UseSwaggerUI();
}

app.MapHealthChecks("/health");
app.UseOpenApi();
app.UseSwaggerUi3();

app.UseCors("CorsPolicy");
//app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();



app.Run();

