using System.ComponentModel.DataAnnotations;

namespace hoppKutya.Models
{
	public class GameTables
	{
		[Key]
		public int TableId { get; set; }
		public string? Player1 { get; set; }
		public string? Player2 { get; set; }
		public double Player1Points { get; set; }
		public double Player2Points { get; set; }
		public DateTime LastHoppKutya { get; set; }
	}
}
