using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using server_a.ApiModels;
using RabbitMQ.Client;
using System.Text.Json;
using System.Text;
using server_a.Services;

namespace server_a.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [ApiController]
    public class OrderController(ConnectionFactory rabbitFactory, OrdersCollection orders) 
        : ControllerBase
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
            var orderId = orders.LastOrDefault()?.Id ?? 0;
            order.Id = orderId + 1;

            using var rabbitConnection = rabbitFactory.CreateConnection();
            using var channel = rabbitConnection.CreateModel();

            channel.ExchangeDeclare("orders", ExchangeType.Direct);
            channel.QueueDeclare("orders", false, false, false, null);
            channel.QueueBind("orders", "orders", "order");

            var message = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(order));
            channel.BasicPublish("orders", "order", null, message);

            orders.Add(order);

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
            var order = orders.FirstOrDefault(o => o.Id == orderId);
            if (order == null)
            {
                return NotFound();
            }

            return Ok(order);
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
            return Ok(orders);
        }
    }
}
