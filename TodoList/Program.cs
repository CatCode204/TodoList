using Microsoft.Data.SqlClient;
using TodoList.Infrastructure;
using TodoList.UseCases;

namespace TodoList
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			// Add services to the container.
			builder.Services.AddControllersWithViews();

			string? dataBaseType = builder.Configuration.GetValue<string>("Database");

			SqlConnection? connection = null;

			if (!String.IsNullOrEmpty(dataBaseType))
			{
				var connectionString = builder.Configuration.GetConnectionString(dataBaseType);
				connection = new SqlConnection(connectionString);
				connection.Open();
				builder.Services.AddTransient<ITodoItemRepository, TodoItemRepositorySqlServer>(serviceProvider => {
					return new TodoItemRepositorySqlServer(connection);
				});
			}
			else
			{
				builder.Services.AddSingleton<ITodoItemRepository, TodoItemInMemoryRepository>();
			}
			
				

			builder.Services.AddTransient<TodoListService>();

			var app = builder.Build();

			// Configure the HTTP request pipeline.
			if (!app.Environment.IsDevelopment())
			{
				app.UseExceptionHandler("/Home/Error");
				// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
				app.UseHsts();
			}

			app.UseHttpsRedirection();
			app.UseStaticFiles();

			app.UseRouting();

			app.UseAuthorization();

			app.MapControllerRoute(
				name: "default",
				pattern: "{controller=Home}/{action=Index}/{id?}");

			app.Run();

			connection?.Close();
		}
	}
}
