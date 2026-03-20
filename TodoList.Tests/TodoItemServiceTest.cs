using TodoList.Infrastructure;
using TodoList.UseCases;

namespace TodoList.Tests
{
	public class TodoItemServiceTest
	{
		[Fact]
		public void AddTodoItem_AddsItem()
		{
			// Arrange
			var repository = new TodoItemInMemoryRepository();
			var service = new TodoListService(repository);
			var item = new TodoItem { Text = "Test add", IsTaskDone = false };

			// Act
			service.AddTodoItem(item);
			var items = service.GetAllTodoItems().ToList();

			// Assert
			Assert.Single(items);
			Assert.Equal("Test add", items[0].Text);
			Assert.False(items[0].IsTaskDone);
		}

		[Fact]
		public void GetAllTodoItems_ReturnsAllItems()
		{
			// Arrange
			var repository = new TodoItemInMemoryRepository();
			var service = new TodoListService(repository);
			var item1 = new TodoItem { Text = "One", IsTaskDone = false };
			var item2 = new TodoItem { Text = "Two", IsTaskDone = true };

			// Act
			service.AddTodoItem(item1);
			service.AddTodoItem(item2);
			var items = service.GetAllTodoItems().OrderBy(i => i.ID).ToList();

			// Assert
			Assert.Equal(2, items.Count);
			Assert.Equal("One", items[0].Text);
			Assert.Equal("Two", items[1].Text);
		}

		[Fact]
		public void RemoveTodoItem_RemovesItem()
		{
			// Arrange
			var repository = new TodoItemInMemoryRepository();
			var service = new TodoListService(repository);
			var item1 = new TodoItem { Text = "Keep", IsTaskDone = false };
			var item2 = new TodoItem { Text = "Remove", IsTaskDone = false };

			// Act
			service.AddTodoItem(item1); // expected ID = 0
			service.AddTodoItem(item2); // expected ID = 1
			service.RemoveTodoItem(0);
			var items = service.GetAllTodoItems().ToList();

			// Assert
			Assert.Single(items);
			Assert.Equal(1, items[0].ID);
			Assert.Equal("Remove", items[0].Text);
		}

		[Fact]
		public void MarkCompleteTodoItem_SetsIsTaskDoneTrue()
		{
			// Arrange
			var repository = new TodoItemInMemoryRepository();
			var service = new TodoListService(repository);
			var item = new TodoItem { Text = "Incomplete", IsTaskDone = false };

			// Act
			service.AddTodoItem(item); // expected ID = 0
			service.MarkCompleteTodoItem(0, true);
			var updated = repository.GetItemByID(0);

			// Assert
			Assert.NotNull(updated);
			Assert.True(updated!.IsTaskDone);
		}

		[Fact]
		public void SetTextTodoItem_UpdatesText()
		{
			// Arrange
			var repository = new TodoItemInMemoryRepository();
			var service = new TodoListService(repository);
			var item = new TodoItem { Text = "Old text", IsTaskDone = false };

			// Act
			service.AddTodoItem(item); // expected ID = 0
			service.SetTextTodoItem(0, "New text");
			var updated = repository.GetItemByID(0);

			// Assert
			Assert.NotNull(updated);
			Assert.Equal("New text", updated!.Text);
		}
	}
}