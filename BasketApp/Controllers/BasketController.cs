using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using BasketApp.Domain;
using BasketApp.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters.Internal;

namespace BasketApp.Controllers
{
    [Route("api/basket")]
    [ApiController]
    public class BasketController : ControllerBase
    {
        private readonly IBasketService _basketService;

        public BasketController(IBasketService basketService)
        {
            _basketService = basketService;
        }

        // GET api/values
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var sessionId = Guid.Parse(Request.Cookies["Session"]);
            try
            {
                var response = await _basketService.GetContents(sessionId);
                return Ok(response);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // POST api/values
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] BasketItem value)
        {
            var sessionId = Guid.Parse(Request.Cookies["Session"]);
            try
            {
                var response = await _basketService.Save(sessionId, value);
                return Ok(response);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

        }

        // PUT api/values/5
        [HttpPut]
        public async Task<IActionResult> Put([FromBody] BasketItem value)
        {
            var sessionId = Guid.Parse(Request.Cookies["Session"]);
            try
            {
                var response = await _basketService.Save(sessionId, value);
                return Ok(response);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // DELETE api/values/5
        [HttpDelete]
        public async Task<IActionResult> Delete()
        {
            var sessionId = Guid.Parse(Request.Cookies["Session"]);
            try
            {
                var response = await _basketService.Clear(sessionId);
                return Ok(response);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
