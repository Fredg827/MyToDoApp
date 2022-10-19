using System;
using System.Collections.Generic;


namespace MyToDoApp
{
    public class ToDoItemInput
    {

        private static Dictionary<int, ToDoItem> _toDoItems = new Dictionary<int, ToDoItem>();


        private static int _lastId = 0;


        static void Main(string[] args)

        {
            DisplayMainMenu();
        }





        private static void DisplayMainMenu()
        {
            Console.Clear();
            MenuStringTemplate(" Menu ");
            Console.WriteLine("\n List All - l");
            Console.WriteLine("\n Create - c");
            MenuDivider();
            Console.WriteLine("Please select an option from above or press [x] to exit");
            MenuDivider();
            MainMenuOptions();
        }





        private static void MainMenuOptions()
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


        public static void MenuStringTemplate(string MenuTitle)

        {
            int length = MenuTitle.Length;
            int lengthTwo = length / 2;
            string titleString = new string('=', 40 - lengthTwo);

            Console.WriteLine(titleString + MenuTitle + titleString);
        }

        public static void MenuDivider()
        {
            string divider = new string('=', 80);
            Console.WriteLine(divider);
        }




        public static string GetOption(List<string> list)
        {

            var input = Console.ReadLine();
            while (string.IsNullOrEmpty(input) || (!list.Contains(input)))

            {
                Console.WriteLine("Input was not Valid - Please select an option from above or press [x] to exit");
                input = Console.ReadLine();
            }
            return input;
        }







        public static void Create()
        {
            Console.Clear();
            MenuStringTemplate(" Create ");
            Console.WriteLine("Please enter description:");
            var itemDescription = GetNonEmptyString();
            var dueDate = GetDateTimeFromInput();

            var newId = ++_lastId;

            var toDoItem = new ToDoItem
            {
                Id = newId,
                Description = itemDescription,
                DueDate = dueDate,
            };

            _toDoItems.Add(newId, toDoItem);

            DisplayMainMenu();
        }






        private static string GetNonEmptyString()
        {
            string itemDescription = Console.ReadLine();
            Console.WriteLine("Item Description :" + itemDescription);
            while (string.IsNullOrEmpty(itemDescription))
            {
                Console.WriteLine("Input was invalid - please add a description");
                itemDescription = Console.ReadLine();
            }
            return itemDescription;
        }







        public static DateOnly? GetDateTimeFromInput()
        {
            Console.WriteLine("Please enter a valid due date (dd/mm/yyyy) - or press enter to skip");
            var input = Console.ReadLine();
            if (string.IsNullOrEmpty(input))
            {
                return null;
            }
            var isValidDate = false;

            while (!isValidDate)
            {
                isValidDate = DateOnly.TryParse(input, out DateOnly date);
                if (isValidDate)
                {
                    return date;
                }
                else
                {
                    Console.WriteLine("Please enter a valid date or press enter to skip");
                    input = Console.ReadLine();
                    if (string.IsNullOrEmpty(input))
                    {
                        break;
                    }
                }
            }
            return null;
        }




        public static void ListAll()
        {
            Console.Clear();
            MenuStringTemplate(" List All ");
            Console.WriteLine("| ID | Description | Complete By | Completed |");

            //var x = new DateTime().ToString("dd/MM/yyyy");

            foreach (var toDoItem in _toDoItems)
            {

                var nullString = "-";

                if (toDoItem.Value.DueDate == null)
                    Console.WriteLine(toDoItem.Value + nullString);

                else
                    Console.WriteLine(toDoItem.Value);

            }


            //loop thorugh dictionary 
            //build string that going to be printed 
            //complete by should be in a format of dd/mm/yyyy
            // if null print "-"
            //completed - yes or no 


            Console.WriteLine("View Item - v");
            Console.WriteLine("Main Menu - m");
            Console.WriteLine("Please select an option from above or press [x] to exit.");
            string input = Console.ReadLine();
            GetSecondMenuOptions();
        }







        private static void GetSecondMenuOptions()
        {
            var list = new List<string>()
            {
                "v", "m", "x"
            };

            var input = GetOption(list);

            switch (input.ToLower())
            {
                case "v":
                    ListAll();
                    break;
                case "m":
                    DisplayMainMenu();
                    break;
                case "x":
                    Exit();
                    break;
            }
        }






        private static void Exit()
        {
            Environment.Exit(0);
        }

    }
}