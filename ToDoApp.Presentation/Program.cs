using Microsoft.Extensions.DependencyInjection;
using Todo.Application;
using ToDoApp.DataAccess;

var services = new ServiceCollection();
//services.AddSingleton<ITodoRepository, DictionaryTodoRepository>();
services.AddScoped<MyToDoApp.Presentation.ToDoApp>();
services.AddTransient<ToDoContext>();
services.AddTransient<ITodoRepository, DbToDoRepository>();
services.AddTransient<ITodoApplication, ToDoApplication>();

var serviceProvider = services.BuildServiceProvider();
serviceProvider.GetService<MyToDoApp.Presentation.ToDoApp>()!.Start();