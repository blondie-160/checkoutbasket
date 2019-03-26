using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BasketApp.Domain.Models;
using BasketApp.Infrastructure;
using BasketApp.Shared;
using Microsoft.IdentityModel.Logging;
using Polly;
using Remotion.Linq.Utilities;

namespace BasketApp.Domain
{
    public interface IBasketService
    {
        Task<GetBasketResponse> GetContents(Guid sessionId);
        Task<SaveBasketResponse> Save(Guid sessionId, BasketItem item);
        Task<DeleteBasketResponse> Clear(Guid sessionId);
    }

    public class BasketService : IBasketService
    {
        private readonly IBasketStore _basketStore;
        private readonly IPollyProvider _pollyPolicyProvider;

        public BasketService(IBasketStore basketStore, IPollyProvider pollyPolicyProvider)
        {
            _basketStore = basketStore;
            _pollyPolicyProvider = pollyPolicyProvider;
        }

        public async Task<GetBasketResponse> GetContents(Guid sessionId)
        {
            var policy = _pollyPolicyProvider.BasketStorePolicy();

            var result = await policy.ExecuteAndCaptureAsync(async () =>
            {
                var items = await _basketStore.GetAll(sessionId);
                return items.Select(model => new BasketItem { ProductId = model.ProductId.ToString(), Quantity = model.Quantity }).ToList();
            });

            if (result.Outcome == OutcomeType.Failure)
            {
                //LOG 
                var response = new GetBasketResponse();
                response.AddError(result.FinalException.Message);
                return response;
            }
            return new GetBasketResponse { Items = result.Result };
        }


        public async Task<SaveBasketResponse> Save(Guid sessionId, BasketItem item)
        {
            var policy = _pollyPolicyProvider.BasketStorePolicy();

            var result = await policy.ExecuteAndCaptureAsync(async () =>
            {
                await _basketStore.SaveOrUpdate(new BasketItemModel
                {
                    ProductId = Guid.Parse(item.ProductId),
                    Quantity = item.Quantity,
                    SessionId = sessionId
                });
            });

            if (result.Outcome == OutcomeType.Failure)
            {
                //LOG 
                var response = new SaveBasketResponse();
                response.AddError(result.FinalException.Message);
                return response;
            }
            return new SaveBasketResponse();
        }

        public async Task<DeleteBasketResponse> Clear(Guid sessionId)
        {
            var policy = _pollyPolicyProvider.BasketStorePolicy();

            var result = await policy.ExecuteAndCaptureAsync(async () =>
            {
                await _basketStore.Delete(sessionId);
            });

            if (result.Outcome == OutcomeType.Failure)
            {
                //LOG 
                var response = new DeleteBasketResponse();
                response.AddError(result.FinalException.Message);
                return response;
            }
            return new DeleteBasketResponse();
        }
    }
}
