using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BasketApp.Domain.Models;

namespace BasketApp.Domain
{
    public interface IBasketStore
    {
        Task<List<BasketItemModel>> GetAll(Guid sessionId);
        Task SaveOrUpdate(BasketItemModel basketModel);
        Task Delete(Guid sessionId);
    }

    //Completley stubbed for very basic use!
    public class BasketStore : IBasketStore
    {
        public async Task<List<BasketItemModel>> GetAll(Guid sessionId)
        {
            return DataStore.Where(x => x.SessionId == sessionId).ToList();
        }

        public async Task SaveOrUpdate(BasketItemModel basketModel)
        {
            DataStore.RemoveAll(x => x.ProductId == basketModel.ProductId && x.SessionId == basketModel.SessionId);
            if (basketModel.Quantity != 0)
                DataStore.Add(basketModel);
        }

        public async Task Delete(Guid sessionId)
        {
            DataStore.RemoveAll(x => x.SessionId == sessionId);
        }

        public List<BasketItemModel> DataStore = new List<BasketItemModel>();
    }
}
