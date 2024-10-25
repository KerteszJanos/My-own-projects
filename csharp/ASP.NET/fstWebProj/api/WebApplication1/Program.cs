using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace WebApplication1
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			// CORS policy beállítása
			builder.Services.AddCors(options =>
			{
				options.AddPolicy("AllowOrigin",
					builder => builder.AllowAnyOrigin()  // Változtasd meg ezt a specific origin-ra, ha szükséges
									  .AllowAnyMethod()
									  .AllowAnyHeader());
			});

			// Szolgáltatások hozzáadása
			builder.Services.AddControllers();
			builder.Services.AddEndpointsApiExplorer();
			builder.Services.AddSwaggerGen();

			var app = builder.Build();

			// Middleware konfigurálása
			app.UseCors("AllowOrigin"); // CORS policy alkalmazása

			if (app.Environment.IsDevelopment())
			{
				app.UseSwagger();
				app.UseSwaggerUI();
			}

			app.UseAuthorization();

			app.MapControllers();

			app.Run();
		}
	}
}
