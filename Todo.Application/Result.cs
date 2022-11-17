using System.Collections.Generic;
using System.Linq;

namespace Todo.Application
{
    public class Result
    {
        private List<string> _errors = new List<string>();

        public bool Success => !_errors.Any();

        public int? ItemId { get; }

        public Result()
        {

        }
        public Result(int? itemId)
        {
            ItemId = itemId;
        }

        public void AddError(string errorMessage)
        {
            _errors.Add(errorMessage);
        }

        public List<string> GetErrors() => _errors.ToList();
    }
}