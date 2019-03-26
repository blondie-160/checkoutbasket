using System;

namespace BasketApp.Shared
{
    public class BasketItem
    {
        public string ProductId { get; set; }
        public int Quantity { get; set; }
        public override bool Equals(object obj)
        {
            var item = obj as BasketItem;
            return item != null &&
                   ProductId.Equals(item.ProductId) &&
                   Quantity == item.Quantity;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(ProductId, Quantity);
        }
    }
}
