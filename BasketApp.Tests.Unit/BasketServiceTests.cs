using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BasketApp.Domain;
using BasketApp.Domain.Models;
using BasketApp.Infrastructure;
using BasketApp.Shared;
using Moq;
using NSubstitute;
using Polly;
using Xunit;

namespace BasketApp.Tests.Unit
{
    public class BasketServiceTests
    {
        [Fact]
        public async void Getting_basket_should_return_contents()
        {
            var sessionId = Guid.NewGuid();
            var productId = Guid.NewGuid();

            var dbBasketItems = new List<BasketItemModel>
            {
                new BasketItemModel {SessionId = sessionId, ProductId = productId, Quantity = 2}
            };

            var basketRepo = new Mock<IBasketStore>();
            basketRepo.Setup(x => x.GetAll(sessionId)).Returns(Task.FromResult(dbBasketItems));

            var sut = new BasketService(basketRepo.Object, new PollyProvider());
            var result = await sut.GetContents(sessionId);

            var expectedItem = new BasketItem { ProductId = productId.ToString(), Quantity = 2 };
            Assert.Equal(result.Items.First(), expectedItem);
            Assert.True(result.WasSuccess);
        }

        [Fact]
        public async void Getting_basket_should_return_contents_on_retry()
        {
            var sessionId = Guid.NewGuid();
            var productId = Guid.NewGuid();

            var dbBasketItems = new List<BasketItemModel>
            {
                new BasketItemModel {SessionId = sessionId, ProductId = productId, Quantity = 2}
            };

            var basketRepo = new Mock<IBasketStore>();
            basketRepo.SetupSequence(x => x.GetAll(sessionId))
                .Throws(new Exception())
                .Returns(Task.FromResult(dbBasketItems));

            var sut = new BasketService(basketRepo.Object, new PollyProvider());
            var result = await sut.GetContents(sessionId);

            var expectedItem = new BasketItem { ProductId = productId.ToString(), Quantity = 2 };
            Assert.Equal(result.Items.First(), expectedItem);
            Assert.True(result.WasSuccess);
        }

        [Fact]
        public async void Getting_basket_should_return_error_on_end_of_retry()
        {
            var sessionId = Guid.NewGuid();

            var basketRepo = new Mock<IBasketStore>();
            basketRepo.SetupSequence(x => x.GetAll(sessionId))
                .Throws(new Exception())
                .Throws(new Exception())
                .Throws(new Exception())
                .Throws(new Exception("Test message"));

            var sut = new BasketService(basketRepo.Object, new PollyProvider());
            var result = await sut.GetContents(sessionId);

            Assert.False(result.WasSuccess);
            Assert.Equal("Test message", result.Errors.First().Message);
        }

        [Fact]
        public async void Adding_an_item_to_basket_should_save_changes()
        {
            var sessionId = Guid.NewGuid();
            var productId = Guid.NewGuid();

            var basketRepo = new Mock<IBasketStore>();
            basketRepo.Setup(x => x.SaveOrUpdate(It.IsAny<BasketItemModel>())).Returns(Task.CompletedTask);

            var _sut = new BasketService(basketRepo.Object, new PollyProvider());
            var result = await _sut.Save(sessionId, new BasketItem { ProductId = productId.ToString(), Quantity = 3 });

            Assert.True(result.WasSuccess);
        }

        [Fact]
        public async void Adding_an_item_to_basket_should_save_changes_on_retry()
        {
            var sessionId = Guid.NewGuid();
            var productId = Guid.NewGuid();

            var basketRepo = new Mock<IBasketStore>();
            basketRepo.SetupSequence(x => x.SaveOrUpdate(It.IsAny<BasketItemModel>())).Throws(new Exception())
                .Returns(Task.CompletedTask);

            var _sut = new BasketService(basketRepo.Object, new PollyProvider());
            var result = await _sut.Save(sessionId, new BasketItem { ProductId = productId.ToString(), Quantity = 3 });

            basketRepo.Verify(
                x => x.SaveOrUpdate(new BasketItemModel() { ProductId = productId, Quantity = 3, SessionId = sessionId }),
                Times.Exactly(2));
            Assert.True(result.WasSuccess);
        }

        [Fact]
        public async void Adding_an_item_to_basket_should_return_error_on_final_retry()
        {
            var sessionId = Guid.NewGuid();
            var productId = Guid.NewGuid();

            var basketRepo = new Mock<IBasketStore>();
            basketRepo.SetupSequence(x => x.SaveOrUpdate(It.IsAny<BasketItemModel>()))
                .Throws(new Exception())
                .Throws(new Exception())
                .Throws(new Exception())
                .Throws(new Exception("Test message"));

            var _sut = new BasketService(basketRepo.Object, new PollyProvider());
            var result = await _sut.Save(sessionId, new BasketItem { ProductId = productId.ToString(), Quantity = 3 });

            Assert.False(result.WasSuccess);
            Assert.Equal("Test message", result.Errors.First().Message);
        }

        [Fact]
        public async void Clearing_a_basket_should_delete_all_contents()
        {
            var sessionId = Guid.NewGuid();
            var basketRepo = new Mock<IBasketStore>();
            basketRepo.Setup(x => x.Delete(It.IsAny<Guid>())).Returns(Task.CompletedTask);

            var sut = new BasketService(basketRepo.Object, new PollyProvider());
            var result = await sut.Clear(sessionId);

            basketRepo.Verify(x => x.Delete(sessionId));
            Assert.True(result.WasSuccess);
        }

        [Fact]
        public async void Clearing_a_basket_should_delete_all_contents_on_retry()
        {
            var sessionId = Guid.NewGuid();
            var basketRepo = new Mock<IBasketStore>();
            basketRepo.SetupSequence(x => x.Delete(It.IsAny<Guid>())).Throws(new Exception())
                .Returns(Task.CompletedTask);

            var sut = new BasketService(basketRepo.Object, new PollyProvider());
            var result = await sut.Clear(sessionId);

            basketRepo.Verify(x => x.Delete(sessionId), Times.Exactly(2));
            Assert.True(result.WasSuccess);
        }

        [Fact]
        public async void Clearing_a_basket_should_error_on_final_retry()
        {
            var sessionId = Guid.NewGuid();
            var basketRepo = new Mock<IBasketStore>();
            basketRepo.SetupSequence(x => x.Delete(It.IsAny<Guid>()))
                .Throws(new Exception())
                .Throws(new Exception())
                .Throws(new Exception())
                .Throws(new Exception("Test message"));

            var sut = new BasketService(basketRepo.Object, new PollyProvider());
            var result = await sut.Clear(sessionId);

            Assert.False(result.WasSuccess);
            Assert.Equal("Test message", result.Errors.First().Message);
        }
    }
}
