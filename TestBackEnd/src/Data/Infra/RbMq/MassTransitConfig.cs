namespace TestBackEnd.src.Data.Infra.RbMq
{
    public static class AddMassTransitRabbitMq
    {
        public static void AddMassTransitWithRabbitMq(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddMassTransit(x =>
            {
                x.AddConsumer<MotoCreatedConsumer>();

                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host(configuration["RabbitMq:HostName"], h =>
                    {
                        h.Username(configuration["RabbitMq:UserName"]);
                        h.Password(configuration["RabbitMq:Password"]);
                    });

                    cfg.ReceiveEndpoint("moto_cadastrada", ep =>
                    {
                        ep.ConfigureConsumer<MotoCreatedConsumer>(context);
                    });
                });
            });
        }
    }
}
