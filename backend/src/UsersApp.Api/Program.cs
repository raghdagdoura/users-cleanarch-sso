using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using UsersApp.Application;
using UsersApp.Infrastructure;
using UsersApp.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

// Controllers
builder.Services.AddControllers();

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
{
Title = "UsersApp.Api",
Version = "v1"
});

options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
{
Name = "Authorization",
Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
Scheme = "bearer",
BearerFormat = "JWT",
In = Microsoft.OpenApi.Models.ParameterLocation.Header,
Description = "Entrer: Bearer {votre token JWT}"
});

options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
{
{
new Microsoft.OpenApi.Models.OpenApiSecurityScheme
{
Reference = new Microsoft.OpenApi.Models.OpenApiReference
{
Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
Id = "Bearer"
}
},
Array.Empty<string>()
}
});
});

// Application + Infrastructure
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

// JWT
var key = Encoding.UTF8.GetBytes("SUPER_SECRET_KEY_123456789_SUPER_SECRET_KEY_123456789");

builder.Services.AddAuthentication(options =>
{
options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
options.TokenValidationParameters = new TokenValidationParameters
{
ValidateIssuer = false,
ValidateAudience = false,
ValidateIssuerSigningKey = true,
IssuerSigningKey = new SymmetricSecurityKey(key)
};
});

builder.Services.AddAuthorization();
builder.Services.AddCors(options =>
{
options.AddPolicy("AllowFrontend", policy =>
{
policy
.AllowAnyOrigin()
.AllowAnyHeader()
.AllowAnyMethod();
});
});
var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
db.Database.EnsureCreated();
}

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.UseCors("AllowFrontend");

app.MapControllers();
app.Run();