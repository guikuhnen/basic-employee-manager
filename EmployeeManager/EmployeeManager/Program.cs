
using EmployeeManager.Application.Interfaces;
using EmployeeManager.Application.Services;
using EmployeeManager.Data.Context;
using EmployeeManager.Data.Interfaces;
using EmployeeManager.Data.Repositories;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManager.API
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			builder.Services.AddCors(options => options.AddDefaultPolicy(policy =>
			{
				policy.AllowAnyOrigin()
				.AllowAnyMethod()
				.AllowAnyHeader();
			}));

			builder.Services.AddRouting(options => options.LowercaseUrls = true);

			builder.Services.AddControllers();

			builder.Services.AddEndpointsApiExplorer();
			builder.Services.AddSwaggerGen();

			var connection = builder.Configuration["MySQLConnection:MySQLConnectionString"];
			builder.Services.AddDbContext<MySQLContext>(options => options.UseMySql(connection, new MySqlServerVersion(new Version(8, 3, 0))));

			// Dependency Injection
			//builder.Services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
			//// Authentication
			//builder.Services.AddTransient<ITokenService, TokenService>();
			//builder.Services.AddTransient<ILoginService, LoginService>();
			//// Application
			builder.Services.AddTransient<IEmployeeRepository, EmployeeRepository>();
			builder.Services.AddTransient<IPhoneNumberRepository, PhoneNumberRepository>();
			builder.Services.AddScoped<IEmployeeService, EmployeeService>();

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
