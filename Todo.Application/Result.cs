using System.Collections.Generic;
using System.Linq;

namespace Todo.Application
{
    public class Result
    {
        public bool Success => Errors.Any();

        List<string> Errors { get; set; } = new List<string>();

        public int? ItemId { get; set; }
    }
}
