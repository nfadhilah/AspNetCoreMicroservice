using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace Ordering.Application.Features.Orders.Queries.GetOrdersList
{
  public class GetOrderListQuery : IRequest<List<OrderDTO>>
  {
    public GetOrderListQuery(string userName)
    {
      UserName = userName;
    }

    public string UserName { get; set; }

  }
}