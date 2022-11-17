using System;

namespace Todo.Application.Commands
{
    public class UpdateToDoItemCommand
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public DateTime? DueDate { get; set; }
        public bool IsCompleted { get; set; }
    }
}