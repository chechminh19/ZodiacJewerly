using System.Reflection;
using Application.Commons;
using Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Infrastructure.Mappers;
using Microsoft.OpenApi.Models;
using ZodiacJewelryWebApI;
using ZodiacJewelryWebApI.Middlewares;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Application.ViewModels.Cloud;
using CloudinaryDotNet;
using Microsoft.Extensions.Options;
using Net.payOS;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
//var configuration = builder.Configuration.Get<AppConfiguration>();
//?? new AppConfiguration();
//builder.Services.Configure<EmailAdmin>(builder.Configuration.GetSection("EmailSettings"));
//builder.Services.AddTransient<SendMail>();
builder.Configuration.AddJsonFile("appsettings.Development.json", optional: false, reloadOnChange: true);
var configuration = builder.Configuration;


var payOs = new PayOS(configuration["Environment:PAYOS_CLIENT_ID"] ?? throw new Exception("Cannot find environment client"),
                    configuration["Environment:PAYOS_API_KEY"] ?? throw new Exception("Cannot find environment api"),
                    configuration["Environment:PAYOS_CHECKSUM_KEY"] ?? throw new Exception("Cannot find environment sum"));
builder.Services.AddScoped<PayOS>(_ => payOs);
var myConfig = new AppConfiguration();
configuration.Bind(myConfig);
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("DatabaseConnection"))); // Use connection string directly

builder.Services.Configure<CloudinarySettings>(configuration.GetSection("Cloudinary"));
builder.Services.AddSingleton(provider =>
{
    var config = provider.GetRequiredService<IOptions<CloudinarySettings>>().Value;
    return new Cloudinary(new Account(
        config.CloudName,
        config.ApiKey,
        config.ApiSecret));
});
builder.Services.AddSingleton(myConfig);
builder.Services.AddInfrastructuresService();
builder.Services.AddWebAPIService();
builder.Services.AddAutoMapper(typeof(MapperConfigurationsProfile));
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMemoryCache();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
               builder =>
               {
                   builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
               });
});
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Admin", policy => policy.RequireRole("Admin"));
    options.AddPolicy("Staff", policy => policy.RequireRole("Staff"));
    options.AddPolicy("Customer", policy => policy.RequireRole("Customer"));
});
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Session timeout
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
//byte[] keyBytes = Encoding.UTF8.GetBytes(configuration["JWTSection:SecretKey"]);
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        IConfiguration config = (IConfiguration)configuration;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,

            ValidIssuer = configuration["JWTSection:Issuer"],
            ValidAudience = configuration["JWTSection:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["JWTSection:SecretKey"]))

            //ValidIssuer = configuration["JWTSection:Issuer"],
            //ValidAudience = configuration["JWTSection:Audience"],
            //IssuerSigningKey = new SymmetricSecurityKey(keyBytes)

        };
    });

builder.Services.AddSwaggerGen(c =>
{

    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "ZodiacJewerly.API",
    });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme. " +
                            "\n\nEnter your token in the text input below. " +
                              "\n\nExample: '12345abcde'",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "bearer"
    });
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);
    c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
    c.AddSecurityRequirement(new OpenApiSecurityRequirement()
    {
        {
            new OpenApiSecurityScheme{
                Reference = new OpenApiReference{
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("swagger/v1/swagger.json", "ZodiacJewelryWebApI v1");
    c.RoutePrefix = string.Empty;
});//app.UseCors("Allow");
app.UseHttpsRedirection();
app.UseCors("AllowAll");
app.UseAuthentication();
app.UseAuthorization();

app.UseMiddleware<ConfirmationTokenMiddleware>();

app.MapControllers();

app.Run();
