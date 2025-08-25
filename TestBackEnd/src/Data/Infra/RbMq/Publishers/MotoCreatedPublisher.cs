namespace TestBackEnd.src.Data.Infra.RbMq.Publishers
{
    public class MotoCreatedPublisher(IPublishEndpoint publishEndpoint)
    {
        private readonly IPublishEndpoint _publishEndpoint = publishEndpoint;

        public async Task PublishMotoCreatedEvent(MotoCreated moto)
        {
            await _publishEndpoint.Publish(moto);
            Console.WriteLine("MotoCreated event published");
        }
    }
}
