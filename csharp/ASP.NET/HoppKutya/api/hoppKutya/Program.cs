using Microsoft.EntityFrameworkCore;
using hoppKutya.Contexts;

namespace hoppKutya
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			// Add services to the container.
			builder.Services.AddControllers();

			// Add the DbContext for GameDbContext
			builder.Services.AddDbContext<hoppkutyaDbContext>(options =>
				options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

			// Add CORS policy
			builder.Services.AddCors(options =>
			{
				options.AddPolicy("AllowSpecificOrigin",
					builder => builder.WithOrigins("http://127.0.0.1:5500", "https://people.inf.elte.hu/am2vz8") // Engedélyezett origin-ek
									  .AllowAnyMethod()
									  .AllowAnyHeader());
			});

			// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
			builder.Services.AddEndpointsApiExplorer();
			builder.Services.AddSwaggerGen();

			var app = builder.Build();

			// Configure the HTTP request pipeline.
			if (app.Environment.IsDevelopment())
			{
				app.UseSwagger();
				app.UseSwaggerUI();
			}

			app.UseHttpsRedirection();

			// Use CORS
			app.UseCors("AllowSpecificOrigin");

			app.UseAuthorization();


			app.MapControllers();

			app.Run();
		}
	}
}
