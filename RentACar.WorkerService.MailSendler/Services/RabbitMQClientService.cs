using RabbitMQ.Client;

namespace RentACar.WorkerService.MailSendler.Services
{
    public class RabbitMQClientService
    {
        private readonly ConnectionFactory _connectionFactory;

        private IConnection _connection;
        private IModel _channel;

        public static string QueueName = "queue-mail";  

        public RabbitMQClientService(ConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public IModel Connect()
        {
            _connection = _connectionFactory.CreateConnection();
            if (_channel is { IsOpen: true })
            {
                return _channel;
            }
            _channel = _connection.CreateModel();
            _channel.QueueDeclare(QueueName, true, false, false, null);
            _channel.QueueBind(exchange: "DirectExchange-mail", queue: QueueName, routingKey: "route-mail");
            return _channel;
        }

        public void Dispose()
        {
            _channel?.Close();
            _channel?.Dispose();
            _connection?.Close();
            _connection?.Dispose();
        }
    }
}
