namespace TodoList.UseCases
{
	public class TodoItem
	{
		public int ID { get; set; } = 0;
		public string Text { get; set; } = string.Empty;
		public bool IsTaskDone { get; set; } = false;
	}
}
