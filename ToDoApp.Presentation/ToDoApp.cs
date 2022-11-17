using System.Globalization;
using Todo.Application;
using Todo.Application.Commands;
using ToDoApp.Domain;
using ToDoApp.Presentation;

namespace MyToDoApp.Presentation
{

    public class ToDoApp
    {
        private readonly ITodoApplication _app;

        public ToDoApp(ITodoApplication app)
        {
            _app = app;
        }

        public void Start()

        {
            DisplayMainMenu();
        }

        private void DisplayMainMenu()
        {
            Console.Clear();
            MenuStringTemplate(" Menu ");

            var menuItems = new List<ConsoleMenuItem>
            {
                new ConsoleMenuItem("List all", ListAll),
                new ConsoleMenuItem("Create", Create),
                new ConsoleMenuItem("Exit", Exit, "X")
            };
            var menu = new ConsoleMenu(menuItems);
            menu.Display();
        }

        private void MenuStringTemplate(string MenuTitle)

        {
            int length = MenuTitle.Length;
            int lengthTwo = length / 2;
            string titleString = new string('=', 40 - lengthTwo);

            Console.WriteLine(titleString + MenuTitle + titleString);
        }

        //private void MenuDivider()
        //{
        //    string divider = new string('=', 80);
        //    Console.WriteLine(divider);
        //}

        //private string GetOption(List<string> list)
        //{

        //    var input = Console.ReadLine();
        //    while (string.IsNullOrEmpty(input) || (!list.Contains(input)))

        //    {
        //        Console.WriteLine("Input was not Valid - Please select an option from above or press [x] to exit");
        //        input = Console.ReadLine();
        //    }
        //    return input;
        //}

        private void Create()
        {
            Console.Clear();
            MenuStringTemplate(" Create ");

            var itemDescription = GetInput<string>("description", d => !string.IsNullOrEmpty(d), "Description cannot be empty");
            var dueDate = GetInput<DateTime?>("DueDate", duedate => duedate >= DateTime.Now, "Due date cannot be in the past");

            var createToDoItem = new CreateToDoItemCommand
            {
                Description = itemDescription,
                DueDate = dueDate
            };

            _app.Create(createToDoItem);

            DisplayMainMenu();
        }

        private void ListAll()
        {
            Console.Clear();
            MenuStringTemplate(" List All ");
            Console.WriteLine("| ID | Description | Complete By | Completed |");

            foreach (var toDoItem in _app.GetAll())
            {
                var formatedDate = toDoItem.DueDate.HasValue ? toDoItem.DueDate.Value.ToString("dd/MM/yyyy") : "-";

                var formatedIsCompleted = toDoItem.IsCompleted ? "yes" : "no";

                var row = $"{toDoItem.Id} {toDoItem.Description} {formatedDate} {formatedIsCompleted}";

                Console.WriteLine(row);
            }

            ShowIndexMenuOptions();
        }

        public void ViewItem()
        {
            Console.Clear();
            MenuStringTemplate("View");
            Console.WriteLine("Please enter the id of the item you wish to view:");

            var toDoItem = GetById();
            Console.WriteLine($"You have selected: {toDoItem.Description}");
            ShowViewMenuOptions(toDoItem);
        }

        private ToDoItem GetById()
        {
            while (true)
            {
                var id = GetInput<int>("itemId");

                var toDoItem = _app.GetById(id);

                if (toDoItem is not null)
                    return toDoItem;

                Console.WriteLine($"Could not find any to do item with id {id}");
            }

            return null;
        }

        public void Delete(int id)
        {
            var deleteResult = _app.Delete(id);

            if (!deleteResult.Success)
            {
                foreach (var error in deleteResult.GetErrors())
                {
                    Console.WriteLine(error);
                }
            }

            DisplayMainMenu();
        }

        public void Edit(int id)
        {
            var itemDescription = GetInput<string>("description");
            var dueDate = GetInput<DateTime?>("DueDate");

            var updateToDoItemCommand = new UpdateToDoItemCommand
            {
                Description = itemDescription,
                DueDate = dueDate,
                Id = id
            };



            var updateResult = _app.Update(updateToDoItemCommand);
            if (!updateResult.Success)
                foreach (var error in updateResult.GetErrors())
                {
                    Console.WriteLine(error);
                }
            DisplayMainMenu();

        }

        public void MarkAsCompleted(ToDoItem toDoItem)
        {
            var updateToDoItemCommand = new UpdateToDoItemCommand
            {
                Id = toDoItem.Id,   
                DueDate = toDoItem.DueDate,
                Description = toDoItem.Description,
                IsCompleted = true
            };

            var updateIsCompleted = _app.Update(updateToDoItemCommand);
            if (!updateIsCompleted.Success)
                foreach (var error in updateIsCompleted.GetErrors())
                {
                    Console.WriteLine(error);
                }
            DisplayMainMenu();
        }

        public void ShowIndexMenuOptions()
        {
            var menuItems = new List<ConsoleMenuItem>
            {
                new ConsoleMenuItem("View Item", ViewItem),
                new ConsoleMenuItem("Display Main Menu", DisplayMainMenu),
                new ConsoleMenuItem("Exit", Exit, "X")
            };
            var menu = new ConsoleMenu(menuItems);
            menu.Display();
        }

        public void ShowViewMenuOptions(ToDoItem toDoItem)
        {
            var menuItems = new List<ConsoleMenuItem>
            {
                new ConsoleMenuItem("Update", () => Edit(toDoItem.Id)),
                new ConsoleMenuItem("Mark as Completed", () => MarkAsCompleted(toDoItem)),
                new ConsoleMenuItem("Delete", () => Delete(toDoItem.Id)),
                new ConsoleMenuItem("Display Main Menu", DisplayMainMenu),
                new ConsoleMenuItem("Exit", Exit, "X")
            };
            var menu = new ConsoleMenu(menuItems);
            menu.Display();
        }

        private T GetInput<T>(string property, Func<T, bool> validateFunc = null, string validationMessage = null)
        {
            var isValid = false;
            T result = default;

            var isNullable = Nullable.GetUnderlyingType(typeof(T)) is not null;
            var prompt = $"Enter {property}";

            if (isNullable)
                prompt += " or press ENTER to skip";

            while (!isValid)
            {
                Console.WriteLine(prompt + ":");
                var input = Console.ReadLine();

                if (string.IsNullOrEmpty(input) && isNullable)
                    return result;

                try
                {
                    var type = Nullable.GetUnderlyingType(typeof(T)) ?? typeof(T);
                    result = (T)Convert.ChangeType(input, type, CultureInfo.InvariantCulture);
                    isValid = true;
                }
                catch (Exception)
                {
                    Console.WriteLine($"Input is not valid, expected a {GetTypeName(result)}");
                }

                if (validateFunc != null && validateFunc.Invoke(result) == false)
                {
                    if (string.IsNullOrEmpty(validationMessage))
                    {
                        validationMessage = "Could not validate";
                    }
                    Console.WriteLine(validationMessage);

                    isValid = false;
                }
            }

            return result;
        }

        private static string GetTypeName<T>(T o)
        {
            return o switch
            {
                int or double or long or float or decimal => "number",
                DateTime => "date",
                _ => o.GetType().Name
            };
        }

        private void Exit()
        {
            Environment.Exit(0);
        }
    }
}