
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace InventoryManagementSystem.Dao
{
    public class Produit
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }
        public string name { get; set; } = null;
        public int stock { get; set; }
        public Supplier suppliers { get; set; } = null;
        [BsonRepresentation(BsonType.Decimal128)]
        public float unit_price { get; set; } 
        public string category { get; set; } = null;

    }
}
