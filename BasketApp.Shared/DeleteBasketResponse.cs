using System.Collections.Generic;
using System.Linq;

namespace BasketApp.Shared
{
    public class DeleteBasketResponse
    {
        public DeleteBasketResponse()
        {
            Errors = new List<Error>();
        }

        public bool WasSuccess => !Errors.Any();

        public void AddError(string message) => Errors.Add(new Error { Message = message });

        public List<Error> Errors { get; }
    }
}