using System;

namespace MyToDoApp
{
    public class ToDoItem
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public DateTime? CompleteDate { get; set; }
        public bool IsCompleted { get; set; }
    }
}
