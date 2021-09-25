using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using EventBus.Messages.Events;
using Ordering.Application.Features.Orders.Commands.CheckoutOrder;

namespace Ordering.API.Mapper
{
  public class OrderProfile : Profile
  {
    public OrderProfile()
    {
      CreateMap<BasketCheckoutEvent, CheckoutOrderCommand>().ReverseMap();
    }
  }
}