using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using SmartMailAnalytics.Application.DTOs.RabbitMqDtos;
using SmartMailAnalytics.Application.Interfaces.MailInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SmartMailAnalytics.Application.Services.RabbitMqServices
{
    public class RabbitMqListenerService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;

        public RabbitMqListenerService(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            var connection = factory.CreateConnection();
            var channel = connection.CreateModel();

            channel.QueueDeclare(queue: "spam_response", durable: false, exclusive: false, autoDelete: false);

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var response = JsonSerializer.Deserialize<SpamResponseDto>(Encoding.UTF8.GetString(body));

                // Her mesajda yeni bir scope aç
                using var scope = _scopeFactory.CreateScope();
                var mailRepository = scope.ServiceProvider.GetRequiredService<IMailRepository>();

                await mailRepository.UpdateSpamStatusAsync(response.MailId, response.IsSpam);
                Console.WriteLine($"MailId: {response.MailId} → IsSpam: {response.IsSpam} → DB güncellendi");
            };

            channel.BasicConsume(queue: "spam_response", autoAck: true, consumer: consumer);
            return Task.CompletedTask;
        }
    }
}
