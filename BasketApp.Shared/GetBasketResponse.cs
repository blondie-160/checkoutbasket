using System.Collections.Generic;
using System.Linq;

namespace BasketApp.Shared
{
    public class GetBasketResponse
    {
        public GetBasketResponse()
        {
            Errors = new List<Error>();
        }

        public List<BasketItem> Items { get; set; }

        public bool WasSuccess => !Errors.Any();

        public void AddError(string message) => Errors.Add(new Error { Message = message });

        public List<Error> Errors { get; }
    }
}