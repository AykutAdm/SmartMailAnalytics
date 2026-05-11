using RabbitMQ.Client;
using SmartMailAnalytics.Application.DTOs.RabbitMqDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SmartMailAnalytics.Application.Services.RabbitMqServices
{
    public class RabbitMqPublisher
    {
        private readonly IModel _channel;

        public RabbitMqPublisher()
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            var connection = factory.CreateConnection();
            _channel = connection.CreateModel();
            _channel.QueueDeclare(queue: "spam_request", durable: false, exclusive: false, autoDelete: false);
        }

        public void Publish(SpamRequestDto request)
        {
            var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(request));
            _channel.BasicPublish(exchange: "", routingKey: "spam_request", body: body);
        }
    }
}
