using System;
using System.Collections.Generic;
using System.Linq;
using Todo.Application;
using ToDoApp.DataAccess.DomainModels;
using ToDoApp.Domain;

namespace ToDoApp.DataAccess
{
    public class DbToDoRepository : ITodoRepository
    {
        private readonly ToDoContext _context;

        public DbToDoRepository(ToDoContext context)
        {
            _context = context;
        }

        public int? Create(ToDoItem item)
        {
            var domainItem = new DomainToDoItem
            {
                Description = item.Description,
                DueDate = item.DueDate,
                CreatedDate = DateTime.UtcNow
            };
            _context.ToDoItems.Add(domainItem);
            _context.SaveChanges();
            return domainItem.Id;
        }

        public bool Delete(int id)
        {
            var domainItem = _context.ToDoItems.FirstOrDefault(x => x.Id == id);
            if (domainItem is null)
            {
                return false;
            }
            _context.ToDoItems.Remove(domainItem);

            var deleteCount = _context.SaveChanges();
            var deleteWasSuccesful = deleteCount > 0;

            return deleteWasSuccesful;
        }

        public List<ToDoItem> GetAll()
        {
            return _context.ToDoItems.Select(MapFromDomain).ToList();
        }

        public ToDoItem GetById(int id)
        {
            var domainItem = _context.ToDoItems.FirstOrDefault(x => x.Id == id);
            return domainItem is null ? null : MapFromDomain(domainItem);
        }

        public bool Update(ToDoItem item)
        {
            var domainItem = _context.ToDoItems.FirstOrDefault(x => x.Id == item.Id);
            if (domainItem is null)
            {
                return false;
            }

            domainItem.Description = item.Description;
            domainItem.DueDate = item.DueDate;
            domainItem.IsCompleted = item.IsCompleted;
            domainItem.ModifiedDate = DateTime.UtcNow;

            var updateCount = _context.SaveChanges();
            var updateWasSuccesful = updateCount > 0;

            return updateWasSuccesful;
        }

        private ToDoItem MapFromDomain(DomainToDoItem domainItem)
        {
            return new ToDoItem
            {
                Id = domainItem.Id,
                Description = domainItem.Description,
                DueDate = domainItem.DueDate,
                IsCompleted = domainItem.IsCompleted,
            };
        }
    }
}
