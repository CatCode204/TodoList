using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TodoList.UseCases
{
	public class TodoListService
	{
		private readonly ITodoItemRepository repository;

		public TodoListService(ITodoItemRepository repository)
		{
			this.repository = repository;
		}

		public IEnumerable<TodoItem> GetAllTodoItems()
		{
			return repository.GetItems();
		}

		public void AddTodoItem(TodoItem item)
		{
			repository.Add(item);
		}

		public void RemoveTodoItem(int id)
		{
			repository.Delete(id);
		}

		public void MarkCompleteTodoItem(int id, bool isComplete = true)
		{
			TodoItem? todoItem = repository.GetItemByID(id);
			if (todoItem == null) return;
			
			todoItem.IsTaskDone = isComplete;
			repository.Update(todoItem);
		}

		public void SetTextTodoItem(int id, string text)
		{
			TodoItem? todoItem = repository.GetItemByID(id);
			if (todoItem == null) return;

			todoItem.Text = text;
			repository.Update(todoItem);
		}
	}
}
