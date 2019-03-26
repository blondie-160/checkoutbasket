using System.Collections.Generic;
using System.Linq;

namespace BasketApp.Shared
{
    public class SaveBasketResponse
    {
        public SaveBasketResponse()
        {
            Errors = new List<Error>();
        }

        public bool WasSuccess => !Errors.Any();

        public void AddError(string message) => Errors.Add(new Error { Message = message });

        public override bool Equals(object obj)
        {
            var response = obj as SaveBasketResponse;
            return response != null &&
                   WasSuccess == response.WasSuccess &&
                   EqualityComparer<List<Error>>.Default.Equals(Errors, response.Errors);
        }

        public List<Error> Errors { get; }


    }
}