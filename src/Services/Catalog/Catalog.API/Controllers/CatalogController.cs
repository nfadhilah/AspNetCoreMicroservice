﻿using Catalog.API.Entities;
using Catalog.API.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace Catalog.API.Controllers
{
  [ApiController]
  [Route("api/v1/[controller]")]
  public class CatalogController : Controller
  {
    private readonly IProductRepository _repo;
    private readonly ILogger<CatalogController> _logger;

    public CatalogController(IProductRepository repo, ILogger<CatalogController> logger)
    {
      _repo = repo;
      _logger = logger;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<Product>), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
    {
      var products = await _repo.GetProducts();
      return Ok(products);
    }

    [HttpGet("{id:length(24)}", Name = nameof(GetProductById))]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<Product>> GetProductById(string id)
    {
      var product = await _repo.GetProduct(id);
      if (product == null)
      {
        _logger.LogError($"Product with category: {id}, not found.");
        return NotFound();
      }
      return Ok(product);
    }

    [Route("[action]/{category}", Name = nameof(GetProductByCategory))]
    [HttpGet]
    [ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<IEnumerable<Product>>> GetProductByCategory(string category)
    {
      var products = await _repo.GetProductByCategory(category);
      return Ok(products);
    }

    [HttpPost]
    [ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<Product>> CreateProduct([FromBody] Product product)
    {
      await _repo.CreateProduct(product);
      return CreatedAtRoute(nameof(GetProductById), new { id = product.Id }, product);
    }

    [HttpPut]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    public async Task<IActionResult> UpdateProduct([FromBody] Product product)
    {
      return Ok(await _repo.UpdateProduct(product));
    }

    [HttpDelete("{id:length(24)}", Name = nameof(DeleteProductById))]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    public async Task<IActionResult> DeleteProductById(string id)
    {
      return Ok(await _repo.DeleteProduct(id));
    }
  }
}