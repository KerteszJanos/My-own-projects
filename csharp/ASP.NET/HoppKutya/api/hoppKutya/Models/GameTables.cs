using System.ComponentModel.DataAnnotations;

namespace hoppKutya.Models
{
	public class GameTables
	{
		[Key]
		public int TableId { get; set; }
		public string? Player1 { get; set; }
		public string? Player2 { get; set; }
		public int Player1Points { get; set; }
		public int Player2Points { get; set; }
		public DateTime LastHoppKutya { get; set; }
	}
}
