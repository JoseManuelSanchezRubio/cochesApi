using Microsoft.EntityFrameworkCore;
using cochesApi.Logic.Models;
using DataAccess.Data;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Swashbuckle.AspNetCore.Filters;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using cochesApi.Logic.Interfaces;
using cochesApi.Logic.Validations;
using cochesApi.DataAccess.Queries;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddControllers().AddJsonOptions(x =>
                x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);


builder.Services.AddDbContext<myAppContext>(opt =>
    opt.UseLazyLoadingProxies()
    .UseSqlite(builder.Configuration.GetConnectionString("cochesApi")?? throw new InvalidOperationException("Connection string 'cochesApi' not found.")));



// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>{
    options.AddSecurityDefinition("oauth2", new Microsoft.OpenApi.Models.OpenApiSecurityScheme{
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey
    });
    options.OperationFilter<SecurityRequirementsOperationFilter>();
});

/* builder.Services.AddAuthentication().AddJwtBearer(); */
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>{
    options.TokenValidationParameters=new Microsoft.IdentityModel.Tokens.TokenValidationParameters{
        ValidateIssuer=true,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer="http://localhost:5218/",
        ValidAudience="http://localhost:5218/",
        IssuerSigningKey=new SymmetricSecurityKey(Encoding.UTF8.GetBytes("SecretPassword.1234"))
    };
    builder.Configuration.Bind("JwtBearer", options);
});


//Dependencias
builder.Services.AddScoped<IBranch, BranchValidation>();
builder.Services.AddScoped<ICar, CarValidation>();
builder.Services.AddScoped<ICustomer, CustomerValidation>();
builder.Services.AddScoped<IReservation, ReservationValidation>();
builder.Services.AddScoped<ITypeCar, TypeCarValidation>();

builder.Services.AddScoped<IBranchQueries, QueriesBranch>();
builder.Services.AddScoped<ICarQueries, QueriesCar>();
builder.Services.AddScoped<ICustomerQueries, QueriesCustomer>();
builder.Services.AddScoped<IReservationQueries, QueriesReservation>();
builder.Services.AddScoped<ITypeCarQueries, QueriesTypeCar>();
builder.Services.AddScoped<IDBQueries, QueriesDB>();
builder.Services.AddScoped<IPlanningQueries, QueriesPlanning>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseAuthentication();

app.MapControllers();

app.Run();
