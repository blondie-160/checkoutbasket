using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BasketApp.Shared;

namespace BasketApp.Client
{
    public class BasketClient : BaseHttpClient
    {
        //for testing
        public BasketClient(string sessionCookie)
        {
            _sessionCookie = sessionCookie;
        }

        public async Task<GetBasketResponse> GetBasketAsync()
        {
            return await GetAsync<GetBasketResponse>($"api/basket");
        }

        public GetBasketResponse GetBasket()
        {
            return GetBasketAsync().Result;
        }

        public async Task AddItemAsync(string productId, int quantity)
        {
            await PostAsync($"api/basket", new BasketItem { ProductId = productId, Quantity = quantity });
        }

        public void AddItem(string productId, int quantity)
        {
            AddItemAsync(productId, quantity).Wait();
        }

        public async Task UpdateBasketAsync(string productId, int quantity)
        {
            await PutAsync($"api/basket", new BasketItem { ProductId = productId, Quantity = quantity });
        }

        public void UpdateBasket(string productId, int quantity)
        {
            UpdateBasketAsync(productId, quantity).Wait();
        }

        public async Task ClearBasketAsync()
        {
            await DeleteAsync($"api/basket");
        }

        public void ClearBasket()
        {
            ClearBasketAsync().Wait();
        }
    }
}
