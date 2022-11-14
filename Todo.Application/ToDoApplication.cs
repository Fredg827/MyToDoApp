using System;
using System.Collections.Generic;
using Todo.Application.Commands;
using ToDoApp.Domain;

namespace Todo.Application
{
    public class ToDoApplication : ITodoApplication
    {

        private readonly ITodoRepository _todoRepository;

        public ToDoApplication(ITodoRepository todoRepository)
        {
            _todoRepository = todoRepository;
        }

        public Result Create(CreateToDoItemCommand createToDoItem)
        {
            //validate that description is not empty
            var result = ValidateDescription(createToDoItem.Description);
            if (result.Success == false)
            {
                //if so, return result with error 
                return result;
            }

            //if not, create new to do item out of the create command 
            var toDoItem = new ToDoItem
            {
                Description = createToDoItem.Description,
                DueDate = createToDoItem.DueDate
            };

            //call to do repository with the new to do item 
            var newId = _todoRepository.Create(toDoItem);

            //if id is not null => new Result(id)
            if (newId is not null)
            {
               return new Result(newId);
            }
            // else, add error - could not create item 
            result.AddError("Could not save item");
            return result;
        }

        public Result Delete(int id)
        {
            var result = new Result(id);
            var toDoItem = _todoRepository.GetById(id);

            if(toDoItem is null)
            {
                result.AddError($"Could not find item with ID {id}");
                return result;
            }
            if (_todoRepository.Delete(id) == false)
            {
                result.AddError($"Could not delete item with {id}");
            }
            return result;
        }

        public List<ToDoItem> GetAll() //*
        {
            return _todoRepository.GetAll();
        }

        public ToDoItem GetById(int id) //*
        {
            return _todoRepository.GetById(id);
        }

        public Result Update(UpdateToDoItemCommand updateToDoItem)
        {
            var result = ValidateDescription(updateToDoItem.Description, updateToDoItem.Id);
            if (result.Success == false)
            {
                return result;
            }
            var toDoItem = _todoRepository.GetById(updateToDoItem.Id);

            if (toDoItem is null)
            {
                result.AddError($"Could not find item with ID {updateToDoItem.Id}");
                return result;
            }

            toDoItem.Description = updateToDoItem.Description;
            toDoItem.DueDate = updateToDoItem.DueDate;
            toDoItem.IsCompleted = updateToDoItem.IsCompleted;

           var isUpdated = _todoRepository.Update(toDoItem);
            if (isUpdated == false)
            {
                result.AddError("Could not update");
            }
            return result;
        }

        private static Result ValidateDescription(string description, int? id = null)
        {
            var result = new Result(id);

            if (string.IsNullOrEmpty(description))
            {
                result.AddError("Description cannot be empty");
            }
            return result;
        }
    }
}