// See https://aka.ms/new-console-template for more information

using System.Text;
using System.Text.Json.Serialization;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

DotEnv.DotEnv.LoadDotEnvPathFromTheSolutionDirectory();
var factory = new ConnectionFactory
{
    Uri = new Uri(Environment.GetEnvironmentVariable("MQ_URL"))
};

string queueName = Environment.GetEnvironmentVariable("Q_NAME");

using var connection = await factory.CreateConnectionAsync();
using var channel = await connection.CreateChannelAsync();

await channel.QueueDeclareAsync(queue: queueName,
    durable: true,
    exclusive: false,
    autoDelete: false,
    arguments: null
);

var consumer = new EventingBasicConsumer(channel);
consumer.Received += (sender, e) =>
{
    var body = e.Body.ToArray();
    var message = Encoding.UTF8.GetString(body);
    Console.WriteLine(message);
};

await channel.BasicConsumeAsync(queueName, true, consumer);
Console.ReadLine();