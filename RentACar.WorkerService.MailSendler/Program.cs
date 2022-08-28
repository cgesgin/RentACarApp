using RabbitMQ.Client;
using RentACar.WorkerService.MailSendler;
using RentACar.WorkerService.MailSendler.Services;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {   
        IConfiguration Configuration = hostContext.Configuration;
        services.AddSingleton(sp => new ConnectionFactory() { Uri = new Uri(Configuration.GetConnectionString("RabbitMQ")), DispatchConsumersAsync = true });
        services.AddSingleton<RabbitMQClientService>();
        
        services.AddHostedService<Worker>();
    })
    .Build();

await host.RunAsync();