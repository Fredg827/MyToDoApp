using System;

namespace MyToDoApp
{
    public class ToDoItem
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public DateOnly? DueDate { get; set; }
        public bool IsCompleted { get; set; }


        public override string ToString()
        {
            return $"{Id} {Description} {DueDate}";
        }

    }
}
