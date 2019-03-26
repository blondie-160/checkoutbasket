using System;

namespace BasketApp.Domain.Models
{
    public class BasketItemModel
    {
        public Guid SessionId { get; set; }
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }

        public override bool Equals(object obj)
        {
            var model = obj as BasketItemModel;
            return model != null &&
                   SessionId.Equals(model.SessionId) &&
                   ProductId.Equals(model.ProductId) &&
                   Quantity == model.Quantity;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine<Guid, Guid, int>(SessionId, ProductId, Quantity);
        }
    }
}