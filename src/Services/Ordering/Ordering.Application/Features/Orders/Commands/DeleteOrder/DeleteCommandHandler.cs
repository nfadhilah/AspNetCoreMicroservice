using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.Application.Contracts.Persistence;
using Ordering.Application.Exceptions;
using Ordering.Domain.Entities;

namespace Ordering.Application.Features.Orders.Commands.DeleteOrder
{
  public class DeleteCommandHandler : IRequestHandler<DeleteOrderCommand>
  {
    private readonly IOrderRepository _orderRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<DeleteCommandHandler> _logger;

    public DeleteCommandHandler(IOrderRepository orderRepository, IMapper mapper, ILogger<DeleteCommandHandler> logger)
    {
      _orderRepository = orderRepository;
      _mapper = mapper;
      _logger = logger;
    }

    public async Task<Unit> Handle(DeleteOrderCommand request, CancellationToken cancellationToken)
    {
      var order = await _orderRepository.GetByIdAsync(request.Id);

      if (order == null)
      {
        _logger.LogError("Order not exists on database.");
        throw new NotFoundException(nameof(Order), request.Id);
      }

      await _orderRepository.DeleteAsync(order);
      return Unit.Value;
    }
  }
}