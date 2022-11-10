using System;

namespace Todo.Application.Commands
{
    public class CreateToDoItemCommand
    {
        public string Description { get; set; }

        public DateTime? DueDate { get; set; }

    }
}
