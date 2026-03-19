using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TodoList.UseCases
{
	public interface ITodoItemRepository
	{
		IEnumerable<TodoItem> GetItems();
		TodoItem? GetItemByID(int id);

		TodoItem Add(TodoItem item);

		bool Update(TodoItem todoItem);
		bool Delete(int id);
	}
}
