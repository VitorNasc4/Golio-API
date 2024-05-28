using System;
using System.Text;
using Golio.API.Filters;
using Golio.Application.Validators;
using Golio.Core.Repositories;
using Golio.Core.Services;
using Golio.Infrastructure.AuthServices;
using Golio.Infrastructure.Persistence;
using Golio.Infrastructure.Persistence.Repositories;
using FluentValidation;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Golio.Application.Commands.UserCommands.CreateUser;
using Golio.Infrastructure.MessageBusService;
using Golio.Messaging.Consumers;
using Azure.Identity;
using Golio.Infrastructure.CacheService;

var builder = WebApplication.CreateBuilder(args);

var appConfigString = Environment.GetEnvironmentVariable("APP_CONFIG");
if (string.IsNullOrEmpty(appConfigString))
{
    appConfigString = builder.Configuration["ConnectionStrings:AppConfig"];
}
builder.Configuration.AddAzureAppConfiguration(options =>
{
    options.Connect(appConfigString)
        .ConfigureKeyVault(kv => kv.SetCredential(new DefaultAzureCredential()));
});

// Para usar o Postgres
var connectionString = builder.Configuration.GetConnectionString("GolioCsPostgres");
builder.Services.AddDbContext<GolioDbContext>
    (option => option.UseNpgsql(connectionString));

// Para usar o Redis
builder.Services.AddSingleton<ICacheService, CacheService>();
builder.Services.AddStackExchangeRedisCache(o =>
{
    o.InstanceName = "Golio";
    o.Configuration = "localhost:6379";
});

// Injeções de dependências
builder.Services.AddMediatR(typeof(CreateUserCommand));
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IPriceRepository, PriceRepository>();
builder.Services.AddScoped<ISuggestionRepository, SuggestionRepository>();
builder.Services.AddScoped<IStoreRepository, StoreRepository>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IMessageBusService, MessageBusService>();

// Ajustando HttpClient
builder.Services.AddHttpClient();

// Ajustando HttpContentAccessor
builder.Services.AddHttpContextAccessor();

builder.Services.AddHostedService<SuggestionQueueConsumer>();
builder.Services.AddHostedService<VoteQueueConsumer>();
builder.Services.AddControllers(options => options.Filters.Add(typeof(ValidationFilters)));

// Fluent Validator
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddFluentValidationClientsideAdapters();
builder.Services.AddValidatorsFromAssemblyContaining<CreateUserCommandValidator>();

// Autenticação JWT
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,

            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        };
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Golio.API", Version = "v1" });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authorization header usando o esquema Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

var app = builder.Build();


app.UseSwagger();
app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Golio.API v1"));

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
