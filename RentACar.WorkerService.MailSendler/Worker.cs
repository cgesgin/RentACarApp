using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RentACar.RabbitMQ;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Text.Json;
using RabbitMQClientService = RentACar.WorkerService.MailSendler.Services.RabbitMQClientService;

namespace RentACar.WorkerService.MailSendler
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly RabbitMQClientService _rabbitMQClientService;

        private IModel channel;

        public Worker(ILogger<Worker> logger, RabbitMQClientService rabbitMQClientService)
        {
            _logger = logger;
            _rabbitMQClientService = rabbitMQClientService;
        }
        public override Task StartAsync(CancellationToken cancellationToken)
        {
            channel = _rabbitMQClientService.Connect();
            channel.BasicQos(0, 1, false);

            return base.StartAsync(cancellationToken);
        }
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var consumer = new AsyncEventingBasicConsumer(channel);

            channel.BasicConsume(RabbitMQClientService.QueueName, false, consumer);

            consumer.Received += Consumer_Received;

            return Task.CompletedTask;
        }

        private Task Consumer_Received(object sender, BasicDeliverEventArgs @event)
        {
            var result = JsonSerializer.Deserialize<CustomerMailSendedForRentalEvent>
                (Encoding.UTF8.GetString(@event.Body.ToArray()));

            string message = GetHtmlTemplate(result);
            string subject = "Rent A Car";
            string from = "cevhergesgin@gmail.com";
            string fromPassword = "ladddmwzwfghfayu";
            string to = result.CostumerEmail;            

            MailSendler(from, fromPassword, to, subject, message);

            channel.BasicAck(@event.DeliveryTag, false);

            _logger.LogInformation("Success : sent to " + result.CostumerEmail);

            return Task.CompletedTask;
        }

        private Task MailSendler(string fromMail, string fromPassword, string to, string subject, string mailMessage)
        {
            MailMessage message = new MailMessage();
            message.From = new MailAddress(fromMail);
            message.Subject = subject;
            message.To.Add(new MailAddress(to));
            message.Body = mailMessage;
            message.IsBodyHtml = true;

            var smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential(fromMail, fromPassword),
                EnableSsl = true,
            };

            smtpClient.Send(message);

            return Task.CompletedTask;
        }

        public string GetHtmlTemplate(CustomerMailSendedForRentalEvent data)
        {
            string html = $"<html>" +
                $"<body>" +
                $"<table class=\"table table-striped\">" +
                $"<thead>" +
                $" <tr>" +
                $"<td>Brand</td>" +
                $"<td>Model</td>" +
                $"<td>Rental Store</td>" +
                $"<td>Drop Store</td>" +
                $"<td>Amount</td>" +
                $"<td>Costumer name</td>" +
                $"<td>Costumer Lastname</td>" +
                $"<td>Costumer email</td>" +
                $"</tr>" +
                $"</thead> " +
                $"<tbody>" +
                $" <tr>" +
                $"<td>{data.CarBrand}</td>" +
                $"<td>{data.CarModel}</td>" +
                $"<td>{data.RentalStores}</td>" +
                $"<td>{data.DropStore}</td>" +
                $"<td>{data.Amount}</td>" +
                $"<td>{data.CostumerName}</td>" +
                $"<td>{data.CostumerLastName}</td>" +
                $"<td>{data.CostumerEmail}</td>" +
                $"</tr>" +
                $"</tbody>" +
                $"</table>" +
                $"</body>" +
                $"</html>";
            return html;
        }
    }
}