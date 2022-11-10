using System.Collections.Generic;
using Todo.Application.Commands;
using ToDoApp.Domain;

namespace Todo.Application
{
    public interface ITodoApplication
    {
        List<ToDoItem> GetAll();
        ToDoItem GetById(int id);
        Result Create(CreateToDoItemCommand createToDoItem);
        Result Update(UpdateToDoItemCommand updateToDoItem);

        Result Delete(DeleteToDoItemCommand deleteToDoItem);
    }
}