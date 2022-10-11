using System;
using System.Globalization;


namespace MyToDoApp
{
    public class ToDoItemInput
    {
        static void Main(string[] args)

        {
            var dateValidator = new ChDateValidator();
            Console.WriteLine("=============================Menu=============================");
            Console.WriteLine("\n List All - l");
            Console.WriteLine("\n Create - c");
            Console.WriteLine("\n==============================================================");
            Console.WriteLine("Please select an option from above or press [x] to exit");
            Console.WriteLine("==============================================================");
            string input = Console.ReadLine();

            input = GetMainMenuOptions(input);

        }

        private static string GetMainMenuOptions(string input)
        {
            while (string.IsNullOrEmpty(input) || (input != "l" && input != "c" && input != "x"))
            {
                Console.WriteLine("Input was not Valid - Please select an option from above or press [x] to exit");
                input = Console.ReadLine();
            }

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

            return input;
        }

        public static void Create()
        {
            Console.Clear();
            Console.WriteLine("==================================Create==================================");
            Console.WriteLine("Please enter description:");
            string itemDescription = Console.ReadLine();
            Console.WriteLine("Item Description :" + itemDescription);

            while (string.IsNullOrEmpty(itemDescription))
            {
                Console.WriteLine("Input was invalid - please add a description");
                itemDescription = Console.ReadLine();
            }

            var dueDate = GetDateTimeFromInput("Please enter complete by date in the following format – dd / mm / YYYY or press ENTER to skip:");

            //var dateFormats = new[] { "dd.MM.yyyy", "dd-MM-yyyy", "dd/MM/yyyy" };
            //Console.WriteLine("Please enter complete by date in the following format – dd/mm/YYYY or press ENTER to skip:");
            //bool validDate = false;
            //string completedByDate = string.Empty;
            //while (string.IsNullOrEmpty(itemDescription))
            //{
            //    Console.WriteLine("To do item -" + itemDescription);
            //}

            //while (!validDate)
            //{
            //    completedByDate = Console.ReadLine();
            //    validDate = DateTime.TryParseExact(
            //        completedByDate,
            //        dateFormats,
            //        DateTimeFormatInfo.InvariantInfo,
            //        DateTimeStyles.None,
            //        out var scheduleDate);
            //    if (!validDate && !string.IsNullOrEmpty(itemDescription))
            //    {
            //        Console.WriteLine("Invalid date: \"{0}\" - Please enter a valid date", completedByDate);

            //    }

            //    else string.IsNullOrEmpty(itemDescription);
            //    {
            //        Console.WriteLine("To do item -" + itemDescription);
            //    }
            //}

        }

        public static DateTime? GetDateTimeFromInput(string prompt)
        {
            Console.WriteLine(prompt);
            var input = Console.ReadLine();

            if (string.IsNullOrEmpty(input))
            {
                return null;
            }

            var isValidDate = false;

            while (!isValidDate)
            {
                isValidDate = DateTime.TryParse(input, out DateTime date);
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
            Console.WriteLine("============================= All Items =============================");
            Console.WriteLine("| ID | Description | Complete By | Completed |");
            Console.ReadLine();

        }

        private static void Exit()
        {
            Environment.Exit(0);
        }

    }
}