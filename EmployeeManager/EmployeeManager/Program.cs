
using EmployeeManager.API.Configuration;
using EmployeeManager.Application.Interfaces;
using EmployeeManager.Application.Services;
using EmployeeManager.Data.Context;
using EmployeeManager.Data.Interfaces;
using EmployeeManager.Data.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

namespace EmployeeManager.API
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			var tokenConfigurations = new TokenConfiguration();
			new ConfigureFromConfigurationOptions<TokenConfiguration>(builder.Configuration.GetSection("TokenConfigurations"))
				.Configure(tokenConfigurations);

			// Add services to the container.
			builder.Services.AddSingleton(tokenConfigurations);

			// Authentication
			builder.Services.AddAuthentication(options =>
			{
				options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
				options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
			}).AddJwtBearer(options =>
			{
				options.TokenValidationParameters = new()
				{
					ValidateIssuer = true,
					ValidateAudience = true,
					ValidateLifetime = true,
					ValidateIssuerSigningKey = true,
					ClockSkew = TimeSpan.Zero,
					ValidIssuer = tokenConfigurations.Issuer,
					ValidAudience = tokenConfigurations.Audience,
					IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenConfigurations.Secret))
				};
			});
			builder.Services.AddAuthorizationBuilder().AddPolicy("Bearer",
				new AuthorizationPolicyBuilder().AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
					.RequireAuthenticatedUser()
					.Build()
			);

			builder.Services.AddCors(options => options.AddDefaultPolicy(policy =>
			{
				policy.AllowAnyOrigin()
				.AllowAnyMethod()
				.AllowAnyHeader();
			}));

			builder.Services.AddRouting(options => options.LowercaseUrls = true);
			builder.Services.AddControllers();

			var connection = builder.Configuration["MySQLConnection:MySQLConnectionString"];
			builder.Services.AddDbContext<MySQLContext>(options => options.UseMySql(connection, new MySqlServerVersion(new Version(8, 3, 0))));

			// Authentication
			builder.Services.AddTransient<ITokenService, TokenService>();
			builder.Services.AddTransient<ILoginService, LoginService>();
			builder.Services.AddTransient<IUserRepository, UserRepository>();
			// Application
			builder.Services.AddTransient<IEmployeeRepository, EmployeeRepository>();
			builder.Services.AddTransient<IPhoneNumberRepository, PhoneNumberRepository>();
			builder.Services.AddScoped<IEmployeeService, EmployeeService>();

			builder.Services.AddEndpointsApiExplorer();
			builder.Services.AddSwaggerGen(options =>
			{
				options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
				{
					Name = "Authorization",
					Type = SecuritySchemeType.ApiKey,
					Scheme = "Bearer",
					BearerFormat = "JWT",
					In = ParameterLocation.Header,
					Description = "JWT Authorization header using the Bearer scheme." +
						"\r\n\r\n Enter 'Bearer'[space] and then your token in the text input below." +
						"\r\n\r\nExample: \"Bearer 12345abcdef\"",
				});
				options.AddSecurityRequirement(new OpenApiSecurityRequirement
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
						 Array.Empty<string>()
					}
				});
			});

			var app = builder.Build();

			if (app.Environment.IsDevelopment())
			{
				app.UseSwagger();
				app.UseSwaggerUI();
			}

			app.UseHttpsRedirection();

			app.UseCors();

			app.UseAuthorization();

			app.MapControllers();

			app.Run();
		}
	}
}
