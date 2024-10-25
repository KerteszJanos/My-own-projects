using hoppKutya.Models;
using Microsoft.EntityFrameworkCore;

namespace hoppKutya.Contexts
{
	public class hoppkutyaDbContext : DbContext
	{
		// Konstruktor a DbContextOptions paraméterrel
		public hoppkutyaDbContext(DbContextOptions<hoppkutyaDbContext> options) : base(options) { }

		// DbSet tulajdonság a GameTable entitás számára
		public DbSet<GameTables> GameTables { get; set; }
	}
}
