using InventoryManagementSystem.Dao;
using InventoryManagementSystem.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;

namespace InventoryManagementSystem.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class StockManagementController : ControllerBase
    {   private readonly IMongoCollection<Produit> _produitCollection;
        public StockManagementController(IOptions<StockDbSettings> stockDbSettings)
        {   
            var mongoClient = new MongoClient(stockDbSettings.Value.ConnectionString);
            var mongoDatabase = mongoClient.GetDatabase(stockDbSettings.Value.DatabaseName);
            _produitCollection = mongoDatabase.GetCollection<Produit>(stockDbSettings.Value.Collection);
        }

        [HttpGet]
        public IActionResult GetProduits()
        {
            var filter = Builders<Produit>.Filter.Empty;
            var result = _produitCollection.Find(filter).ToList();

            return Ok(result);
        }

        [HttpGet("{nom}")]
        public IActionResult GetProduits(string nom)
        {   nom = nom.Replace("%20", "");
            var filter = Builders<Produit>.Filter.Eq("name", nom);
            var result = _produitCollection.Find<Produit>(filter).FirstOrDefault();

            return Ok(result);
        }
        [HttpPost]
        public IActionResult AjouterProduit([FromBody]Produit p)
        {
            _produitCollection.InsertOne(p);
            return Ok(p);
        }
        [HttpPut("{nom}")]
        public IActionResult ModifierProduit(string nom,Produit p)
        {
            var filter = Builders<Produit>.Filter.Eq("name", nom);
            var result = _produitCollection.Find<Produit>(filter).FirstOrDefault();
            p.Id = result.Id;
            _produitCollection.ReplaceOne(x=> x.name == nom, p);
            return NoContent();
        }
        [HttpDelete("{nom}")]
        public IActionResult SupprimerProduit(string nom)
        {
            _produitCollection.DeleteOne(x=> x.name == nom);
            return NoContent();
        }
    }
}
