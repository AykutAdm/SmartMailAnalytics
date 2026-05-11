using Microsoft.ML;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using SmartMailAnalytics.MLWorker;
using SmartMailAnalytics.MLWorker.DTOs;
using System.Text;
using System.Text.Json;

var csvPath = "spam_train.csv";
var modelPath = "spam_model.zip";

if (!File.Exists(modelPath))
{
    ModelTrainer.Train(csvPath, modelPath);
}

var mlContext = new MLContext();
var model = mlContext.Model.Load(modelPath, out _);
var engine = mlContext.Model.CreatePredictionEngine<MailModelInput, MailModelOutput>(model);


var factory = new ConnectionFactory() { HostName = "localhost" };
using var connection = factory.CreateConnection();
using var channel = connection.CreateModel();

// İki queue'yu da tanımla
channel.QueueDeclare(queue: "spam_request", durable: false, exclusive: false, autoDelete: false);
channel.QueueDeclare(queue: "spam_response", durable: false, exclusive: false, autoDelete: false);

var consumer = new EventingBasicConsumer(channel);
consumer.Received += (model, ea) =>
{
    var body = ea.Body.ToArray();
    var request = JsonSerializer.Deserialize<SpamRequest>(Encoding.UTF8.GetString(body));

    var prediction = engine.Predict(new MailModelInput
    {
        Subject = request.Subject,
        Content = request.Content
    });

    bool isSpam = prediction.IsSpam;

    var response = new SpamResponse { MailId = request.MailId, IsSpam = prediction.IsSpam };
    var responseBody = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(response));

    channel.BasicPublish(exchange: "", routingKey: "spam_response", body: responseBody);
    Console.WriteLine($"MailId: {request.MailId} - IsSpam: {prediction.IsSpam}");
};

channel.BasicConsume(queue: "spam_request", autoAck: true, consumer: consumer);
Console.WriteLine("ML Worker dinleniyor...");

Console.ReadLine();