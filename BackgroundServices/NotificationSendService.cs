
using InventoryManagementSystem.Dao;
using InventoryManagementSystem.Hubs;
using InventoryManagementSystem.Models;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace InventoryManagementSystem.BackgroundServices
{
    public class NotificationSendService : BackgroundService
    {
        private readonly IHubContext<NotificationHub> _hub;
        private readonly IMongoCollection<Produit> _produitCollection;
        public NotificationSendService(IHubContext<NotificationHub> hub, IOptions<StockDbSettings> stockDbSettings)
        {
            _hub = hub;
            var mongoClient = new MongoClient(stockDbSettings.Value.ConnectionString);
            var mongoDatabase = mongoClient.GetDatabase(stockDbSettings.Value.DatabaseName);
            _produitCollection = mongoDatabase.GetCollection<Produit>(stockDbSettings.Value.Collection);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            
                var pipeline = new EmptyPipelineDefinition<ChangeStreamDocument<Produit>>()
                    .Match(change => change.OperationType == ChangeStreamOperationType.Update || change.OperationType == ChangeStreamOperationType.Insert);
                using (var cursor = await _produitCollection.WatchAsync(pipeline,cancellationToken: stoppingToken))
                {
                    await cursor.ForEachAsync(async change =>
                    {
                        var result = await _produitCollection.Find(p => p.stock <= 10).ToListAsync(stoppingToken);
                        await _hub.Clients.All.SendAsync("Alerte_stock",result);

                    });
                }
            
        }
    }
}
