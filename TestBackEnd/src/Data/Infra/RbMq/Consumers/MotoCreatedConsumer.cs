using System.Data;

namespace TestBackEnd.src.Data.Infra.RbMq.Consumers
{
    public class MotoCreatedConsumer(IMongoDatabase mongoDatabase) : IConsumer<MotoCreated>
    {
        private readonly IMongoCollection<BsonDocument> _mongoCollection = mongoDatabase.GetCollection<BsonDocument>("MotoRegistered");

        public async Task Consume(ConsumeContext<MotoCreated> context)
        {
            var moto = context.Message;

            if (moto.Year == 2024)
            {
                var document = new BsonDocument
                {
                    { "LicensePlate", moto.LicensePlate },
                    { "Year", moto.Year },
                    { "Timestamp", DateTime.UtcNow }
                };

                await _mongoCollection.InsertOneAsync(document);
                Console.WriteLine($"Moto with Plate '{moto.LicensePlate}' saved to MongoDB.");
            }
            else
            {
                throw new Exception("TESTE DE ERRO NA EXCHANGE");
            }
        }
    }
}
