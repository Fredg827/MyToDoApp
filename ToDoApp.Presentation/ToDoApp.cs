using System.Globalization;
using ToDoApp.DataAccess;
using ToDoApp.DataAccess.Models;
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
            //Console.WriteLine("\n List All - l");
            //Console.WriteLine("\n Create - c");
            //MenuDivider();
            //Console.WriteLine("Please select an option from above or press [x] to exit");
            //MenuDivider();
            //MainMenuOptions();
            var menuItems = new List<ConsoleMenuItem>
            {
                new ConsoleMenuItem("List all", ListAll),
                new ConsoleMenuItem("Create", Create),
                new ConsoleMenuItem("Exit", Exit, "X")
            };
            var menu = new ConsoleMenu(menuItems);
            menu.Display();
        }

        private void MainMenuOptions()
        {
            var list = new List<string>()
            {
                "l", "c", "x"
            };
            var input = GetOption(list);
            switch (input.ToLower())
            {
                case "l":
                    ListAll();
                    break;
                case "c":
                    Create();
                    break;
                case "x":
                    Exit();
                    break;
            }
        }
        public void MenuStringTemplate(string MenuTitle)

        {
            int length = MenuTitle.Length;
            int lengthTwo = length / 2;
            string titleString = new string('=', 40 - lengthTwo);

            Console.WriteLine(titleString + MenuTitle + titleString);
        }

        public void MenuDivider()
        {
            string divider = new string('=', 80);
            Console.WriteLine(divider);
        }

        public string GetOption(List<string> list)
        {

            var input = Console.ReadLine();
            while (string.IsNullOrEmpty(input) || (!list.Contains(input)))

            {
                Console.WriteLine("Input was not Valid - Please select an option from above or press [x] to exit");
                input = Console.ReadLine();
            }
            return input;
        }

        public void Create()
        {
            Console.Clear();
            MenuStringTemplate(" Create ");
            Console.WriteLine("Please enter description:");


            var itemDescription = GetInput<string>("description");
            var dueDate = GetInput<DateTime?>("DueDate");

            _todoRepository.Create(itemDescription, dueDate);


            DisplayMainMenu();
        }

        public void ListAll()
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

            Console.WriteLine("View Item - v");
            Console.WriteLine("Main Menu - m");
            Console.WriteLine("Please select an option from above or press [x] to exit.");
            GetSecondMenuOptions();
        }

        public void ViewItem()
        {
            Console.Clear();
            MenuStringTemplate("View");
            Console.WriteLine("Please enter the id of the item you wish to view:");

            var toDoItem = GetById();
            Console.WriteLine($"To do item selected: {toDoItem.Description}");
            Console.WriteLine("Edit Item - e");
            Console.WriteLine("Delete Item - d");
            Console.WriteLine("Main Menu - m");
            Console.WriteLine("Please select an option from above or press [x] to exit.");
            GetViewMenuOptions(toDoItem.Id);
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

        public void Update(int id)
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

        public void GetSecondMenuOptions()
        {
            var list = new List<string>()
            {
                "v", "m", "x"
            };

            var input = GetOption(list);

            switch (input.ToLower())
            {
                case "v":
                    ViewItem();
                    break;
                case "m":
                    DisplayMainMenu();
                    break;
                case "x":
                    Exit();
                    break;
            }
        }

        public void GetViewMenuOptions(int id)
        {
            var list = new List<string>()
            {
                "e", "d", "m", "x"
            };

            var input = GetOption(list);

            switch (input.ToLower())
            {
                case "e":
                    Update(id);
                    break;
                case "d":
                    Delete(id);
                    break;
                case "m":
                    DisplayMainMenu();
                    break;
                case "x":
                    Exit();
                    break;
            }
        }

        private T GetInput<T>(string property)
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