using MediatR;
using Microsoft.AspNetCore.Mvc;
using Ordering.Application.Features.Orders.Commands.DeleteOrder;
using Ordering.Application.Features.Orders.Queries.GetOrdersList;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ordering.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class OrderController : Controller
    {
        private readonly IMediator mediator;

        public OrderController(IMediator mediator)
        {
            this.mediator = mediator;
        }
        [HttpGet("{userName}")]
        public async Task<ActionResult<IEnumerable<OrdersViewModel>>> GetOrdersByUserName(string userName)
        {
            var query = new GetOrdersListQuery() { UserName = userName };
            var orders = await mediator.Send(query);
            return Ok(orders);
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteOrder(int id)
        {
            var command = new DeleteOrderCommand() { Id = id };
            await mediator.Send(command);
            return NoContent();
        }
    }
}
