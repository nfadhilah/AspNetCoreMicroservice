using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Ordering.Application.Contracts.Persistence;

namespace Ordering.Application.Features.Orders.Queries.GetOrdersList
{
  public class GetOrderListQueryHandler : IRequestHandler<GetOrderListQuery, List<OrderDTO>>
  {
    private readonly IOrderRepository _orderRepository;
    private readonly IMapper _mapper;

    public GetOrderListQueryHandler(IOrderRepository orderRepository, IMapper mapper)
    {
      _orderRepository = orderRepository;
      _mapper = mapper;
    }

    public async Task<List<OrderDTO>> Handle(GetOrderListQuery request, CancellationToken cancellationToken)
    {
      var orderList = await _orderRepository.GetOrdersByUserName(request.UserName);
      return _mapper.Map<List<OrderDTO>>(orderList);
    }
  }
}