namespace TodoList.Models
{
	public class TodoItemModel
	{
		public required int ID { get; set; }
		public required string Text { get; set; }
		public bool IsTaskCompleted { get; set; } = false;
	}
}
