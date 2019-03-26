using System;
using System.Linq;
using Xunit;

namespace BasketApp.Client.Tests
{
    public class BasketClientTests
    {
        [Fact]
        public async void When_posting_to_client_the_basket_should_be_updated()
        {
            //setup client
            var userGuid = Guid.NewGuid();
            var client = new BasketClient(userGuid.ToString());

            //add product
            var productId = Guid.NewGuid();
            await client.AddItemAsync(productId.ToString(), 2);

            //assert items match
            var basketResponse = await client.GetBasketAsync();
            Assert.Single(basketResponse.Items);
            Assert.Equal(2, basketResponse.Items[0].Quantity);

            //clear basket
            await client.ClearBasketAsync();

            //assert basket is clear
            basketResponse = await client.GetBasketAsync();
            Assert.Empty(basketResponse.Items);

            //add multiple items
            var secondProductId = Guid.NewGuid();
            await client.AddItemAsync(secondProductId.ToString(), 7);
            var thirdProductId = Guid.NewGuid();
            await client.AddItemAsync(thirdProductId.ToString(), 1);

            //assert items match
            basketResponse = await client.GetBasketAsync();
            Assert.Equal(7, basketResponse.Items.FirstOrDefault(x => x.ProductId == secondProductId.ToString()).Quantity);
            Assert.Equal(1, basketResponse.Items.FirstOrDefault(x => x.ProductId == thirdProductId.ToString()).Quantity);

            //clear basket
            await client.ClearBasketAsync();

            //assert basket is clear
            basketResponse = await client.GetBasketAsync();
            Assert.Empty(basketResponse.Items);
        }
    }
}
