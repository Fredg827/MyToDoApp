using System;
using System.Collections.Generic;
using ToDoApp.Domain;

namespace ToDoApp.DataAccess
{
    public interface ITodoRepository
    {
        List<ToDoItem> GetAll();
        ToDoItem GetById(int id);
        ToDoItem Create(string description, DateTime? dueDate);
        bool Update(int id, ToDoItem item);
        bool Delete(int id);
    }
}
