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

			// CORS policy be�ll�t�sa
			builder.Services.AddCors(options =>
			{
				options.AddPolicy("AllowOrigin",
					builder => builder.AllowAnyOrigin()  // V�ltoztasd meg ezt a specific origin-ra, ha sz�ks�ges
									  .AllowAnyMethod()
									  .AllowAnyHeader());
			});

			// Szolg�ltat�sok hozz�ad�sa
			builder.Services.AddControllers();
			builder.Services.AddEndpointsApiExplorer();
			builder.Services.AddSwaggerGen();

			var app = builder.Build();

			// Middleware konfigur�l�sa
			app.UseCors("AllowOrigin"); // CORS policy alkalmaz�sa

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
