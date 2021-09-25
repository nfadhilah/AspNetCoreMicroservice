using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Discount.API.Entities;
using Discount.API.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Discount.API.Controllers
{
  [ApiController]
  [Route("api/v1/[controller]")]
  public class DiscountController : Controller
  {
    private readonly IDiscountRepository _repo;

    public DiscountController(IDiscountRepository repo)
    {
      _repo = repo;
    }

    [HttpGet("{productName}", Name = nameof(GetDiscount))]
    [ProducesResponseType(typeof(Coupon), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<Coupon>> GetDiscount(string productName)
    {
      var coupon = await _repo.GetDiscount(productName);

      return Ok(coupon);
    }

    [HttpPost]
    [ProducesResponseType(typeof(Coupon), (int) HttpStatusCode.OK)]
    public async Task<ActionResult<Coupon>> CreateDiscount([FromBody] Coupon coupon)
    {
      await _repo.CreateDiscount(coupon);
      return CreatedAtRoute(nameof(GetDiscount), new {productName = coupon.ProductName}, coupon);
    }

    [HttpPut]
    [ProducesResponseType(typeof(Coupon), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<Coupon>> UpdateDiscount([FromBody] Coupon coupon)
    {
      await _repo.UpdateDiscount(coupon);
      return Ok(coupon);
    }

    [HttpDelete("{productName}", Name = nameof(DeleteDiscount))]
    [ProducesResponseType(typeof(void), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<Coupon>> DeleteDiscount(string productName)
    {
      await _repo.DeleteDiscount(productName);
      return Ok();
    }
  }
}