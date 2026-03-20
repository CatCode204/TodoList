using TodoList.UseCases;

namespace TodoList.Infrastructure
{
	public class TodoItemInMemoryRepository : ITodoItemRepository
	{
		private readonly List<TodoItem> todoItems = new List<TodoItem>();

		public IEnumerable<TodoItem> GetItems()
		{
			return todoItems;
		}

		public TodoItem? GetItemByID(int id)
		{
			return todoItems.FirstOrDefault(item => item.ID == id);
		}

		public TodoItem? Add(TodoItem todoItem)
		{
			todoItem.ID = todoItems.Count;
			todoItems.Add(todoItem);
			return todoItem;
		}

		public bool Delete(int id)
		{
			TodoItem? deleteItem = todoItems.FirstOrDefault(item => item.ID == id);
			if (deleteItem == null)
			{
				return false;
			}

			todoItems.Remove(deleteItem);
			return true;
		}

		public bool Update(TodoItem todoItem)
		{
			TodoItem? updatedItem = todoItems.FirstOrDefault(item => item.ID == todoItem.ID);

			if (updatedItem != null)
			{
				updatedItem.ID = todoItem.ID;
				updatedItem.Text = todoItem.Text;
				updatedItem.IsTaskDone = todoItem.IsTaskDone;
				return true;
			}
			return false;
		}

		public void DeleteAll()
		{
			todoItems.Clear(); 
		}
	}
}
