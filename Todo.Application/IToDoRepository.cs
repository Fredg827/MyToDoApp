using System.Collections.Generic;
using ToDoApp.Domain;

namespace Todo.Application
{
    public interface ITodoRepository
    {
        List<ToDoItem> GetAll();
        ToDoItem GetById(int id);
        int? Create(ToDoItem item);
        bool Update(ToDoItem item);
        bool Delete(int id);
    }
}