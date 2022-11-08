namespace ToDoApp.Presentation
{
    internal class ConsoleMenu
    {
        private List<ConsoleMenuItem> _items;
        public ConsoleMenu(List<ConsoleMenuItem> items)
        {
            _items = items;
        }

        public void Display()
        {
            foreach (ConsoleMenuItem item in _items)
            {
                Console.WriteLine($"{item.Title} - {item.Key}");
            }
            var menuKeys = _items.Select(x => x.Key).ToList();
            var option = GetOption(menuKeys);
            var menuItem = _items.First(x => x.Key == option);
            menuItem.ShowMenu.Invoke();
        }

        //private List<string> Test(List<ConsoleMenuItem> items)
        //{
        //    var result = new List<string>();
        //    foreach (var item in items)
        //    {
        //        result.Add(item.Key);
        //    }
        //    return result;
        //}


        public string GetOption(List<string> list)
        {
            Console.WriteLine("Please select an option from above");
            var input = Console.ReadLine();
            while (string.IsNullOrEmpty(input) || (!list.Contains(input)))

            {
                Console.WriteLine("Input was not Valid - Please select an option from above");
                input = Console.ReadLine();
            }
            return input;
        }
    }

    public class ConsoleMenuItem
    {
        public string Title { get; }
        public string Key { get; }
        public Action ShowMenu { get; }

        public ConsoleMenuItem(string title, Action showMenu, string key = null)
        {
            Title = title;
            Key = string.IsNullOrEmpty(key) ? title.First().ToString().ToLower() : key;
            ShowMenu = showMenu;
        }

    }
}
