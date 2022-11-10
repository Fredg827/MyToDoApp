using System.Globalization;
using ToDoApp.DataAccess;
using ToDoApp.Domain;
using ToDoApp.Presentation;

namespace MyToDoApp.Presentation
{
    public class ToDoApp
    {

        private readonly ITodoRepository _todoRepository;

        public ToDoApp(ITodoRepository todoRepository)
        {
            _todoRepository = todoRepository;
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

        private void MenuDivider()
        {
            string divider = new string('=', 80);
            Console.WriteLine(divider);
        }

        private string GetOption(List<string> list)
        {

            var input = Console.ReadLine();
            while (string.IsNullOrEmpty(input) || (!list.Contains(input)))

            {
                Console.WriteLine("Input was not Valid - Please select an option from above or press [x] to exit");
                input = Console.ReadLine();
            }
            return input;
        }

        private void Create()
        {
            Console.Clear();
            MenuStringTemplate(" Create ");

            var itemDescription = GetInput<string>("description", d => !string.IsNullOrEmpty(d), "Description cannot be empty");
            var dueDate = GetInput<DateTime?>("DueDate", duedate => duedate >= DateTime.Now, "Due date cannot be in the past");

            _todoRepository.Create(itemDescription, dueDate);

            DisplayMainMenu();
        }

        private void ListAll()
        {
            Console.Clear();
            MenuStringTemplate(" List All ");
            Console.WriteLine("| ID | Description | Complete By | Completed |");

            foreach (var toDoItem in _todoRepository.GetAll())
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
            ShowViewMenuOptions(toDoItem.Id);
        }

        private ToDoItem GetById()
        {
            while (true)
            {
                var id = GetInput<int>("itemId");

                var toDoItem = _todoRepository.GetById(id);

                if (toDoItem is not null)
                    return toDoItem;

                Console.WriteLine($"Could not find any to do item with id {id}");
            }

            return null;
        }

        public void Delete(int id)
        {
            var isDeleted = _todoRepository.Delete(id);

            if (!isDeleted)
                Console.WriteLine("Could not remove");

            DisplayMainMenu();
        }

        public void Edit(int id)
        {
            var itemDescription = GetInput<string>("description");
            var dueDate = GetInput<DateTime?>("DueDate");

            var toDoItem = new ToDoItem
            {
                Description = itemDescription,
                DueDate = dueDate,
            };

            var isUpdated = _todoRepository.Update(id, toDoItem);
            if (!isUpdated)
                Console.WriteLine("Could not update");

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

        public void ShowViewMenuOptions(int id)
        {
            var menuItems = new List<ConsoleMenuItem>
            {
                new ConsoleMenuItem("Update", () => Edit(id)),
                new ConsoleMenuItem("Delete", () => Delete(id)),
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