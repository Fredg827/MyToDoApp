using System;
using System.Collections.Generic;
using System.Linq;
using Todo.Application;
using ToDoApp.Domain;

namespace ToDoApp.DataAccess
{
    public class DictionaryTodoRepository : ITodoRepository
    {
        private static Dictionary<int, ToDoItem> _toDoItems = new Dictionary<int, ToDoItem>()
        {
            {1, new ToDoItem() {Id = 1, Description = "finish the app"} },
            {2, new ToDoItem() {Id = 2, Description = "vjndovnsovmsdv" , DueDate = new DateTime(2022, 10, 20)} },
        };
        private static int _lastId = 2;


        public int? Create(ToDoItem item)
        {
            var newId = ++_lastId;

            var toDoItem = new ToDoItem
            {
                Id = newId,
                Description = item.Description,
                DueDate = item.DueDate,
            };

            _toDoItems.Add(newId, toDoItem);

            return newId;
        }

        public bool Delete(int id)
        {
            if (!_toDoItems.TryGetValue(id, out var todoItem))
            {
                return false;
            }

            _toDoItems.Remove(id);
            return true;
        }

        public List<ToDoItem> GetAll()
        {
            return _toDoItems.Values.ToList();
        }

        public ToDoItem GetById(int id)
        {
            if (_toDoItems.TryGetValue(id, out var toDoItem))
            {
                return toDoItem;
            }
            return null;
        }

        public bool Update(ToDoItem item)
        {
            var itemToUpdate = GetById(item.Id);

            if (itemToUpdate is null)
            {
                return false;
            }

            itemToUpdate.Description = item.Description;
            itemToUpdate.DueDate = item.DueDate;
            return true;
        }
    }
}