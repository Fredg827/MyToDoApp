using Microsoft.Extensions.DependencyInjection;
using ToDoApp.DataAccess;

var services = new ServiceCollection();
services.AddSingleton<ITodoRepository, DictionaryTodoRepository>();
services.AddSingleton<MyToDoApp.Presentation.ToDoApp>();

var serviceProvider = services.BuildServiceProvider();
serviceProvider.GetService<MyToDoApp.Presentation.ToDoApp>()!.Start();