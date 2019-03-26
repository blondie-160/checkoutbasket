using BasketApp.Controllers;
using System;
using System.Collections.Generic;
using System.Net;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using BasketApp.Domain;
using BasketApp.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Moq;
using NSubstitute;
using Xunit;

namespace BasketApp.Tests.Unit
{
    public class ControllerTests
    {
        private BasketController _sut;


        [Fact]
        public async void Get_should_return_OK_result()
        {
            var basketService = new Mock<IBasketService>();
            basketService.Setup(x => x.GetContents(It.IsAny<Guid>())).Returns(Task.FromResult(new GetBasketResponse { Items = new List<BasketItem>() }));

            _sut = new BasketController(basketService.Object);
            SetupCookies();
            var result = await _sut.Get();

            var expectedResult = new GetBasketResponse { Items = new List<BasketItem>() };
            var okObjectResult = result as OkObjectResult;
            Assert.Equal(expectedResult, okObjectResult.Value as GetBasketResponse);
        }

        [Fact]
        public async void Get_should_return_bad_request()
        {
            var basketService = new Mock<IBasketService>();
            basketService.Setup(x => x.GetContents(It.IsAny<Guid>())).Throws(new Exception());
            _sut = new BasketController(basketService.Object);
            SetupCookies();
            var result = _sut.Get();

            Assert.IsType<BadRequestObjectResult>(result.Result);
        }

        [Fact]
        public async void Put_should_return_OK_result()
        {
            var basketService = new Mock<IBasketService>();
            _sut = new BasketController(basketService.Object);
            SetupCookies();
            var result = await _sut.Put(new BasketItem());

            var expectedResult = new SaveBasketResponse();
            var okObjectResult = result as OkObjectResult;
            Assert.Equal(expectedResult, okObjectResult.Value as SaveBasketResponse);
        }

        [Fact]
        public async void Put_should_return_bad_request()
        {
            var basketService = new Mock<IBasketService>();
            basketService.Setup(x => x.Save(It.IsAny<Guid>(), It.IsAny<BasketItem>())).Throws(new Exception());
            _sut = new BasketController(basketService.Object);
            SetupCookies();

            var result = _sut.Put(new BasketItem());

            Assert.IsType<BadRequestObjectResult>(result.Result);
        }

        [Fact]
        public async void Post_should_return_created_result()
        {
            var basketService = new Mock<IBasketService>();
            basketService.Setup(x => x.Save(It.IsAny<Guid>(), It.IsAny<BasketItem>()))
                .Returns(Task.FromResult(new SaveBasketResponse()));
            _sut = new BasketController(basketService.Object);
            SetupCookies();

            var result = await _sut.Post(new BasketItem());

            var expectedResult = new SaveBasketResponse();
            var okObjectResult = result as OkObjectResult;
            Assert.Equal(expectedResult, okObjectResult.Value as SaveBasketResponse);
        }

        [Fact]
        public async void Post_should_return_bad_request()
        {
            var basketService = new Mock<IBasketService>();
            basketService.Setup(x => x.Save(It.IsAny<Guid>(), It.IsAny<BasketItem>())).Throws(new Exception());
            _sut = new BasketController(basketService.Object);
            SetupCookies();

            var result = _sut.Post(new BasketItem());

            Assert.IsType<BadRequestObjectResult>(result.Result);
        }

        [Fact]
        public async void Delete_should_return_OK_result()
        {
            var basketService = new Mock<IBasketService>();
            _sut = new BasketController(basketService.Object);
            SetupCookies();

            var result = await _sut.Delete();

            var expectedResult = new SaveBasketResponse();
            var okObjectResult = result as OkObjectResult;
            Assert.Equal(expectedResult, okObjectResult.Value as SaveBasketResponse);
        }

        [Fact]
        public async void Delete_should_return_bad_request()
        {
            var basketService = new Mock<IBasketService>();
            basketService.Setup(x => x.Clear(It.IsAny<Guid>())).Throws(new Exception());
            _sut = new BasketController(basketService.Object);
            SetupCookies();

            var result = _sut.Delete();

            Assert.IsType<BadRequestObjectResult>(result.Result);
        }

        private void SetupCookies()
        {
            var sessionId = Guid.NewGuid();
            var context = Substitute.For<HttpContext>();
            var request = Substitute.For<HttpRequest>();
            request.Cookies.Returns(
                new RequestCookieCollection(new Dictionary<string, string> { { "Session", sessionId.ToString() } }));
            context.Request.Returns(request);
            var controllerCtx = new ControllerContext();
            controllerCtx.HttpContext = context;
            _sut.ControllerContext = controllerCtx;
        }
    }
}
