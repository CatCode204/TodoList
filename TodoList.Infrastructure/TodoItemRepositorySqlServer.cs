using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoList.UseCases;

namespace TodoList.Infrastructure
{
	public class TodoItemRepositorySqlServer : ITodoItemRepository
	{
		private const string SQL_SELECT = "SELECT * FROM TodoItems ";
		private const string SQL_WHERE = "WHERE 1 = 1 ";
		private const string SQL_UPDATE = "UPDATE TodoItems SET [Text] = @text, [IsComplete] = @status ";
		private const string SQL_DELETE = "DELETE FROM TodoItems ";
		private const string SQL_INSERT_INTO = "INSERT INTO TodoItems VALUES(@text, @status)";

		private readonly SqlConnection sqlConnection;
		private readonly SqlTransaction? sqlTransaction;

		public TodoItemRepositorySqlServer(SqlConnection sqlConnection, SqlTransaction? sqlTransaction = null)
		{
			this.sqlConnection = sqlConnection;
			this.sqlTransaction = sqlTransaction;
		}

		public TodoItem? Add(TodoItem item)
		{
			var sqlBuilder = new StringBuilder(SQL_INSERT_INTO);
			sqlBuilder.Append("; SELECT SCOPE_IDENTITY();");

			var sql = sqlBuilder.ToString();

			var command = new SqlCommand(sql, sqlConnection);
			command.Transaction = sqlTransaction ?? null;

			command.Parameters.Add(new SqlParameter("@text", SqlDbType.NVarChar)).Value = item.Text;
			command.Parameters.Add(new SqlParameter("@status", SqlDbType.Bit)).Value = item.IsTaskDone;

			var insertedID = command.ExecuteScalar();

			if (insertedID != null && insertedID != DBNull.Value)
			{
				item.ID = Convert.ToInt32(insertedID);
				return item;
			}

			return null;
		}

		public bool Delete(int id)
		{
			TodoItem? item = GetItemByID(id);

			if (item == null) return false;

			var sqlBuilder = new StringBuilder(SQL_DELETE);
			sqlBuilder.Append(SQL_WHERE);
			sqlBuilder.Append($"AND ID = @id");

			var sql = sqlBuilder.ToString();

			var command = new SqlCommand(sql, sqlConnection);
			command.Transaction = sqlTransaction ?? null;
			command.Parameters.Add(new SqlParameter("@id", SqlDbType.Int)).Value = item.ID;

			return command.ExecuteNonQuery() > 0;
		}

		public void DeleteAll()
		{
			var command = new SqlCommand(SQL_DELETE, sqlConnection);
			command.Transaction = sqlTransaction ?? null;

			command.ExecuteNonQuery();
		}

		public TodoItem? GetItemByID(int id)
		{
			var sqlBuilder = new StringBuilder(SQL_SELECT);

			sqlBuilder.Append(SQL_WHERE);
			sqlBuilder.Append("AND ID = @id");

			var sql = sqlBuilder.ToString();

			var command = sqlConnection.CreateCommand();
			command.CommandText = sql;
			command.Transaction = sqlTransaction ?? null;

			command.Parameters.Add(new SqlParameter("id", SqlDbType.Int)).Value = id;

			using var reader = command.ExecuteReader();
			if (reader != null && reader.Read())
			{
				return new TodoItem() { 
					ID = reader.GetInt32(0), 
					Text = reader.GetString(1), 
					IsTaskDone = reader.GetBoolean(2) 
				};
			}

			return null;
		}

		public IEnumerable<TodoItem> GetItems()
		{
			var command = new SqlCommand(SQL_SELECT, sqlConnection);
			command.Transaction = sqlTransaction ?? null;

			using var reader = command.ExecuteReader();

			if (reader == null) return [];

			List<TodoItem> items = [];
			while (reader.Read())
			{
				var item = new TodoItem() {
					ID = reader.GetInt32(0),
					Text = reader.GetString(1),
					IsTaskDone= reader.GetBoolean(2)
				};
				items.Add(item);
			}

			return items;
		}

		public bool Update(TodoItem todoItem)
		{
			var sqlBuilder = new StringBuilder(SQL_UPDATE);
			sqlBuilder.Append(SQL_WHERE);
			sqlBuilder.Append("AND ID = @id");

			string sql = sqlBuilder.ToString();

			var command = new SqlCommand(sql, sqlConnection);
			command.Transaction  = sqlTransaction ?? null;

			command.Parameters.Add(new SqlParameter("@id", SqlDbType.Int)).Value = todoItem.ID;
			command.Parameters.Add(new SqlParameter("@text", SqlDbType.NVarChar)).Value = todoItem.Text;
			command.Parameters.Add(new SqlParameter("@status", SqlDbType.Bit)).Value = todoItem.IsTaskDone;

			return command.ExecuteNonQuery() > 0;
		}
	}
}
