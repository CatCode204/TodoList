namespace TodoList.Models.DTOs
{
	public class UpdateItemDTO
	{
		public required int ID { get; set; }
		public required bool Status { get; set; }
	}
}
