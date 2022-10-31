using System;

namespace ToDoApp.DataAccess.Models
{
    public class ToDoItem
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public DateTime? DueDate { get; set; }
        public bool IsCompleted { get; set; }

    }
}
