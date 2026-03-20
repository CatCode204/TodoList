using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using TodoList.Models;
using TodoList.Models.DTOs;
using TodoList.UseCases;

namespace TodoList.Controllers
{
	public class HomeController : Controller
	{
		private readonly ILogger<HomeController> _logger;
		private readonly TodoListService service;

		public HomeController(ILogger<HomeController> logger, TodoListService todoListService)
		{
			_logger = logger;
			service = todoListService;
		}

		[HttpGet]
		public IActionResult Index()
		{
			IEnumerable<TodoItem> todoItems = service.GetAllTodoItems();

			return View(new TodoItemCollectionModel() 
				{ Items = todoItems.Select(
					item => new TodoItemModel() { ID = item.ID, Text = item.Text, IsTaskCompleted = item.IsTaskDone}
				) 
			});
		}

		[HttpPost]
		public IActionResult Index([FromForm] string text)
		{
			TodoItem item = new TodoItem() { Text = text };
			service.AddTodoItem(item);

			IEnumerable<TodoItem> todoItems = service.GetAllTodoItems();

			return RedirectPermanent("/");
		}

		[HttpPut]
		public IActionResult Index([FromBody] UpdateItemDTO updateItemDTO)
		{
			_logger.LogInformation("Update TodoItem ID: {id}, status: {status}", updateItemDTO.ID, updateItemDTO.Status);
			service.MarkCompleteTodoItem(updateItemDTO.ID, updateItemDTO.Status);

			IEnumerable<TodoItem> todoItems = service.GetAllTodoItems();

			return View(new TodoItemCollectionModel() {
				Items = todoItems.Select(
					item => new TodoItemModel() { ID = item.ID, Text = item.Text, IsTaskCompleted = item.IsTaskDone }
				)
			});
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}
