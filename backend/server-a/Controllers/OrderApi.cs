using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using server_a.ApiModels;
using RabbitMQ.Client;

namespace server_a.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [ApiController]
    public class OrderApiController : ControllerBase
    {
        /// <summary>
        /// Add an order for an sandwich
        /// </summary>

        /// <param name="order">place an order for a sandwich</param>
        /// <response code="200">successful operation</response>
        /// <response code="400">Order not created</response>
        [HttpPost]
        [Route("/v1/order")]
        [ProducesResponseType(statusCode: 200, type: typeof(Order))]
        public IActionResult AddOrder([FromBody] Order order)
        {
            //TODO: Uncomment the next line to return response 200 or use other options such as return this.NotFound(), return this.BadRequest(..), ...
            // return StatusCode(200, default(Order));

            //TODO: Uncomment the next line to return response 400 or use other options such as return this.NotFound(), return this.BadRequest(..), ...
            // return StatusCode(400);

            return Ok(order);
        }

        /// <summary>
        /// Find an order by its ID
        /// </summary>
        /// <remarks>IDs must be positive integers</remarks>
        /// <param name="orderId">ID of the order that needs to be fetched</param>
        /// <response code="200">successful operation</response>
        /// <response code="400">Invalid ID supplied</response>
        /// <response code="404">Order not found</response>
        [HttpGet]
        [Route("/v1/order/{orderId}")]
        [ProducesResponseType(statusCode: 200, type: typeof(Order))]
        public IActionResult GetOrderById([FromRoute][Required] long? orderId)
        {
            //TODO: Uncomment the next line to return response 200 or use other options such as return this.NotFound(), return this.BadRequest(..), ...
            // return StatusCode(200, default(Order));

            //TODO: Uncomment the next line to return response 400 or use other options such as return this.NotFound(), return this.BadRequest(..), ...
            // return StatusCode(400);

            //TODO: Uncomment the next line to return response 404 or use other options such as return this.NotFound(), return this.BadRequest(..), ...
            // return StatusCode(404);

            return Ok();
        }

        /// <summary>
        /// Get a list of all orders. Empty array if no orders are found.
        /// </summary>

        /// <response code="200">successful operation</response>
        [HttpGet]
        [Route("/v1/order")]
        [ProducesResponseType(statusCode: 200, type: typeof(List<Order>))]
        public IActionResult GetOrders()
        {
            //TODO: Uncomment the next line to return response 200 or use other options such as return this.NotFound(), return this.BadRequest(..), ...
            // return StatusCode(200, default(ArrayOfOrders));

            return Ok();
        }
    }
}
