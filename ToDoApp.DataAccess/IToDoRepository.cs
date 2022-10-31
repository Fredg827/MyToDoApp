using System;
using System.Collections.Generic;
using ToDoApp.DataAccess.Models;

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
