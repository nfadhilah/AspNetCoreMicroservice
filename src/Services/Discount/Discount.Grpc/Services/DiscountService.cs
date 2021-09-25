using System.Threading.Tasks;
using AutoMapper;
using Discount.Grpc.Entities;
using Discount.Grpc.Protos;
using Discount.Grpc.Repositories;
using Grpc.Core;
using Microsoft.Extensions.Logging;

namespace Discount.Grpc.Services
{
  public class DiscountService : DiscountProtoService.DiscountProtoServiceBase
  {
    private readonly ILogger<DiscountService> _logger;
    private readonly IDiscountRepository _repo;
    private readonly IMapper _mapper;

    public DiscountService(ILogger<DiscountService> logger, IDiscountRepository repo, IMapper mapper)
    {
      _logger = logger;
      _repo = repo;
      _mapper = mapper;
    }

    public override async Task<CouponModel> GetDiscount(GetDiscountRequest request, ServerCallContext context)
    {
      var coupon = await _repo.GetDiscount(request.ProductName);
      if (coupon == null)
        throw new RpcException(new Status(StatusCode.NotFound,
          $"Discount with Product Name = {request.ProductName} is not found."));
      _logger.LogInformation($"Discount is retrieved for ProductName: {coupon.ProductName}, Amount: {coupon.Amount}",
        coupon.ProductName, coupon.Amount);
      var couponModel = _mapper.Map<CouponModel>(coupon);
      return couponModel;
    }

    public override async Task<CouponModel> CreateDiscount(CreateDiscountRequest request, ServerCallContext context)
    {
      var coupon = _mapper.Map<Coupon>(request.Coupon);
      await _repo.CreateDiscount(coupon);
      _logger.LogInformation($"Discount is successfully created. ProductName: {coupon.ProductName}",
        coupon.ProductName);
      return _mapper.Map<CouponModel>(coupon);
    }

    public override async Task<CouponModel> UpdateDiscount(UpdateDiscountRequest request, ServerCallContext context)
    {
      var coupon = _mapper.Map<Coupon>(request.Coupon);
      await _repo.UpdateDiscount(coupon);
      _logger.LogInformation($"Discount is successfully updated. ProductName: {coupon.ProductName}",
        coupon.ProductName);
      return _mapper.Map<CouponModel>(coupon);
    }

    public override async Task<DeleteDiscountResponse> DeleteDiscount(DeleteDiscountRequest request,
      ServerCallContext context)
    {
      var deleted = await _repo.DeleteDiscount(request.ProductName);
      if (deleted)
        _logger.LogInformation($"Discount is successfully deleted. ProductName: {request.ProductName}",
          request.ProductName);
      return new DeleteDiscountResponse {Success = deleted};
    }
  }
}