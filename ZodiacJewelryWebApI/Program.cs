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


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
//var configuration = builder.Configuration.Get<AppConfiguration>();
//?? new AppConfiguration();
//builder.Services.Configure<EmailAdmin>(builder.Configuration.GetSection("EmailSettings"));
//builder.Services.AddTransient<SendMail>();
builder.Configuration.AddJsonFile("appsettings.Development.json", optional: false, reloadOnChange: true);
var configuration = builder.Configuration;
var myConfig = new AppConfiguration();
configuration.Bind(myConfig);
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("DatabaseConnection"))); // Use connection string directly


builder.Services.AddSingleton(myConfig);
builder.Services.AddInfrastructuresService();
builder.Services.AddWebAPIService();
builder.Services.AddAutoMapper(typeof(MapperConfigurationsProfile));
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.WithOrigins("*")
               .AllowAnyHeader()
               .AllowAnyMethod();
    });
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

            ValidIssuer = myConfig.JWTSection.Issuer,
            ValidAudience = myConfig.JWTSection.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(myConfig.JWTSection.SecretKey))

            //ValidIssuer = configuration["JWTSection:Issuer"],
            //ValidAudience = configuration["JWTSection:Audience"],
            //IssuerSigningKey = new SymmetricSecurityKey(keyBytes)

        };
    });

builder.Services.AddSwaggerGen(setup =>
{
    // Include 'SecurityScheme' to use JWT Authentication
    var jwtSecurityScheme = new OpenApiSecurityScheme
    {
        BearerFormat = "JWT",
        Name = "JWT Authentication",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = JwtBearerDefaults.AuthenticationScheme,
        Description = "Put *_ONLY_* your JWT Bearer token on textbox below!",

        Reference = new OpenApiReference
        {
            Id = JwtBearerDefaults.AuthenticationScheme,
            Type = ReferenceType.SecurityScheme
        }
    };

    setup.AddSecurityDefinition(jwtSecurityScheme.Reference.Id, jwtSecurityScheme);

    setup.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        { jwtSecurityScheme, Array.Empty<string>() }
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("swagger/v1/swagger.json", "ZodiacJewelryWebApI v1");
        c.RoutePrefix = string.Empty;
    });

}
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("swagger/v1/swagger.json", "ZodiacJewelryWebApI v1");
    c.RoutePrefix = string.Empty;
});
//app.UseCors("Allow");
app.UseHttpsRedirection();
app.UseCors();
app.UseAuthorization();
app.UseMiddleware<ConfirmationTokenMiddleware>();
app.MapControllers();

app.Run();