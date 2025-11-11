namespace InventoryManagementSystem.Models
{
    public class StockDbSettings
    {
        public string ConnectionString { get; set; } = null!;
        public string DatabaseName { get; set; } = null!;
        public string Collection {  get; set; } = null!;
    }
}
