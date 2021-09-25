using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Ordering.Application.Features.Orders.Commands.CheckoutOrder;
using Ordering.Application.Features.Orders.Commands.DeleteOrder;
using Ordering.Application.Features.Orders.Commands.UpdateOrder;
using Ordering.Application.Features.Orders.Queries.GetOrdersList;

namespace Ordering.API.Controllers
{
  [ApiController]
  [Route("api/v1/[controller]")]
  public class OrderController : Controller
  {
    private readonly IMediator _mediator;

    public OrderController(IMediator mediator)
    {
      _mediator = mediator;
    }

    [HttpGet("{userName}", Name = nameof(GetOrderByUserName))]
    [ProducesResponseType(typeof(IEnumerable<OrderDTO>), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<IEnumerable<OrderDTO>>> GetOrderByUserName(string userName)
    {
      var orders = await _mediator.Send(new GetOrderListQuery(userName));
      return Ok(orders);
    }

    [HttpPost(Name = nameof(CheckoutOrder))]
    [ProducesResponseType((int) HttpStatusCode.OK)]
    public async Task<ActionResult<int>> CheckoutOrder([FromBody] CheckoutOrderCommand command)
    {
      var result = await _mediator.Send(command);
      return Ok(result);
    }

    [HttpPut(Name = nameof(UpdateOrder))]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateOrder([FromBody] UpdateOrderCommand command)
    {
      await _mediator.Send(command);
      return NoContent();
    }

    [HttpDelete("{id}", Name = nameof(DeleteOrder))]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteOrder(int id)
    {
      await _mediator.Send(new DeleteOrderCommand(id));
      return NoContent();
    }
  }
}