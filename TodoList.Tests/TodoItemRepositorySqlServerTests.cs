using Microsoft.Data.SqlClient;
using System;
using TodoList.Infrastructure;
using TodoList.UseCases;
using Xunit;

namespace TodoList.Tests
{
	public class TodoItemRepositorySqlServerTests
	{
		private const string ConnectionString = "Server=ANTRUONG204;Database=TodoList;Trusted_Connection=True;TrustServerCertificate=True;";

		[Fact]
		public void Connection_Open_And_SelectOne_ReturnsOne()
		{
			using var connection = new SqlConnection(ConnectionString);
			connection.Open();

			using var command = connection.CreateCommand();
			command.CommandText = "SELECT 1";

			var result = command.ExecuteScalar();
			Assert.NotNull(result);
			Assert.IsType<int>(result);
			Assert.Equal(1, (int)result);
		}

		[Fact]
		public void TodoItemsTable_Exists()
		{
			using var connection = new SqlConnection(ConnectionString);
			connection.Open();

			using var command = connection.CreateCommand();
			// Query INFORMATION_SCHEMA to check for the existence of the TodoItems table.
			command.CommandText = @"
				SELECT COUNT(*) 
				FROM INFORMATION_SCHEMA.TABLES 
				WHERE TABLE_SCHEMA = 'dbo' AND TABLE_NAME = 'TodoItems'";

			var result = command.ExecuteScalar();
			Assert.NotNull(result);

			int count = Convert.ToInt32(result);
			Assert.True(count > 0, "Expected table 'TodoItems' to exist in the TodoList database.");
		}

		[Fact]
		public void Add_Get_Update_Delete_Works()
		{
			using var connection = new SqlConnection(ConnectionString);
			connection.Open();

			var repo = new TodoItemRepositorySqlServer(connection);

			// clean slate
			repo.DeleteAll();

			var newItem = new TodoItem { Text = "integration-test", IsTaskDone = false };
			var added = repo.Add(newItem);
			Assert.NotNull(added);
			Assert.True(added!.ID > 0);

			var items = repo.GetItems().ToList();
			Assert.Single(items);
			Assert.Equal("integration-test", items[0].Text);

			var fetched = repo.GetItemByID(added.ID);
			Assert.NotNull(fetched);
			Assert.Equal(added.ID, fetched!.ID);

			// update
			added.IsTaskDone = true;
			var updated = repo.Update(added);
			Assert.True(updated);

			var fetchedAfterUpdate = repo.GetItemByID(added.ID);
			Assert.True(fetchedAfterUpdate!.IsTaskDone);

			// delete
			var deleted = repo.Delete(added.ID);
			Assert.True(deleted);

			var afterDeleteItems = repo.GetItems().ToList();
			Assert.Empty(afterDeleteItems);
		}
	}
}
