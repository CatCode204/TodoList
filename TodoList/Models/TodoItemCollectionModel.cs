namespace TodoList.Models
{
	public class TodoItemCollectionModel
	{
		public required IEnumerable<TodoItemModel> Items { get; set; }
	}
}
