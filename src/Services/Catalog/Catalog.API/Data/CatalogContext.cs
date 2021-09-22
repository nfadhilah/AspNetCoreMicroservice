using Catalog.API.Entities;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Catalog.API.Data
{
  public class CatalogContext : ICatalogContext
  {
    public CatalogContext(IOptions<DatabaseSettings> options)
    {
      var dbSettings = options.Value;
      var client = new MongoClient(dbSettings.ConnectionString);
      var database = client.GetDatabase(dbSettings.DatabaseName);
      Products = database.GetCollection<Product>(dbSettings.CollectionName);
      CatalogContextSeed.SeedData(Products);

    }

    public IMongoCollection<Product> Products { get; }
  }
}
